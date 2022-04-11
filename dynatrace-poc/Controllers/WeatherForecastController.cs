using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;

namespace dynatrace_poc.Controllers
{
    [ApiController]
    [Route("/api")]
    public class WeatherForecastController : ControllerBase
    {
        private static ActivitySource activitySource = new ActivitySource("POC");

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly SimpleQueueService _simpleQueue;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _simpleQueue = new SimpleQueueService();
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<WeatherForecast> Get()
        {
            using var activity = activitySource.StartActivity("Get weather", ActivityKind.Server);
            var forecast = Enumerable.Range(1, 5).Select(index => 
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = 0,
                    Summary = Summaries[1],
                    ActivityId = activity.Id
                })
            .ToArray();

            foreach (var f in forecast)
                _simpleQueue.Enqueue(f);

            return forecast;
        }
    }
}