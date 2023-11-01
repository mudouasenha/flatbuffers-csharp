namespace Serialization.Receiver.Services
{
    public interface IRequestCounterService
    {
        public void IncrementCounter();

        public void SaveToCsv();

        public void StartMonitoring();
    }
}