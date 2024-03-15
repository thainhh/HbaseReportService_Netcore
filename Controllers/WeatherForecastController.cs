using HbaseReportService;
using HbaseReportService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

            

        private readonly IMyDependencyService _myDependency;
        

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMyDependencyService myDependency)
        {
            _myDependency = myDependency;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

          string _name =   _myDependency.GetName("ThaiNh");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
