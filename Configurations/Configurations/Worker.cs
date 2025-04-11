using Configurations;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _httpClient;
    private readonly WorkerSettings _settings;

    public Worker(
        ILogger<Worker> logger,
        HttpClient httpClient,
        IOptions<WorkerSettings> options)
    {
        _logger = logger;
        _httpClient = httpClient;
        _settings = options.Value;

        ValidateSettings();
    }

    private void ValidateSettings()
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(_settings);
        
        if (!Validator.TryValidateObject(_settings, validationContext, validationResults, true))
        {
            var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
            throw new InvalidOperationException($"Invalid configuration: {errors}");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started with settings: {@Settings}", _settings);

        while (!stoppingToken.IsCancellationRequested)
        {
            bool success = false;
            Exception lastException = null;

            for (int i = 0; i < _settings.RetryCount; i++)
            {
                try
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_settings.TimeoutSeconds));
                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, cts.Token);

                   
                    var response = await _httpClient.GetAsync(_settings.ApiUrl, linkedCts.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Data fetched successfully!");
                        success = true;
                        break;
                    }

                    _logger.LogWarning("API request failed with status code {StatusCode}. Attempt {Attempt}/{TotalAttempts}");
                }
                catch (TaskCanceledException ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogWarning(ex, "Request timed out. Attempt {Attempt}/{TotalAttempts}", 
                        i + 1, _settings.RetryCount);
                    lastException = ex;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error during API request. Attempt {Attempt}/{TotalAttempts}", 
                        i + 1, _settings.RetryCount);
                    lastException = ex;
                }
            }
            await Task.Delay(_settings.DelayMilliseconds, stoppingToken);
        }
    }
}