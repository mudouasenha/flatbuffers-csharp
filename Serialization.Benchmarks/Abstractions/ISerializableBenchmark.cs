using Serialization.Domain.Interfaces;

namespace Serialization.Benchmarks.Abstractions
{
    public interface ISerializableBenchmark
    {
        ISerializer Serializer { get; set; }
        ISerializationTarget Target { get; set; }
        long Serialize();
    }
}
