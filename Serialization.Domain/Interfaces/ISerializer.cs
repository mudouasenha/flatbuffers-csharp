namespace Serialization.Domain.Interfaces
{
    public interface ISerializer
    {
        long BenchmarkSerialize<T>(T original) where T : ISerializationTarget;

        long BenchmarkSerialize(Type type, ISerializationTarget original);

        long BenchmarkDeserialize<T>(T original) where T : ISerializationTarget;

        long BenchmarkDeserialize(Type type, ISerializationTarget original);

        bool GetDeserializationResult(Type type, out ISerializationTarget result);

        bool GetSerializationResult(Type type, out object result);

        void Cleanup();
    }
}