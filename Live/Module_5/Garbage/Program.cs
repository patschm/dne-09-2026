namespace Garbage;

internal class Program
{

    static void Main(string[] args)
    {
        DoeIets();
        //GC.Collect();
        //GC.WaitForPendingFinalizers();
        DoeNogIets();
        DoeWeerIets();
        Console.ReadLine();
    }

    private static void DoeWeerIets()
    {
        var u1 = new UnmanagedResource();
        try
        {
            u1.Open();
        }
        finally
        {
            u1.Dispose();
        }
    }
    private static void DoeNogIets()
    {
        var u1 = new UnmanagedResource();
        using (u1)
        {
            u1.Open();
        }
        //u1.Dispose();
    }

    private static void DoeIets()
    {
        using (var u1 = new UnmanagedResource())
        {
            u1.Open();
        }
        
    }
}
