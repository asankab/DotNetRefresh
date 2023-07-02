using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pros.Application.Behaviours;
using Pros.Application.Results;
using Pros.Database;
using System;
using System.Collections.Generic;

namespace Pros.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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

        [HttpGet("students", Name = "GetStudents")]
        //public async Task<Result<List<Student>>> GetStudents()
        public async Task<Result> GetStudents()
        {
            var result = new Result();

            //try
            //{
                // throw new ApplicationException("Error occured");
                var students = await _dbContext.Students.ToListAsync();

                if (students.Count == 0)
                {
                    result.Error = new ApplicationException("No records found.");
                }

                _logger.LogInformation($"Number of records returned count: {students.Count}");

                result.Data = students;
            //}
            //catch (Exception exception)
            //{
            //    _logger.LogError(exception, "An unhandled exception occured");
            //    result.isFailuer = true;

            //    throw;
            //}

            return result;

            //var response = await _weatherClient.GetDataAsync();
            //return response;
        }


        [HttpGet("users", Name = "GetUsers")]
        //public async Task<Result<List<Student>>> GetStudents()
        public async Task<Result> GetUsers()
        {
            var result = new Result();

                // throw new ApplicationException("Error occured");
                var students = await _dbContext.Students.ToListAsync();

                throw new ApplicationException("Error occured");
                
                if (students.Count == 0)
                {
                    result.Error = new ApplicationException("No records found.");
                }

                _logger.LogInformation($"Number of records returned count: {students.Count}");

                result.Data = students;

            return result;
        }
    }
}