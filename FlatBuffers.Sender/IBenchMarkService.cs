namespace FlatBuffers.Sender
{
    public interface IBenchMarkService<T>
    {
        Task<T> RunBenchMark();
    }
}