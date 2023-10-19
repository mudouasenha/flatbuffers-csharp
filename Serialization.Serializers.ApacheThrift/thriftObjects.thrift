namespace netstd thriftObjects

struct Channel {
    1: string Name;
    2: i32 Subscribers;
    3: string ChannelId;
}

struct SocialInfo {
    1: i32 Likes;
    2: i32 Dislikes;
    3: list<string> Comments;
    4: i32 Views;
}

enum VideoQualities {
    Undefined = 0;
    Lowest = 144;
    Low = 360;
    Medium = 480;
    SD = 720;
    HD = 1080;
    TwoK = 1440;
    FourK = 2160;
}

struct VideoInfo {
    1: i64 Duration;
    2: string Description;
    3: i64 Size;
    4: list<VideoQualities> Qualities;
}

struct Video {
    1: string VideoId;
    2: string Url;
    3: Channel Channel;
    4: SocialInfo SocialInfo;
    5: VideoInfo VideoInfo;
}
