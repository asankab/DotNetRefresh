using Microsoft.AspNetCore.Mvc;

namespace Pros.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherClient _weatherClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherClient weatherClient)
        {
            _logger = logger;
            _weatherClient = weatherClient;
        }

        [HttpGet(Name = "GetWeather")]
        public async Task<List<object>> GetWeather()
        {
            var response = await _weatherClient.GetDataAsync();
            return response;
        }
    }
}