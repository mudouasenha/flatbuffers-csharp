using Serialization.Domain.Entities;
using Serialization.Serializers.FlatBuffers;
using Serialization.Services;
using System.Diagnostics;

namespace Serialization.Benchmarks
{
    public class FlatBuffersBenchmarkSimple
    {
        private VideoService _videoService = new();
        private VideoFlatBuffersConverter _videoconverter = new();
        private readonly Video vid;
        private const string Name = "FlatBuffers";
        private byte[] vidSerialized;

        public FlatBuffersBenchmarkSimple()
        {
            vid = _videoService.CreateVideo();
            vidSerialized = _videoconverter.Serialize(vid);
        }


        public void Initialize()
        {
            ExecuteMethod(FullProcess);
            ExecuteMethod(Serialization);
            ExecuteMethod(Serialization_1000, true);
            ExecuteMethod(Serialization_1_000_000, true);
            ExecuteMethod(DeSerialization);

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

        public static void ExecuteMethod(Action action, bool multiple = false)
        {
            Process currentProcess = Process.GetCurrentProcess();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();

            TimeSpan cpuTime = currentProcess.TotalProcessorTime;

            Console.WriteLine($"{Name}.{action.Method.Name} - Elapsed Time: {FormatElapsedTime(stopwatch.Elapsed)}");
            Console.WriteLine($"{Name}.{action.Method.Name} - Memory used: {currentProcess.WorkingSet64 / 1024} KB");

            if (multiple == true) Console.WriteLine($"{Name}.{action.Method.Name} - CPU Usage: {cpuTime.TotalMilliseconds / stopwatch.ElapsedMilliseconds * 100:F2}% \n");
        }

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
