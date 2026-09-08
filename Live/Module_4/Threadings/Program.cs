using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Threadings;

internal class Program
{
    static async Task Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (o, e) => Console.WriteLine(e.ExceptionObject);
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nl-NL");
        Console.WriteLine(Thread.CurrentThread.CurrentCulture);
        Console.WriteLine($"ThreadID = {Thread.CurrentThread.ManagedThreadId}");
        // SynchronousDemo();
        //ASynchronousDemo1();
        //ASynchronousDemo2();
        //ASynchronousDemo3();
        //ASynchronousDemo4();
        //int res = await ASynchronousDemoHip();
        try
        {
            //AsynErrorsDemo().ContinueWith(pt => {
            //    Console.WriteLine(pt.Status);
            //    Console.WriteLine(pt.Exception);
            //});
            //int res = await LongAddAsync(6, 7);
            //Console.WriteLine(res);
            //await AsynErrorsDemo();
           
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }
        //EchtMultiThreading();
        //EchtMultiThreading1();
        //EchtMultiThreading2();
        //EchtMultiThreading3();
       // EchtMultiThreading4();
        EchtMultiThreading5();
        Console.WriteLine($"We zijn aan het eind van de hoofddraad gekomen {3.14}");
        Console.ReadLine();
    }

    private static void EchtMultiThreading5()
    {
        //ArrayList list = new ArrayList();
        //list.Sy
        //List<int> list = new List<int>();
        
        ConcurrentBag<int> list = new ConcurrentBag<int>();
        list.Add(1);

        ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
        queue.Enqueue(1);



    }

    private static void EchtMultiThreading4()
    {
        CountdownEvent cde = new CountdownEvent(10);
        Parallel.For(0, 10, idx => {
            Task.Delay(100).Wait();
            cde.Signal();
            Console.WriteLine($"{idx} is klaar");
        });
        cde.Wait();
        Console.WriteLine("En dooorrrrrrrrrr");

        Barrier barriere = new Barrier(10);
        Parallel.For(0, 10, idx => {
           // barriere.AddParticipant();
            Task.Delay(100).Wait();
            barriere.SignalAndWait();
            Console.WriteLine($"{idx} is klaar");
        });

    }

    private static void EchtMultiThreading3()
    {
        ThreadPool.SetMinThreads(50, 0);
        Semaphore parking = new Semaphore(10, 10);
        Parallel.For(0, 50, idx => {
            Console.WriteLine($"Auto met kenteken {idx} rijdt naar de slagboom");
            parking.WaitOne();
          //  Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"=== Auto met kenteken {idx} rijdt naar binnen");
            Task.Delay(5000 + Random.Shared.Next(5000, 10000)).Wait();
            //Console.ResetColor();
            parking.Release();
            Console.WriteLine($"Auto met kenteken {idx} naar buiten");
        });
    }

    static object stokje = new object();

    private static void EchtMultiThreading2()
    {
        int counter = 0;
        //ThreadPool.SetMinThreads(50, 0);
        Parallel.For(0, 50, idx => {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} started");
            //Monitor.Enter(stokje);
            lock (stokje)
            {
                int tmp = counter;
                Task.Delay(100).Wait();
                counter = ++tmp;
            }
            //Monitor.Exit(stokje);
            Console.WriteLine(counter);
        });
    }

    private static async void EchtMultiThreading1()
    {
        var zl1 = new ManualResetEvent(false);
        var zl2 = new ManualResetEvent(false);

        int a = 0,b = 0;
        var t1 = Task.Run(() =>
        {
            Task.Delay(1000).Wait();
            a = 100;
            //zl1.Set();
        });

        var t2 =Task.Run(() =>
        {
            Task.Delay(2000).Wait();
            b = 200;
            //zl2.Set();
        });

        //WaitHandle.WaitAny([zl1, zl2]);
        //Task.WaitAll(t1, t2);
        await Task.WhenAll(t1, t2);
        int result = a + b;
        Console.WriteLine(result);
    }

    private static void EchtMultiThreading()
    {
        CancellationTokenSource nikko = new CancellationTokenSource();
        CancellationToken bommetje = nikko.Token; //CancellationToken.None;
        var t1 =Task.Run(() => {
            for (int i = 0; i < 1000; i++)
            {
                //if (bommetje.IsCancellationRequested) return;
                bommetje.ThrowIfCancellationRequested();
                Console.WriteLine($"{i}e loop");
                Task.Delay(100).Wait();
            }      
        });

        Task.Delay(5000).Wait();
        nikko.Cancel();
        Task.Delay(100).Wait();
        Console.WriteLine(t1.Status);
        Console.WriteLine(t1.IsCanceled);
    }

    private static Task AsynErrorsDemo()
    {
        return Task.Run(() => {
            Console.WriteLine("Doe Iets");
            Task.Delay(1000).Wait();
            throw new Exception("Ooops");
        });
    }

    private static async Task<int> ASynchronousDemoHip()
    {
        var t2 = Task.Run<int>(() => LongAdd(2, 3));
        int result = await t2;
        Console.WriteLine(result);
        Console.WriteLine("En we gaan door..");
        result = await Task.Run(() => LongAdd(5, 6));
        Console.WriteLine(result);


        return 42;

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
        Console.WriteLine($"ThreadID = {Thread.CurrentThread.ManagedThreadId}");
        Task.Delay(5000).Wait();
        return a + b;
    }
    static Task<int> LongAddAsync(int a, int b)
    {
        return Task.Run<int>(() => LongAdd(2, 3));
    }
}
