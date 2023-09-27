using Flurl;
using Serialization.Domain.Interfaces;

namespace Serialization.Services
{
    public class WorkloadService
    {
        private bool _executing;
        private readonly VideoService _videoService = new();
        private readonly HttpClient _httpClient = new();
        private readonly RestClient _restClient = new();
        private const string SenderBaseUrl = "http://localhost:5011/";

        public async Task DispatchAsync(ISerializer serializer, int numHosts = 100, int messagedPerSecond = 100, bool rest = false)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, SenderBaseUrl + $"workload/"
                    .AppendPathSegment(serializer.ToString())
                    .SetQueryParam("numHosts", numHosts)
                    .SetQueryParam("messagedPerSecond", messagedPerSecond)
                    .SetQueryParam("rest", rest));

                var response = await _httpClient.PostAsync(request.RequestUri, null);

                // Handle the response as needed.
                if (response.IsSuccessStatusCode)
                {
                    // Successfully received a response.
                    var content = await response.Content.ReadAsStringAsync();
                    // Process the content.
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions.
                throw ex;
            }
        }


        public async Task RunParallelRestAsync(ISerializer serializer, int numHosts = 100, int messagedPerSecond = 100)
        {
            if (_executing)
            {
                _executing = false;
                Thread.Sleep(200);
            }

            _executing = true;
            SemaphoreSlim semaphore = new SemaphoreSlim(numHosts);
            List<Task> tasks = new();

            for (int i = 0; i < numHosts; i++)
            {
                tasks.Add(ProcessSerializationRestAsync(semaphore, i, serializer, messagedPerSecond));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All tasks completed.");
        }

        public async Task RunParallelAsync(ISerializer serializer, int numThreads = 100, int numMessages = 100)
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
                tasks.Add(ProcessSerialization(semaphore, i, serializer, numMessages));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All tasks completed.");
        }


        private async Task ProcessSerializationRestAsync(SemaphoreSlim semaphore, int threadId, ISerializer serializer, int messagesPerSecond)
        {
            while (_executing)
            {
                await semaphore.WaitAsync();

                try
                {
                    var delayMilliseconds = 1000 / messagesPerSecond;

                    while (true)
                    {
                        var vid = _videoService.CreateVideo();
                        vid.Serialize(serializer);
                        object result;

                        if (serializer.GetSerializationResult(vid.GetType(), out var serializationObject))
                        {
                            result = _restClient.PostAsync($"receiver/{serializer.ToString()}", serializationObject).Result;
                        }

                        await Task.Delay(delayMilliseconds);
                    }

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


        private async Task ProcessSerialization(SemaphoreSlim semaphore, int threadId, ISerializer serializer, int numMessages)
        {
            while (_executing)
            {
                await semaphore.WaitAsync();

                try
                {
                    for (int i = 0; i < numMessages; i++)
                    {
                        var vid = _videoService.CreateVideo();
                        vid.Serialize(serializer);
                        vid.Deserialize(serializer);
                    }
                    await Task.Delay(100);
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