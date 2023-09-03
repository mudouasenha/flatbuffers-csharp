using Serialization.Domain.Interfaces;

namespace Serialization.Benchmarks
{
    public interface ISerializableBenchmark
    {
        ISerializer Serializer { get; set; }
        ISerializable Target { get; set; }
    }
}
