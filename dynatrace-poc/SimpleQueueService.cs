using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;

namespace dynatrace_poc 
{
    internal class SimpleQueueService 
    {
        private static ActivitySource activitySource = new ActivitySource("POC", "semver1.0.0");

        private static ConcurrentQueue<WeatherForecast> GlobalQueue = new ConcurrentQueue<WeatherForecast>();
        private Thread _worker;

        public SimpleQueueService() 
        {
            _worker = new Thread(() =>
                {
                    if (GlobalQueue.TryDequeue(out var r))
                    {
                        var parentId = r.ActivityId;
                        using var activity = activitySource.StartActivity("Dequeue", ActivityKind.Internal, parentId);
                        Thread.Sleep(300);
                    }
                });
            _worker.Start();
        }

        public void Enqueue(WeatherForecast forecast) 
        {
            GlobalQueue.Enqueue(forecast);
        }
     }
}