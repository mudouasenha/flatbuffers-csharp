namespace Serialization.Receiver.Services
{
    public interface IRequestCounter
    {
        void RecordRequest();
        int GetRequestsPerSecond();
    }
}
