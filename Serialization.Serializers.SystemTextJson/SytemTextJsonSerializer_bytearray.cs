//using Newtonsoft.Json;
//using Serialization.Domain.Entities;
//using Serialization.Domain.Interfaces;
//using System.Text.Json;

//namespace Serialization.Serializers.SystemTextJson
//{
//    public class SytemTextJsonSerializer : BaseDirectSerializer<string>
//    {
//        public JsonSerializerOptions Options { get; set; }

//        public SytemTextJsonSerializer()
//        {
//            Options = new JsonSerializerOptions { IncludeFields = true };
//        }

//        protected override string Serialize<T>(T original, out long messageSize)
//        {
//            var jsonString = JsonConvert.SerializeObject(original);
//            messageSize = System.Text.Encoding.UTF8.GetByteCount(jsonString);
//            return jsonString;
//        }

//        protected override ISerializationTarget Deserialize<T>(string buf)
//        {
//            T copy;
//            copy = JsonConvert.DeserializeObject<T>(buf);

//            return copy;
//        }

//        protected override string Serialize(Type type, ISerializationTarget original, out long messageSize)
//        {
//            try
//            {
//                var jsonString = JsonConvert.SerializeObject(original);
//                messageSize = System.Text.Encoding.UTF8.GetByteCount(jsonString);
//                return jsonString;
//            }
//            catch (Exception ex)
//            {
//                // Log the exception for debugging
//                Console.WriteLine($"Serialization error: {ex.Message}");
//                messageSize = 0; // Set message size to 0 to indicate an error
//                return null; // Return null or an error message as appropriate
//            }

//        }

//        protected override ISerializationTarget Deserialize(Type type, string serializedObject)
//        {
//            ISerializationTarget copy;
//            copy = JsonConvert.DeserializeObject<ISerializationTarget>(serializedObject);

//            return copy;
//        }

//        public override bool GetSerializationResult(Type type, out object result)
//        {
//            if (type == typeof(Video))
//            {
//                result = SerializationResults[typeof(Video)].Result;
//                return true;
//            }
//            if (type == typeof(VideoInfo))
//            {
//                result = SerializationResults[typeof(VideoInfo)].Result;
//                return true;
//            }
//            if (type == typeof(SocialInfo))
//            {
//                result = SerializationResults[typeof(SocialInfo)].Result;
//                return true;
//            }
//            if (type == typeof(Channel))
//            {
//                result = SerializationResults[typeof(Channel)].Result;
//                return true;
//            }
//            throw new NotImplementedException($"Conversion for type {type} not implemented!");
//        }

//        public override Type GetSerializationOutPutType() => typeof(string);
//    }
//}