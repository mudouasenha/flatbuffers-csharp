namespace Serialization.Receiver
{
    public class RequestCounterMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestCounterMiddleware> logger;
        private readonly IRequestCounter requestCounter;

        public RequestCounterMiddleware(RequestDelegate next, ILogger<RequestCounterMiddleware> logger, IRequestCounter requestCounter)
        {
            this.next = next;
            this.logger = logger;
            this.requestCounter = requestCounter;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            requestCounter.RecordRequest();
            await next(context);
        }
    }
}
