using System.Diagnostics;

namespace Lab_1___programowanie_wspolbiezne;

public class Solution
{
    static void Main(string[] args)
    {
        //Zad 1
        // Task[] miners = new Task[5];
        //
        // for (int i = 0; i < miners.Length; i++)
        // {
        //     int minerId = i + 1;
        //     miners[i] = Task.Run(() => Zad1.StartMining(minerId));
        // }
        //
        // Task.WaitAll(miners);
        // Console.WriteLine("Symulacja zakończona.");
        
        //Zad 2
        for (int minerCount = 1; minerCount <= 6; minerCount++)
        {
            Zad2.coalDeposit = 2000;
            Zad2.warehouse = 0;

            Console.WriteLine($"\nSymulacja dla {minerCount} górników:");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Task[] miners = new Task[minerCount];
            for (int i = 0; i < minerCount; i++)
            {
                int minerId = i + 1;
                miners[i] = Task.Run(() => Zad2.StartMining(minerId));
            }

            Task.WaitAll(miners);
            stopwatch.Stop();

            double timeTaken = stopwatch.Elapsed.TotalSeconds;
            double speedup = 163.95 / timeTaken;
            double efficiency = speedup / minerCount;

            Console.WriteLine($"Czas: {timeTaken:F2} s, Przyspieszenie: {speedup:F2}, Efektywność: {efficiency:F2}");
        }
    }
}