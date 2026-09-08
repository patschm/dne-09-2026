// TODO 1: Include the necessary packages for dependency Injection
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shapes;


namespace DrawNotSoPerfect;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // TODO 2: Modify this code to wire up the Dependency Injection infrastructure.
        var dip = new DefaultServiceProviderFactory();
        var services = new ServiceCollection();
        var bld =dip.CreateBuilder(services);
        services.AddSingleton<Form, DrawMain>();
        
        

        // TODO 3: Register LocalFileStorage class in the Dependency Injector
        services.AddSingleton<IStorage, LocalFileStorage>();

        // TODO 5: Configure logging. Clear all log providers and add the Debug Provider
        //var loggerFactory = LoggerFactory.Create(c =>
        //{
        //    c.ClearProviders();
        //    c.AddConsole();

        //});
        //services.AddSingleton(loggerFactory);
        //services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

        services.AddLogging(c => {
            c.ClearProviders();
            c.AddDebug();
        });

        var prov = bld.BuildServiceProvider();

        // (writes output to "Output" window in Visual Studio).
        ApplicationConfiguration.Initialize();

        var form = prov.GetRequiredService<Form>();
        //var host = CreateHostBuilder().Build();
        //var form = host.Services.GetRequiredService<DrawMain>();
        //IStorage stor = host.Services.GetRequiredService<IStorage>();
       // var form = new DrawMain(stor);
        Application.Run(form);
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            //.ConfigureAppConfiguration((ctx, cf) => {
            //    cf.AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json");
            //})
            .ConfigureLogging(bld =>
            {
                bld.ClearProviders();
                bld.AddDebug();
            })
            .ConfigureServices((ctx, services) => {
                services.AddTransient<DrawMain>();
                services.AddSingleton<IStorage, LocalFileStorage>();
            });
    }
}