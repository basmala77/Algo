using System.ComponentModel.DataAnnotations;

namespace Configurations
{
    public class WorkerSettings
    {
        [Required(ErrorMessage = "API URL is required")]
        [Url(ErrorMessage = "API URL must be a valid URL")]
        public string ApiUrl { get; set; }

        [Range(1, 10, ErrorMessage = "Retry count must be between 1 and 10")]
        public int RetryCount { get; set; }

        [Range(1000, 30000, ErrorMessage = "Delay must be between 1000 and 30000 milliseconds")]
        public int DelayMilliseconds { get; set; }

        [Range(1, 60, ErrorMessage = "Timeout must be between 1 and 60 seconds")]
        public int TimeoutSeconds { get; set; }

    }
}
