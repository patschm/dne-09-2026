using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace EnvConfSec;

internal class Program
{
    static void Main(string[] args)
    {
        //EnvironmentDemo();
        // ConfigDemo();
        //LoggingDemo();

        DIDemo();

        Console.ReadLine();
    }

    private static void DIDemo()
    {
        var factory = new DefaultServiceProviderFactory();
        var services = new ServiceCollection();
       var builder = factory.CreateBuilder(services);
        builder.AddScoped<ICalculator, Adder>();
        builder.AddKeyedScoped<ICalculator, Adder>("add");
        builder.AddKeyedScoped<ICalculator, Subtracter>("sub");
        builder.AddTransient<Executor>();
        var provider = builder.BuildServiceProvider();

        // var op = provider.GetRequiredService<ICalculator>();
        // var op = new Adder();
        //var exec = new Executor(op);
        var sc1 = provider.CreateScope();
        var sc2 = provider.CreateScope();
        for (int i = 0; i < 6; i++)
        {
            
            var exec = sc1.ServiceProvider.GetRequiredService<Executor>();
            int result = exec.Calculate(4, 8);
            Console.WriteLine(result);

            var exec2 = sc2.ServiceProvider.GetRequiredService<Executor>();
            result = exec2.Calculate(4, 8);
            Console.WriteLine(result);
        }
    }

    private static void LoggingDemo()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("settingz.json", false, true);
         IConfiguration conf= builder.Build();

        var factory = LoggerFactory.Create(config => {
            config.AddConfiguration(conf.GetSection("Logging"));
            //config.AddFilter(lvl => lvl >= LogLevel.Trace);
            config.AddConsole();
            config.AddEventLog();
            config.AddDebug();
        });
        // ILogger logger = NullLogger.Instance;

        ILogger logger = factory.CreateLogger("Hallo");

        logger.LogCritical("Critical");
        logger.LogError("Error");
        logger.LogWarning("Warning");
        logger.LogInformation("Information");
        logger.LogDebug("Debug");
        logger.LogTrace("Trees");


        ILogger log = factory.CreateLogger<Program>();

        log.LogCritical("Critical");
        log.LogError("Error");
        log.LogWarning("Warning");
        log.LogInformation("Information");
        log.LogDebug("Debug");
        log.LogTrace("Trees");
    }

    private static void ConfigDemo()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("settingz.json", false, true);
        builder.AddUserSecrets<EnvConfSec.Program>();

        IConfiguration config = builder.Build();
        IConfigurationSection sec = config.GetSection("Hoi");
        //Console.WriteLine(config["Hoi"]);
        Console.WriteLine(sec.Value);

        Console.WriteLine(config["Jan:Voornaam"]);

        var jan = config.GetSection("Jan");
        var ojan = jan.Get<Person>();
        Console.WriteLine(ojan.Achternaam);

        var p = new Person();
        jan.Bind(p);

        Console.WriteLine(ojan.Voornaam);

        jan.GetReloadToken().RegisterChangeCallback((o) => {
            Console.WriteLine("Wijziging");
            Console.WriteLine(p.Achternaam);
        }, ojan);

        Console.WriteLine(config["Geheim"]);

        Console.ReadLine();
    }

    private static void EnvironmentDemo()
    {
        string text = Environment.GetEnvironmentVariable("HELLO") ?? "Hello";
        //string text = "Hallo";

        Console.WriteLine(text);
    }
}
