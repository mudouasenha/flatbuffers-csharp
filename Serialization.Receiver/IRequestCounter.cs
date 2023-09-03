namespace Serialization.Receiver
{
    public interface IRequestCounter
    {
        void RecordRequest();
        int GetRequestsPerSecond();
    }
}
