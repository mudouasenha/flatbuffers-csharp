using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CapnpGen
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb455bf7d9bfab5ebUL)]
    public class Channel : ICapnpSerializable
    {
        public const UInt64 typeId = 0xb455bf7d9bfab5ebUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Name = reader.Name;
            Subscribers = reader.Subscribers;
            ChannelId = reader.ChannelId;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Name = Name;
            writer.Subscribers = Subscribers;
            writer.ChannelId = ChannelId;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public string Name
        {
            get;
            set;
        }

        public uint Subscribers
        {
            get;
            set;
        }

        public string ChannelId
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public string Name => ctx.ReadText(0, null);
            public uint Subscribers => ctx.ReadDataUInt(0UL, 0U);
            public string ChannelId => ctx.ReadText(1, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 2);
            }

            public string Name
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public uint Subscribers
            {
                get => this.ReadDataUInt(0UL, 0U);
                set => this.WriteData(0UL, value, 0U);
            }

            public string ChannelId
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa6be7e8166ea3b9dUL)]
    public class SocialInfo : ICapnpSerializable
    {
        public const UInt64 typeId = 0xa6be7e8166ea3b9dUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Likes = reader.Likes;
            Dislikes = reader.Dislikes;
            Comments = reader.Comments;
            Views = reader.Views;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Likes = Likes;
            writer.Dislikes = Dislikes;
            writer.Comments.Init(Comments);
            writer.Views = Views;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public uint Likes
        {
            get;
            set;
        }

        public uint Dislikes
        {
            get;
            set;
        }

        public IReadOnlyList<string> Comments
        {
            get;
            set;
        }

        public uint Views
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public uint Likes => ctx.ReadDataUInt(0UL, 0U);
            public uint Dislikes => ctx.ReadDataUInt(32UL, 0U);
            public IReadOnlyList<string> Comments => ctx.ReadList(0).CastText2();
            public uint Views => ctx.ReadDataUInt(64UL, 0U);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 1);
            }

            public uint Likes
            {
                get => this.ReadDataUInt(0UL, 0U);
                set => this.WriteData(0UL, value, 0U);
            }

            public uint Dislikes
            {
                get => this.ReadDataUInt(32UL, 0U);
                set => this.WriteData(32UL, value, 0U);
            }

            public ListOfTextSerializer Comments
            {
                get => BuildPointer<ListOfTextSerializer>(0);
                set => Link(0, value);
            }

            public uint Views
            {
                get => this.ReadDataUInt(64UL, 0U);
                set => this.WriteData(64UL, value, 0U);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfa84441f146d72ffUL)]
    public class VideoInfo : ICapnpSerializable
    {
        public const UInt64 typeId = 0xfa84441f146d72ffUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Duration = reader.Duration;
            Description = reader.Description;
            Size = reader.Size;
            Qualities = reader.Qualities;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Duration = Duration;
            writer.Description = Description;
            writer.Size = Size;
            writer.Qualities.Init(Qualities);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public ulong Duration
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public ulong Size
        {
            get;
            set;
        }

        public IReadOnlyList<CapnpGen.VideoInfo.VideoQuality> Qualities
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public ulong Duration => ctx.ReadDataULong(0UL, 0UL);
            public string Description => ctx.ReadText(0, null);
            public ulong Size => ctx.ReadDataULong(64UL, 0UL);
            public IReadOnlyList<CapnpGen.VideoInfo.VideoQuality> Qualities => ctx.ReadList(1).CastEnums(_0 => (CapnpGen.VideoInfo.VideoQuality)_0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 2);
            }

            public ulong Duration
            {
                get => this.ReadDataULong(0UL, 0UL);
                set => this.WriteData(0UL, value, 0UL);
            }

            public string Description
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public ulong Size
            {
                get => this.ReadDataULong(64UL, 0UL);
                set => this.WriteData(64UL, value, 0UL);
            }

            public ListOfPrimitivesSerializer<CapnpGen.VideoInfo.VideoQuality> Qualities
            {
                get => BuildPointer<ListOfPrimitivesSerializer<CapnpGen.VideoInfo.VideoQuality>>(1);
                set => Link(1, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xecd6dfc7223566e7UL)]
        public enum VideoQuality : ushort
        {
            lowest,
            low,
            medium,
            sd,
            hd,
            twoK,
            fourK
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa40aad1e2d91514fUL)]
    public class Video : ICapnpSerializable
    {
        public const UInt64 typeId = 0xa40aad1e2d91514fUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            VideoId = reader.VideoId;
            Url = reader.Url;
            Channel = CapnpSerializable.Create<CapnpGen.Channel>(reader.Channel);
            SocialInfo = CapnpSerializable.Create<CapnpGen.SocialInfo>(reader.SocialInfo);
            VideoInfo = CapnpSerializable.Create<CapnpGen.VideoInfo>(reader.VideoInfo);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.VideoId = VideoId;
            writer.Url = Url;
            Channel?.serialize(writer.Channel);
            SocialInfo?.serialize(writer.SocialInfo);
            VideoInfo?.serialize(writer.VideoInfo);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public string VideoId
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public CapnpGen.Channel Channel
        {
            get;
            set;
        }

        public CapnpGen.SocialInfo SocialInfo
        {
            get;
            set;
        }

        public CapnpGen.VideoInfo VideoInfo
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public string VideoId => ctx.ReadText(0, null);
            public string Url => ctx.ReadText(1, null);
            public CapnpGen.Channel.READER Channel => ctx.ReadStruct(2, CapnpGen.Channel.READER.create);
            public CapnpGen.SocialInfo.READER SocialInfo => ctx.ReadStruct(3, CapnpGen.SocialInfo.READER.create);
            public CapnpGen.VideoInfo.READER VideoInfo => ctx.ReadStruct(4, CapnpGen.VideoInfo.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 5);
            }

            public string VideoId
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public string Url
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }

            public CapnpGen.Channel.WRITER Channel
            {
                get => BuildPointer<CapnpGen.Channel.WRITER>(2);
                set => Link(2, value);
            }

            public CapnpGen.SocialInfo.WRITER SocialInfo
            {
                get => BuildPointer<CapnpGen.SocialInfo.WRITER>(3);
                set => Link(3, value);
            }

            public CapnpGen.VideoInfo.WRITER VideoInfo
            {
                get => BuildPointer<CapnpGen.VideoInfo.WRITER>(4);
                set => Link(4, value);
            }
        }
    }
}