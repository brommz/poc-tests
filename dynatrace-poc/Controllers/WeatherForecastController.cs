using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;
using OpenTelemetry;

namespace dynatrace_poc.Controllers
{
    [ApiController]
    [Route("/api")]
    public class WeatherForecastController : ControllerBase
    {
        private static ActivitySource ActivitySource = new ActivitySource(TelemetryConsts.ServiceName);

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private static SimpleQueueService SimpleQueue = new SimpleQueueService();

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<WeatherForecast> Get()
        {
            using var activity = ActivitySource.StartActivity("Get weather", ActivityKind.Producer);
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
                SimpleQueue.Enqueue(f);

            return forecast;
        }
    }
}