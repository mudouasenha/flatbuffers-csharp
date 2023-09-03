using Serialization.Domain.Entities;
using Serialization.Serializers.FlatBuffers;
using Serialization.Services;
using System.Diagnostics;

namespace Serialization.Benchmarks
{
    public class FlatBuffersBenchmarkSimple
    {
        private VideoService _videoService = new();
        private VideoFlatBuffersSerializer _videoconverter = new();
        //private SenderService _senderService = new();
        private RestClient client = new();
        private readonly Video vid;
        private const string Name = "FlatBuffers";
        private byte[] vidSerialized;

        public FlatBuffersBenchmarkSimple()
        {
            vid = _videoService.CreateVideo();
            vidSerialized = _videoconverter.Serialize(vid);
        }


        public async Task Initialize()
        {
            ExecuteMethod(FullProcess);
            ExecuteMethod(Serialization);
            ExecuteMethod(Serialization_1000, true);
            ExecuteMethod(Serialization_1_000_000, true);
            ExecuteMethod(DeSerialization);
            await Rest_POST_FullProcess();
            //ExecuteMethodParallel(Serialization_1_000_000, 4, 1000);

        }

        public void FullProcess() => _videoconverter.Deserialize(_videoconverter.Serialize(vid));

        public void Serialization() => _videoconverter.Serialize(vid);

        public void Serialization_1000()
        {
            for (int i = 0; i <= 10000; i++) _videoconverter.Serialize(vid);
        }

        public void Serialization_1_000_000()
        {
            for (int i = 0; i <= 1_000_000; i++) _videoconverter.Serialize(vid);
        }

        public void DeSerialization() => _videoconverter.Deserialize(vidSerialized);

        public async Task Rest_POST_FullProcess()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = client.PostAsync("receiver/video", vidSerialized).Result;
            stopwatch.Stop();

            TimeSpan cpuTime = currentProcess.TotalProcessorTime;

            Console.WriteLine($"\n {Name}.Rest_POST_FullProcess - Elapsed Time: {FormatElapsedTime(stopwatch.Elapsed)}");
            Console.WriteLine($"{Name}.Rest_POST_FullProcess - Memory used: {currentProcess.WorkingSet64 / 1024} KB");

            Console.WriteLine($"{Name}.Rest_POST_FullProcess - CPU Usage: {(cpuTime.TotalMilliseconds / stopwatch.ElapsedMilliseconds) * 100:F2}% \n");
        }

        public static void ExecuteMethod(Action action, bool multiple = false, bool parallel = false)
        {
            Process currentProcess = Process.GetCurrentProcess();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();

            TimeSpan cpuTime = currentProcess.TotalProcessorTime;

            Console.WriteLine($"\n {Name}.{action.Method.Name} - Elapsed Time: {FormatElapsedTime(stopwatch.Elapsed)}");
            Console.WriteLine($"{Name}.{action.Method.Name} - Memory used: {currentProcess.WorkingSet64 / 1024} KB");

            if (multiple == true) Console.WriteLine($"{Name}.{action.Method.Name} - CPU Usage: {cpuTime.TotalMilliseconds / stopwatch.ElapsedMilliseconds * 100:F2}% \n");
            if (parallel == true) Console.WriteLine($"Parallel.{Name}.{action.Method.Name} - CPU Usage: {(cpuTime.TotalMilliseconds / stopwatch.ElapsedMilliseconds):F2}% \n");
        }


        //public static void ExecuteMethodParallel(Action action, int numThreads = 4, int iterCount = 1000)
        //{
        //    int numberOfThreads = numThreads;
        //    int numberOfIterationsPerThread = iterCount;

        //    Thread[] threads = new Thread[numberOfThreads];

        //    for (int i = 0; i < numberOfThreads; i++)
        //    {
        //        threads[i] = new Thread(() =>
        //        {
        //            for (int j = 0; j < numberOfIterationsPerThread; j++)
        //            {
        //                ExecuteMethod(action, multiple: true, parallel: true);
        //            }
        //        });
        //    }

        //    foreach (Thread thread in threads)
        //    {
        //        thread.Start();
        //    }

        //    foreach (Thread thread in threads)
        //    {
        //        thread.Join();
        //    }
        //}

        static string FormatElapsedTime(TimeSpan elapsedTime)
        {
            if (elapsedTime.TotalSeconds >= 1)
            {
                return $"{elapsedTime.TotalSeconds} s"; // Seconds
            }
            else if (elapsedTime.TotalMilliseconds >= 1)
            {
                return $"{elapsedTime.TotalMilliseconds} ms"; // Milliseconds
            }
            else
            {
                return $"{elapsedTime.TotalMilliseconds * 1000} μs"; // Microseconds
            }
        }
    }
}
