using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using GrpcServiceLab.Controllers;
using HbaseReportService.Services;
using Microsoft.Extensions.Logging;
using HbaseReportService.Services.Base;
using System.ComponentModel.Design;
using HbaseReportService.Services.ReportStops;

namespace HbaseReportService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportStopController : Controller
    {
        private static readonly string[] Summaries = new[]
      {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ILogger<ReportStopController> _logger;
        private readonly IReportStopService _reportStopService;
        
        public ReportStopController(ILogger<ReportStopController> logger, IReportStopService reportStopService)
        {
            _reportStopService = reportStopService;
            
        }


        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            long[] vehicleIds = new long[] { 485127 };

            var data = _reportStopService.GetReportStop();

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
