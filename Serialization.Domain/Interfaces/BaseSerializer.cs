using Serialization.Domain.Entities;

namespace Serialization.Domain.Interfaces
{
    public class SerializationResult<TSerialization>
    {
        public readonly TSerialization Result;
        public readonly long ByteSize;

        public SerializationResult(TSerialization result, long byteSize)
        {
            Result = result;
            ByteSize = byteSize;
        }
    }

    public abstract class BaseSerializer<TSerialization, TDeserialization> : ISerializer
    {
        private readonly object lockObject = new object();
        protected readonly Dictionary<Type, SerializationResult<TSerialization>> SerializationResults;
        protected readonly Dictionary<Type, TDeserialization> DeserializationResults;

        public Type SerializationOutputType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected BaseSerializer()
        {
            SerializationResults = new Dictionary<Type, SerializationResult<TSerialization>>();
            DeserializationResults = new Dictionary<Type, TDeserialization>();
        }

        /// <inheritdoc />
        public long BenchmarkSerialize<T>(T original) where T : ISerializationTarget
        {
            var result = Serialize(original, out long messageSize);
            SerializationResults[typeof(T)] = new SerializationResult<TSerialization>(result, messageSize);
            return messageSize;
        }

        /// <inheritdoc />
        public long BenchmarkSerialize(Type type, ISerializationTarget original)
        {
            var result = Serialize(type, original, out long messageSize);
            SerializationResults[type] = new SerializationResult<TSerialization>(result, messageSize);
            return messageSize;
        }

        /// <inheritdoc />
        public long BenchmarkDeserialize<T>(T original) where T : ISerializationTarget
        {
            var type = typeof(T);
            var target = SerializationResults[type];
            var copy = Deserialize<T>(target.Result);
            DeserializationResults[type] = copy;

            return target.ByteSize;
        }

        /// <inheritdoc />
        public long BenchmarkDeserialize(Type type, ISerializationTarget original)
        {
            var target = SerializationResults[type];
            var copy = Deserialize(type, target.Result);
            DeserializationResults[type] = copy;

            return target.ByteSize;
        }

        /// <inheritdoc />
        public Type BenchmarkDeserialize(Type type, object serialized)
        {
            var copy = Deserialize(type, (TSerialization)serialized);
            DeserializationResults[type] = copy;

            return copy.GetType();
        }

        /// <inheritdoc />
        public long BenchmarkSerializeThreadSafe<T>(T original) where T : ISerializationTarget
        {
            var result = Serialize(original, out long messageSize);
            lock (lockObject)
            {
                SerializationResults[typeof(T)] = new SerializationResult<TSerialization>(result, messageSize);
            }
            return messageSize;
        }

        /// <inheritdoc />
        public long BenchmarkSerializeThreadSafe(Type type, ISerializationTarget original)
        {
            var result = Serialize(type, original, out long messageSize);
            lock (lockObject)
            {
                SerializationResults[type] = new SerializationResult<TSerialization>(result, messageSize);
            }
            return messageSize;
        }

        /// <inheritdoc />
        public long BenchmarkDeserializeThreadSafe<T>(T original) where T : ISerializationTarget
        {
            var type = typeof(T);
            SerializationResult<TSerialization> target = null;
            lock (lockObject)
            {
                target = SerializationResults[type];
            }
            var copy = Deserialize<T>(target.Result);
            lock (lockObject)
            {
                DeserializationResults[type] = copy;
            }

            return target.ByteSize;
        }

        /// <inheritdoc />
        public long BenchmarkDeserializeThreadSafe(Type type, ISerializationTarget original)
        {
            SerializationResult<TSerialization> target = null;
            lock (lockObject)
            {
                target = SerializationResults[type];
            }

            var copy = Deserialize(type, target.Result);

            lock (lockObject)
            {
                DeserializationResults[type] = copy;
            }

            return target.ByteSize;
        }

        /// <inheritdoc />
        public bool Validate(Type type, ISerializationTarget original)
        {
            if (GetDeserializationResult(type, out ISerializationTarget result))
            {
                var isValid = EqualityComparer<ISerializationTarget>.Default.Equals(original, result);
                return isValid;
            }

            Console.WriteLine($"Serialized result with type {type} not found!");
            return false;
        }

        public abstract bool GetDeserializationResult(Type type, out ISerializationTarget result);

        public bool GetSerializationResult(Type type, out object result)
        {
            if (type == typeof(Video))
            {
                result = SerializationResults[typeof(Video)].Result;
                return true;
            }
            if (type == typeof(VideoInfo))
            {
                result = SerializationResults[typeof(VideoInfo)].Result;
                return true;
            }
            if (type == typeof(SocialInfo))
            {
                result = SerializationResults[typeof(SocialInfo)].Result;
                return true;
            }
            if (type == typeof(Channel))
            {
                result = SerializationResults[typeof(Channel)].Result;
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        #region Serialization

        protected abstract TSerialization Serialize<T>(T original, out long messageSize) where T : ISerializationTarget;

        protected abstract TSerialization Serialize(Type type, ISerializationTarget original, out long messageSize);

        #endregion Serialization

        #region Deserialization

        protected abstract TDeserialization Deserialize<T>(TSerialization serializedObject) where T : ISerializationTarget;

        protected abstract TDeserialization Deserialize(Type type, TSerialization serializedObject);

        #endregion Deserialization

        /// <inheritdoc />
        public virtual void Cleanup()
        {
            SerializationResults.Clear();
            DeserializationResults.Clear();
        }

        public abstract Type GetSerializationOutPutType();

        //public bool GetSerializationResult<T>(Type type, out T result) where T : class => GetSerializationResult(type, out result);
    }

    public abstract class BaseDirectSerializer<ISerializationObject> : BaseSerializer<ISerializationObject, ISerializationTarget>
    {
        public override bool GetDeserializationResult(Type type, out ISerializationTarget result) => DeserializationResults.TryGetValue(type, out result);
    }
}