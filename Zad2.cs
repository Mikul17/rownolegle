using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

class Zad2
{
    public static int coalDeposit = 2000;
    public static int vehicleCapacity = 200;
    public static int extractionTimePerUnit = 3;
    public static int unloadingTimePerUnit = 3;
    public static int transportTime = 50;

    public static SemaphoreSlim mineSemaphore = new SemaphoreSlim(2, 2);
    public static SemaphoreSlim warehouseSemaphore = new SemaphoreSlim(1, 1);
    public static object lockObject = new object();
    
    public static int warehouse = 0;

    public static void StartMining(int minerId)
    {
        while (coalDeposit > 0)
        {
            mineSemaphore.Wait();
            int coalExtracted = ExtractCoal(minerId);
            mineSemaphore.Release();

            if (coalExtracted > 0)
            {
                TransportToWarehouse(minerId);
                warehouseSemaphore.Wait();
                UnloadCoal(minerId, coalExtracted);
                warehouseSemaphore.Release();
            }
            DisplayStatus();
        }
    }

    static int ExtractCoal(int minerId)
    {
        lock (lockObject)
        {
            if (coalDeposit > 0)
            {
                int coalToExtract = Math.Min(vehicleCapacity, coalDeposit);
                coalDeposit -= coalToExtract;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Górnik {minerId} wydobył {coalToExtract} jednostek węgla.");
                Thread.Sleep(coalToExtract * extractionTimePerUnit);
                return coalToExtract;
            }
            return 0;
        }
    }

    static void TransportToWarehouse(int minerId)
    {
        Console.WriteLine($"Górnik {minerId} transportuje węgiel do magazynu...");
        Thread.Sleep(transportTime);
    }

    static void UnloadCoal(int minerId, int coalAmount)
    {
        warehouse += coalAmount;
        Console.WriteLine($"Górnik {minerId} rozładowuje {coalAmount} jednostek węgla.");
        Thread.Sleep(coalAmount * unloadingTimePerUnit);
    }

    static void DisplayStatus()
    {
        Console.SetCursorPosition(0, 5);
        Console.WriteLine($"Stan złoża: {coalDeposit} jednostek węgla, Stan magazynu: {warehouse} jednostek węgla.");
    }
}