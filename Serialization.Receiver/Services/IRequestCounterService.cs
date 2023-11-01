namespace Serialization.Receiver.Services
{
    public interface IRequestCounterService
    {
        public void IncrementCounter();

        public void SaveToCsv(string datetime, string serializerType, string serializationType, int numThreads);

        public void StartMonitoring();
    }
}