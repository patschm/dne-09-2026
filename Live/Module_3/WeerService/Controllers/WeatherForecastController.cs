using Microsoft.AspNetCore.Mvc;

namespace WeerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private readonly List<WeatherForecast> _forecasts;

        public WeatherForecastController()
        {
            _forecasts = Enumerable.Range(1, 15).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToList();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return _forecasts;
        }
        [HttpPost(Name = "PostWeatherForecast")]
        public IActionResult Post([FromBody]WeatherForecast weer)
        {
            _forecasts.Add(weer);
            return Accepted();
        }
    }
}
