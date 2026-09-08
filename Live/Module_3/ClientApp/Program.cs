using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using WeerService;

namespace ClientApp;

internal class Program
{
    static void Main(string[] args)
    {
        //Basics();
        //FFStangen();
        Pooling();

    }

    private static void Pooling()
    {
        var factory = new DefaultServiceProviderFactory();
        var services = new ServiceCollection();
        var builder = factory.CreateBuilder(services);
        builder.AddHttpClient("weather", opts =>
        {
            opts.BaseAddress = new Uri("https://localhost:7093/");
        });
        var prov = builder.BuildServiceProvider();
        var cf = prov.GetRequiredService<IHttpClientFactory>();
        
        for (int i = 0; i < 20000; i++)
        {
            var client = cf.CreateClient("weather");
            var res = client.GetAsync("weatherforecast").Result;
            //client.Dispose(); // Gaat niet werken
        }

    }

    private static void FFStangen()
    {
        HttpClient client = new HttpClient();
        //client.DefaultRequestHeaders.Authorization
        client.BaseAddress = new Uri("https://localhost:7093");

        for (int i = 0; i < 20000; i++)
        {
            var res = client.GetAsync("weatherforecast").Result;
            //client.Dispose(); // Gaat niet werken
        }
    }

    private static void Basics()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7093");

        HttpResponseMessage msg = client.GetAsync("weatherforecast").Result;
        if (msg.IsSuccessStatusCode)
        {
            Console.WriteLine(string.Join(':', msg.Headers));
            HttpContent content = msg.Content;
            Console.WriteLine(string.Join(':', content.Headers));
            var stream = content.ReadAsStream();
            var rdr = new StreamReader(stream);
            string data = rdr.ReadToEnd();
            Console.WriteLine(data);
        }

        Console.WriteLine(new string('=', 80));
        string s = client.GetStringAsync("weatherforecast").Result;
        Console.WriteLine(s);

        var w = new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now), Summary = "Best wel fris", TemperatureC = 19 };
        HttpContent ct = new StringContent(JsonSerializer.Serialize(w));
        ct.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var resp = client.PostAsync("weatherforecast", ct).Result;
        Console.WriteLine(resp.StatusCode);
    }
}
