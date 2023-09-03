using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;

namespace Serialization.Services
{
    public class SenderService
    {
        private bool _executing;
        private readonly IVideoService _videoService;
        private static readonly RestClient _restClient = new();

        public SenderService(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task RunParallelProcessingAsync(int numThreads = 10, ISerializer serializer)
        {
            if (_executing)
            {
                _executing = false; // Para o serviço anterior antes de iniciar um novo
                Thread.Sleep(200);
            }

            _executing = true;

            SemaphoreSlim semaphore = new SemaphoreSlim(numThreads);

            List<Task> tasks = new();

            for (int i = 0; i < numThreads; i++)
            {
                tasks.Add(ProcessAsync(semaphore, i, serializer));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("All tasks completed.");
        }

        private async Task ProcessAsync(SemaphoreSlim semaphore, int threadId, ISerializer serializer)
        {
            while (_executing)
            {
                await semaphore.WaitAsync();

                try
                {
                    Console.WriteLine($"Thread {threadId} started");

                    var vid = _videoService.CreateVideo();
                    var vidSerialized = vid.Serialize<byte[]>(serializer);
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