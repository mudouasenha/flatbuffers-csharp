using Serialization.Domain.Interfaces;

namespace Serialization.Benchmarks
{
    public interface ISerializableBenchmark
    {
        ISerializer Serializer { get; set; }
        ISerializationTarget Target { get; set; }
        long Serialize();
    }
}
