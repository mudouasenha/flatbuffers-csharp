namespace FlatBuffers.Sender
{
    public interface IBenchMarkService<T>
    {
        T RunBenchMarkLocal();
        Task<T> RunBenchMark();
    }
}