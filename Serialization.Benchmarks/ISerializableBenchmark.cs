using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;

namespace Serialization.Benchmarks
{
    public interface ISerializableBenchmark
    {
        ISerializer<Video> Serializer { get; set; }
        //ISerializationTarget Target { get; set; }
    }
}
