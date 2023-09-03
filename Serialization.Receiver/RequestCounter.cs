using System.Collections.Concurrent;

namespace Serialization.Receiver
{
    public class RequestCounter : IRequestCounter, IDisposable
    {
        private int requestCount;
        private bool isMonitoring;
        private readonly object lockObject = new object();
        private readonly ILogger<RequestCounter> logger;
        private readonly ConcurrentQueue<int> requestCounts;
        private readonly Timer monitoringTimer;

        public RequestCounter(ILogger<RequestCounter> logger)
        {
            requestCount = 0;
            isMonitoring = true;
            this.logger = logger;
            requestCounts = new ConcurrentQueue<int>();
            monitoringTimer = new Timer(MonitoringCallback, null, 1000, 1000); // Start monitoring every second
        }

        public void RecordRequest()
        {
            Interlocked.Increment(ref requestCount);
        }

        public int GetRequestsPerSecond()
        {
            int totalRequests = 0;
            while (requestCounts.TryDequeue(out int count))
            {
                totalRequests += count;
            }
            return totalRequests;
        }

        private void MonitoringCallback(object state)
        {
            requestCounts.Enqueue(requestCount);
            requestCount = 0;

            int requestsPerSecond = GetRequestsPerSecond();
            logger.LogInformation($"Requests per second: {requestsPerSecond}");
        }

        public void Dispose()
        {
            isMonitoring = false;
            monitoringTimer.Dispose();
        }
    }
}
