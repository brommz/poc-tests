using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using OpenTelemetry;

namespace dynatrace_poc 
{
    internal class SimpleQueueService 
    {
        private static ActivitySource ActivitySource = new ActivitySource(TelemetryConsts.ServiceName);
        private static ConcurrentQueue<WeatherForecast> GlobalQueue = new ConcurrentQueue<WeatherForecast>();
        private static Thread Worker;

        public SimpleQueueService() 
        {
            Worker = new Thread(() =>
                {
                    while(true) 
                    {
                        if (GlobalQueue.TryDequeue(out var r))
                        {
                            var parentId = r.ActivityId;
                            using var activity = ActivitySource.StartActivity("Dequeue", ActivityKind.Consumer, parentId);
                            activity.AddTag("tempC", r.TemperatureC);
                        }
                        Thread.Sleep(3000);
                    }
                });
            Worker.Start();
        }

        public void Enqueue(WeatherForecast forecast) 
        {
            GlobalQueue.Enqueue(forecast);
        }
     }
}