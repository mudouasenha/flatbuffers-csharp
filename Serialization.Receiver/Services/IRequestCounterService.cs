namespace Serialization.Receiver.Services
{
    public interface IRequestCounterService
    {
        public void IncrementCounter(short serializer, bool? serialize = null);

        public void SaveToCsv(string serializerType, string serializationType, int numThreads, string method);

        public void StartMonitoring();
    }
}