using Serialization.Domain.Interfaces;

namespace Serialization.Benchmarks.Abstractions
{
    public interface ISerializableBenchmark
    {
        ISerializer Serializer { get; set; }
        ISerializationTarget Target { get; set; }
        long Serialize();
    }

    public interface IMultipleSerializableBenchmark
    {
        ISerializer Serializer { get; set; }
        ISerializationTarget[] TargetList { get; set; }
    }
}
