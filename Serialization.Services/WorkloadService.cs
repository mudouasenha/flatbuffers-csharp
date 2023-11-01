using Flurl;
using Serialization.Domain;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.ApacheAvro;

namespace Serialization.Services
{
    public class WorkloadService
    {
        private bool _executing;
        private readonly HttpClient _httpClient = new();
        private readonly RestClient _restClient = new();
        private const string SenderBaseUrl = "http://localhost:5010/";
        private const int delayMilliseconds = 1000 / 20;
        private ISerializationTarget[] objects;
        private ISerializer _serializer;

        public async Task SaveServerResultsAsync(string datetime, string serializerType, string serializationType, int numThreads)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5020/" + "receiver/serializer/monitoring/save-results"
                .SetQueryParam("datetime", datetime)
                    .SetQueryParam("serializerType", serializerType)
                    .SetQueryParam("serializationType", serializationType)
                .SetQueryParam("numThreads", numThreads));

            var response = await _httpClient.PostAsync(request.RequestUri, null);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Clear ok");
            }
            else
            {
                //var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Clear Not ok");
            }
        }

        public async Task ClearAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, SenderBaseUrl + "sender/workload/clear");

            var response = await _httpClient.PostAsync(request.RequestUri, null);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Clear ok");
            }
            else
            {
                //var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Clear Not ok");
                throw new Exception();
            }
        }

        public void Clear()
        {
            _executing = false;
            if (_serializer != null) _serializer.Cleanup();
            _serializer = null;
        }

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
                    Console.WriteLine("Dispatch ok");
                }
                else
                {
                    Console.WriteLine("Dispatch Not ok");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public async Task RunParallelRestAsync(ISerializer serializer, Type serializationType, int numThreads = 100)
        {
            if (_executing)
            {
                _executing = false;
                Thread.Sleep(200);
            }

            _executing = true;
            _serializer = serializer;
            SemaphoreSlim semaphore = new SemaphoreSlim(numThreads);
            List<Task> tasks = new();

            GenerateObjects(serializationType, numThreads);
            for (int i = 0; i < numThreads; i++)
            {
                tasks.Add(ProcessSerializationRestAsync(semaphore, i, _serializer, serializationType.Name));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All tasks completed.");
        }

        private async Task ProcessSerializationRestAsync(SemaphoreSlim semaphore, int threadId, ISerializer serializer, string serializationType)
        {
            while (_executing)
            {
                await semaphore.WaitAsync();
                //var rand = new Random(8).Next();

                try
                {
                    var obj = objects[threadId];
                    //var obj = objects[threadId][rand];
                    GenerateIntermediateIfNeeded(obj, _serializer);
                    obj.Serialize(_serializer);
                    if (serializer.GetSerializationResult(obj.GetType(), out var serializationObject))
                    {
                        while (_executing)
                        {
                            //Console.WriteLine($"Thread {threadId} started");

                            var result = await _restClient.PostAsync("receiver/serializer"
                                .SetQueryParam("serializerType", serializer.ToString())
                                .SetQueryParam("serializationType", serializationType),
                                serializationObject);

                            await Task.Delay(delayMilliseconds);
                        }
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

        private void GenerateIntermediateIfNeeded(ISerializationTarget obj, ISerializer serializer)
        {
            if (serializer is ProtobufSerializer)
                obj.CreateProtobufMessage();
            if (serializer is ApacheThriftSerializer)
                obj.CreateThriftMessage();
            if (serializer is ApacheAvroSerializer)
                obj.CreateAvroMessage();
        }

        private void GenerateObjects(Type type, int numThreads)
        {
            switch (type.Name)
            {
                case "Video":
                    objects = new VideoBuilder().Generate(numThreads).Cast<ISerializationTarget>().ToArray();
                    break;

                case "SocialInfo":
                    objects = new SocialInfoBuilder().Generate(numThreads).Cast<ISerializationTarget>().ToArray();
                    break;

                case "VideoInfo":
                    objects = new VideoInfoBuilder().Generate(numThreads).Cast<ISerializationTarget>().ToArray();
                    break;

                case "Channel":
                    objects = new ChannelBuilder().Generate(numThreads).Cast<ISerializationTarget>().ToArray();
                    break;

                default:
                    throw new ArgumentException("Unsupported target type");
            }
        }
    }
}