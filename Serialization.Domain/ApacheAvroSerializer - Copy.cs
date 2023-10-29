//using Avro.Specific;
//using Serialization.Domain.Entities;
//using Serialization.Domain.Entities.Enums;
//using Serialization.Domain.Interfaces;
//using SolTechnology.Avro;
//using System.Buffers;

//namespace Serialization.Serializers.ApacheAvro
//{
//    public class ApacheAvroSerializer : BaseSerializer<byte[], ISpecificRecord>
//    {
//        private readonly string channelSchema = avroObjects.Channel._SCHEMA.ToString();
//        private readonly string videoInfoSchema = avroObjects.VideoInfo._SCHEMA.ToString();
//        private readonly string socialInfoSchema = avroObjects.SocialInfo._SCHEMA.ToString();
//        private readonly string videoSchema = avroObjects.Video._SCHEMA.ToString();

//        protected override ISpecificRecord Deserialize<T>(byte[] serializedObject)
//        {
//            ISpecificRecord thriftObject = (ISpecificRecord)Activator.CreateInstance(typeof(T));
//            var deserializedObject = AvroConvert.Deserialize<ISpecificRecord>(serializedObject);

//            return deserializedObject;
//        }

//        protected override ISpecificRecord Deserialize(Type type, byte[] serializedObject)
//        {
//            if (type == typeof(Channel))
//            {
//                avroObjects.Channel deserializedObject = AvroConvert.DeserializeHeadless<avroObjects.Channel>(serializedObject, channelSchema);
//                return deserializedObject;
//            }

//            if (type == typeof(VideoInfo))
//            {
//                avroObjects.VideoInfo deserializedObject = AvroConvert.DeserializeHeadless<avroObjects.VideoInfo>(serializedObject, videoInfoSchema);
//                return deserializedObject;
//            }

//            if (type == typeof(SocialInfo))
//            {
//                string schemaInJsonFormat = AvroConvert.GetSchema(serializedObject);
//                SocialInfo temp = AvroConvert.Deserialize<SocialInfo>(serializedObject);
//                temp.CreateAvroMessage();
//                var deserializedObject = temp.GetAvroMessage();
//                return deserializedObject;
//            };

//            if (type == typeof(Video))
//            {
//                string schemaInJsonFormat = AvroConvert.GetSchema(serializedObject);
//                var temp = AvroConvert.Deserialize<avroObjects.Video>(serializedObject);
//                //temp.CreateAvroMessage();
//                //var deserializedObject = temp.GetAvroMessage();
//                return temp;
//                //avroObjects.Video deserializedObject = AvroConvert.DeserializeHeadless<avroObjects.Video>(serializedObject, videoSchema);
//                //return deserializedObject;
//            }

//            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
//        }

//        protected override byte[] Serialize<T>(T original, out long messageSize)
//        {
//            original.CreateAvroMessage();
//            var avroMsg = original.GetAvroMessage();
//            byte[] avroObject = AvroConvert.SerializeHeadless(avroMsg, avroMsg.GetType());
//            messageSize = avroObject.Length;
//            return avroObject;
//        }

//        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
//        {
//            Console.WriteLine("Ok");
//            if (type == typeof(Channel))
//            {
//                //var intermediateResult = (Channel)original;

//                original.CreateAvroMessage();
//                var avroMsg = original.GetAvroMessage();
//                byte[] avroObject = AvroConvert.SerializeHeadless(avroMsg, channelSchema);
//                messageSize = avroObject.Length;
//                return avroObject;
//            }

//            if (type == typeof(VideoInfo))
//            {
//                //var intermediateResult = (VideoInfo)original;
//                //var qualities = intermediateResult.Qualities.Select(x => (avroObjects.VideoQualities)x).ToArray();

//                //var avroMsg = new avroObjects.VideoInfo()
//                //{
//                //    Duration = intermediateResult.Duration,
//                //    Description = intermediateResult.Description,
//                //    Size = intermediateResult.Size,
//                //    Qualities = qualities
//                //};

//                original.CreateAvroMessage();
//                var avroMsg = original.GetAvroMessage();
//                //byte[] avroObject = AvroConvert.SerializeHeadless(avroMsg, videoInfoSchema);
//                byte[] avroObject = AvroConvert.SerializeHeadless(avroMsg, typeof(avroObjects.SocialInfo));
//                messageSize = avroObject.Length;
//                return avroObject;
//            }

//            if (type == typeof(SocialInfo))
//            {
//                //var intermediateResult = (SocialInfo)original;
//                //var avroMsg = new avroObjects.SocialInfo()
//                //{
//                //    Dislikes = intermediateResult.Dislikes,
//                //    Likes = intermediateResult.Likes,
//                //    Views = intermediateResult.Views,
//                //    Comments = intermediateResult.Comments
//                //};

