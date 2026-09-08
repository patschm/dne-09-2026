using System.Diagnostics;

namespace Threadings;

internal class Program
{
    static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nl-NL");
        Console.WriteLine(Thread.CurrentThread.CurrentCulture);
        // SynchronousDemo();
        //ASynchronousDemo1();
        //ASynchronousDemo2();
        //ASynchronousDemo3();
        ASynchronousDemo4();
        Console.WriteLine($"We zijn aan het eind van de hoofddraad gekomen {3.14}");
        Console.ReadLine();
    }

    private static void ASynchronousDemo4()
    {
        var t1 = Task.Run(() => Console.WriteLine("Taak 1"));
        t1.ContinueWith(pt => Console.WriteLine("Taak2"));
        t1.ContinueWith(pt => Console.WriteLine("Taak3"))
            .ContinueWith(pt=> Console.WriteLine("Taak 4"))
            .ContinueWith (pt=> Console.WriteLine(pt.IsCompleted));
    }

    private static void ASynchronousDemo3()
    {
        var t2 = Task.Run<int>(() => LongAdd(2, 3));

        Task<int> t1 = new Task<int>(() =>
        {
            int res = LongAdd(3, 4);
             return res;
        });
        t1.ContinueWith(prevTask => {
            int res = t1.Result;
            Console.WriteLine(res);
        });
        t1.Start();
        
    }

    private static void ASynchronousDemo2()
    {
        // Ouwe meuk. Werkt niet op moderne .NET. Alleen op .NET Framework
        Func<int, int, int> del = LongAdd;

        //int res = del.Invoke(2, 3);
        IAsyncResult ar = del.BeginInvoke(2, 3, arr => {
            var dl = arr.AsyncState as Func<int, int, int>;
            int res = dl!.EndInvoke(arr);
            Console.WriteLine(res);
        }, del);
        
        int res = del.EndInvoke(ar);

        Console.WriteLine(res);

    }

    private static void ASynchronousDemo1()
    {
        Thread t = new Thread(() =>
        {
            Thread.Sleep(1000);
            Console.WriteLine($"Hoi {3.14}");
        });
        t.Start();

        //ThreadPool.SetMinThreads(100, 0);
        Stopwatch w = new Stopwatch();
        w.Start();
        for (int i = 0; i < 100; i++)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                Console.WriteLine($"Thread {i}");
                Thread.Sleep(100);
                Console.WriteLine($"Hoi {3.14}");
            });
        }
        w.Stop();
        //ThreadPool.RegisterWaitForSingleObject()
        Console.WriteLine($"Duurde {w.Elapsed} s");
    }

    private static void SynchronousDemo()
    {
        int result = LongAdd(2, 3);
        Console.WriteLine($"Het resultaat is {result}");
    }

    static int LongAdd(int a, int b)
    {
        Task.Delay(5000).Wait();
        return a + b;
    }
}
