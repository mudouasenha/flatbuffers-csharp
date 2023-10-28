using Flurl;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;

namespace Serialization.Services
{
    public class WorkloadService
    {
        private bool _executing;
        private readonly HttpClient _httpClient = new();
        private readonly RestClient _restClient = new();
        private const string SenderBaseUrl = "http://localhost:5010/";
        private const int delayMilliseconds = 1000 / 20;

        public async Task DispatchAsync(ISerializer serializer, string serializationType, int numThreads = 100)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, SenderBaseUrl + "sender/workload"
                    .SetQueryParam("serializerType", serializer.ToString())
                    .SetQueryParam("serializationType", serializationType)
                .SetQueryParam("numThreads", numThreads));

                var response = await _httpClient.PostAsync(request.RequestUri, null);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    await Console.Out.WriteLineAsync("Dispatch ok");
                }
                else
                {
                    await Console.Out.WriteLineAsync("Dispatch Not ok");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public async Task RunParallelRestAsync(ISerializer serializer, Type serializationTargetType, int numHosts = 100)
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
                tasks.Add(ProcessSerializationRestAsync(semaphore, i, serializer, serializationTargetType));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All tasks completed.");
        }

        private async Task ProcessSerializationRestAsync(SemaphoreSlim semaphore, int threadId, ISerializer serializer, Type serializationTargetType)
        {
            while (_executing)
            {
                await semaphore.WaitAsync();

                try
                {
                    while (true)
                    {
                        //var rand = new Random().Next(10);
                        //if (rand % 3 == 0) continue;

                        var obj = GenerateObject(serializationTargetType);
                        obj.Serialize(serializer);
                        object result;

                        if (serializer.GetSerializationResult(obj.GetType(), out var serializationObject))
                        {
                            result = await _restClient.PostAsync($"receiver/serializer", serializationObject);
                        }

                        await Task.Delay(delayMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

        private ISerializationTarget GenerateObject(Type type)
        {
            return type.Name switch
            {
                "Video" => new VideoBuilder().Generate(),
                "SocialInfo" => new SocialInfoBuilder().Generate(),
                "VideoInfo" => new VideoInfoBuilder().Generate(),
                "Channel" => new ChannelBuilder().Generate(),
                _ => throw new ArgumentException("Unsupported target type"),
            };

            throw new ArgumentException("Unsupported target type");
        }
    }
}