//                original.CreateAvroMessage();
//                var avroMsg = original.GetAvroMessage();
//                var schema = AvroConvert.GenerateSchema(typeof(avroObjects.SocialInfo));
//                byte[] avroObject = AvroConvert.Serialize(original);
//                messageSize = avroObject.Length;
//                return avroObject;
//            }

//            if (type == typeof(Video))
//            {
//                //var intermediateResult = (Video)original;
//                //var qualities = intermediateResult.VideoInfo.Qualities.Select(x => (avroObjects.VideoQualities)x).ToArray();
//                //var avroMsg = new avroObjects.Video()
//                //{
//                //    VideoId = intermediateResult.VideoId,
//                //    Url = intermediateResult.Url,
//                //    Channel = new avroObjects.Channel()
//                //    {
//                //        ChannelId = intermediateResult.Channel.ChannelId,
//                //        Name = intermediateResult.Channel.Name,
//                //        Subscribers = intermediateResult.Channel.Subscribers
//                //    },
//                //    SocialInfo = new avroObjects.SocialInfo()
//                //    {
//                //        Dislikes = intermediateResult.SocialInfo.Dislikes,
//                //        Likes = intermediateResult.SocialInfo.Likes,
//                //        Views = intermediateResult.SocialInfo.Views,
//                //        Comments = intermediateResult.SocialInfo.Comments.ToList()
//                //    },
//                //    VideoInfo = new avroObjects.VideoInfo()
//                //    {
//                //        Duration = intermediateResult.VideoInfo.Duration,
//                //        Description = intermediateResult.VideoInfo.Description,
//                //        Size = intermediateResult.VideoInfo.Size,
//                //        Qualities = qualities
//                //    },
//                //};

//                original.CreateAvroMessage();
//                var avroMsg = original.GetAvroMessage();

//                byte[] avroObject = AvroConvert.Serialize(avroMsg);
//                //byte[] avroObject = AvroConvert.SerializeHeadless(avroMsg, videoSchema);
//                messageSize = avroObject.Length;
//                return avroObject;
//            }

//            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
//        }

//        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
//        {
//            var intermediateResult = DeserializationResults[type];

//            if (type == typeof(Channel))
//            {
//                var channel = (avroObjects.Channel)intermediateResult;
//                result = new Channel()
//                {
//                    ChannelId = channel.ChannelId,
//                    Name = channel.Name,
//                    Subscribers = channel.Subscribers
//                };
//                return true;
//            }

//            if (type == typeof(VideoInfo))
//            {
//                var videoInfo = (avroObjects.VideoInfo)intermediateResult;
//                result = new VideoInfo()
//                {
//                    Duration = videoInfo.Duration,
//                    Size = videoInfo.Size,
//                    Description = videoInfo.Description,
//                    Qualities = (VideoQualities[])videoInfo.Qualities.ToArray().Select(x => (VideoQualities)x)
//                };
//                return true;
//            }

//            if (type == typeof(SocialInfo))
//            {
//                var socialInfo = (avroObjects.SocialInfo)intermediateResult;
//                result = new SocialInfo()
//                {
//                    Likes = socialInfo.Likes,
//                    Dislikes = socialInfo.Dislikes,
//                    Views = socialInfo.Views,
//                    Comments = socialInfo.Comments.ToArray()
//                };
//                return true;
//            }

//            if (type == typeof(Video))
//            {
//                var video = (avroObjects.Video)intermediateResult;
//                result = new Video()
//                {
//                    VideoId = video.VideoId,
//                    Url = video.Url,
//                    Channel = new Channel()
//                    {
//                        ChannelId = video.Channel.ChannelId,
//                        Name = video.Channel.Name,
//                        Subscribers = (int)video.Channel.Subscribers
//                    },
//                    VideoInfo = new VideoInfo()
//                    {
//                        Description = video.VideoInfo.Description,
//                        Duration = video.VideoInfo.Duration,
//                        Qualities = (VideoQualities[])video.VideoInfo.Qualities.ToArray().Select(x => (VideoQualities)x)
//                    },
//                    SocialInfo = new SocialInfo()
//                    {
//                        Likes = video.SocialInfo.Likes,
//                        Dislikes = video.SocialInfo.Dislikes,
//                        Views = video.SocialInfo.Views,
//                        Comments = video.SocialInfo.Comments.ToArray()
//                    }
//                };
//                return true;
//            }

//            throw new NotImplementedException($"Conversion for type {type} not implemented!");
//        }

//        public override string ToString() => "Avro";

//        public override Type GetSerializationOutPutType() => typeof(byte[]);
//    }
//}