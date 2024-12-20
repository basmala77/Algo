using System;

class Heapsort
{
    static void Heapify(int[] arr, int n, int i)
    {
        int largest = i; 
        int left = 2 * i + 1; 
        int right = 2 * i + 2;

        if (left < n && arr[left] > arr[largest])
        {
            largest = left;
        }

        if (right < n && arr[right] > arr[largest])
        {
            largest = right;
        }

        if (largest != i)
        {
            int temp = arr[i];
            arr[i] = arr[largest];
            arr[largest] = temp;

            Heapify(arr, n, largest);
        }
    }
    static void BuildMaxHeap(int[] arr)
    {
        int n = arr.Length;
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            Heapify(arr, n, i);
        }
    }

    public static void PerformHeapsort(int[] arr)
    {
        int n = arr.Length;
        BuildMaxHeap(arr);
        for (int i = n - 1; i > 0; i--)
        {
           
            int temp = arr[0];
            arr[0] = arr[i];
            arr[i] = temp;
            Heapify(arr, i, 0);
        }
    }

    static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine());
        int[] numbers = new int[n];

        for (int i = 0; i < n; i++)
        {
            Console.Write($"Element {i + 1}: ");
            numbers[i] = int.Parse(Console.ReadLine()); 
        }
        Console.WriteLine("Original array:");
        foreach (int num in numbers)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
        PerformHeapsort(numbers);

        Console.WriteLine("Sorted array:");
        foreach (int num in numbers)
        {
            Console.Write(num + " ");
        }
    }
}