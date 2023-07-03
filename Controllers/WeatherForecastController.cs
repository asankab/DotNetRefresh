using BenchmarkDotNet.Attributes;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pros.Application.Behaviours;
using Pros.Application.Results;
using Pros.Database;
using Pros.Models;
using Pros.Service;
using System;
using System.Collections.Generic;

namespace Pros.Controllers
{

   //ApiVersion("1.0"), Deprecated = true]
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
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

        private static readonly Func<SchoolDatabaseContext, int, Task<Student?>> SingleStudentAsync =
            EF.CompileAsyncQuery(
                    (SchoolDatabaseContext context, int id) =>
                        context.Students.SingleOrDefault(x => x.ID == 1)
                );

        //[Benchmark]
        [HttpGet("students", Name = "GetStudents")]
        //public async Task<Result<List<Student>>> GetStudents()
        public async Task<Result> GetStudents()
        {
            var result = new Result();

            //try
            //{
            // throw new ApplicationException("Error occured");
            //var students = await _dbContext.Students.SingleOrDefaultAsync(x => x.ID  == 1);
            var students = await SingleStudentAsync(_dbContext, 1);

            _logger.LogInformation($"Data returned");

            result.Data = students != null ? students : new Student();
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

        private static readonly Func<SchoolDatabaseContext, int, IAsyncEnumerable<Student>> GetStudentAsync =
            EF.CompileAsyncQuery(
                    (SchoolDatabaseContext context, int id) =>
                        context.Students.Where(x => x.ID == id)
                );


        //[Benchmark]
        [HttpGet("users", Name = "GetUsers")]
        //public async Task<Result<List<Student>>> GetStudents()
        public async Task<Result> GetUsers()
        {
            var result = new Result();

            var studentList = new List<Student>();

            // throw new ApplicationException("Error occured");
            //var students = await _dbContext.Students.ToListAsync();
            await foreach (var item in GetStudentAsync(_dbContext, 1))
            {
                studentList.Add(item);
            }

            //hrow new ApplicationException("Error occured");

            if (studentList.Count == 0)
            {
                result.Error = new ApplicationException("No records found.");
            }

            _logger.LogInformation($"Number of records returned count: {studentList.Count}");

            result.Data = studentList;

            return result;
        }

        private List<Driver> drivers = new List<Driver>();

        [HttpPost("drivers", Name = "AddDriver")]
        public IActionResult AddDriver(Driver driver)
        {
            if (ModelState.IsValid)
            {
                drivers.Add(driver);

                var jobId = BackgroundJob.Enqueue<IServiceManagement>(x => x.SendEmail());

                return CreatedAtAction(nameof(AddDriver), new { driver.Id }, driver);
            }

            return BadRequest();
        }


        [HttpGet("drivers", Name = "GetDrivers")]
        public IActionResult GetDriver(Guid id)
        {
            var driver = drivers.FirstOrDefault(x => x.Id == id);

            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }


        [HttpDelete("drivers", Name = "DeleteDrivers")]
        public IActionResult DeleteDriver(Guid id)
        {
            var driver = drivers.FirstOrDefault(x => x.Id == id);

            if (driver == null)
            {
                return NotFound();
            }

            driver.Status = 0;

            //CRON: https://freeformatter.com/cron-expression-generator-quartz.html
            //7253/hangfire
            RecurringJob.AddOrUpdate<IServiceManagement>("MyOtherJob",x => x.UpdateDatabase(), Cron.Hourly);

            return NoContent();
        }
    }
}