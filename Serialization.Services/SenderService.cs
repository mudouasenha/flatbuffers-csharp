using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;

namespace Serialization.Services
{
    public class SenderService
    {
        private readonly IVideoService _videoService;
        private static readonly RestClient _restClient = new();
        private static readonly VideoFlatBuffersConverter _videoconverter = new();

        public SenderService(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task RunParallelProcessingAsync(int numThreads = 10)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(numThreads);

            List<Task> tasks = new();

            for (int i = 0; i < numThreads; i++)
            {
                tasks.Add(ProcessAsync(semaphore, i));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("All tasks completed.");
        }

        private async Task ProcessAsync(SemaphoreSlim semaphore, int threadId)
        {
            while (true) // Run continuously
            {
                await semaphore.WaitAsync();

                try
                {
                    Console.WriteLine($"Thread {threadId} started");

                    var vid = _videoService.CreateVideo();
                    var vidSerialized = _videoconverter.Serialize(vid);
                    var result = await _restClient.PostAsync("receiver/video", vidSerialized);
                    await Task.Delay(100);

                    Console.WriteLine($"Thread {threadId} finished");
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

    }
}