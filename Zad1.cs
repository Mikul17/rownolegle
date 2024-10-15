using System;
using System.Threading;
using System.Threading.Tasks;

class Zad1
{
    static int coalDeposit = 2000;
    static int vehicleCapacity = 200;
    static int extractionTimePerUnit = 10;
    static int unloadingTimePerUnit = 10;
    private static int transportTime = 10000;

    static SemaphoreSlim mineSemaphore = new SemaphoreSlim(2, 2);
    static SemaphoreSlim warehouseSemaphore = new SemaphoreSlim(1, 1);
    static object lockObject = new object();

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
                Console.WriteLine($"Górnik {minerId} wydobył {coalToExtract} jednostek węgla. Pozostało w złożu: {coalDeposit} jednostek.");
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
        Console.WriteLine($"Górnik {minerId} rozładowuje {coalAmount} jednostek węgla.");
        Thread.Sleep(coalAmount * unloadingTimePerUnit);
    }
}