using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pros.Database;

namespace Pros.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherClient _weatherClient;
        public SchoolDatabaseContext _dbContext { get; set; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherClient weatherClient, SchoolDatabaseContext dbContext)
        {
            _logger = logger;
            _weatherClient = weatherClient;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetWeather")]
        //public async Task<List<object>> GetWeather()
        public async Task<List<Student>> GetWeather()
        {

            var students = _dbContext.Students.ToList();
            return students;

            //var response = await _weatherClient.GetDataAsync();
            //return response;
        }
    }
}