using Serialization.Domain.Interfaces;

namespace Serialization.Services
{
    public class SenderService
    {
        private bool _executing;
        private readonly VideoService _videoService = new();
        private static readonly RestClient _restClient = new();

        public async Task RunParallelProcessingAsync(ISerializer serializer, int numThreads = 100, int numMessages = 100)
        {
            if (_executing)
            {
                _executing = false;
                Thread.Sleep(200);
            }

            _executing = true;

            SemaphoreSlim semaphore = new SemaphoreSlim(numThreads);

            List<Task> tasks = new();

            for (int i = 0; i < numThreads; i++)
            {
                tasks.Add(ProcessAsync(semaphore, i, serializer, numMessages));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("All tasks completed.");
        }

        private async Task ProcessAsync(SemaphoreSlim semaphore, int threadId, ISerializer serializer, int numMessages)
        {
            while (_executing)
            {
                await semaphore.WaitAsync();

                try
                {
                    Console.WriteLine($"Thread {threadId} loop started");

                    for (int i = 0; i < numMessages; i++)
                    {
                        var vid = _videoService.CreateVideo();
                        vid.Serialize(serializer);
                        object result;
                        if (serializer.GetSerializationResult(vid.GetType(), out var serializationObject))
                            result = _restClient.PostAsync("receiver/video", serializationObject).Result;
                    }
                    await Task.Delay(100);

                    Console.WriteLine($"Thread {threadId} finished");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

    }
}