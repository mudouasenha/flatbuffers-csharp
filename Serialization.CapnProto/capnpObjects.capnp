@0xa8be85dcb1c61b3a;

struct Channel {
  name @0 :Text;
  subscribers @1 :UInt32;
  channelId @2 :Text;
}

struct SocialInfo {
  likes @0 :UInt32;
  dislikes @1 :UInt32;
  comments @2 :List(Text);
  views @3 :UInt32;
}



struct VideoInfo {
  duration @0 :UInt64;
  description @1 :Text;
  size @2 :UInt64;
  qualities @3 :List(VideoQuality);

  enum VideoQuality {
	undefined @0;
	lowest @1;
	low @2;
	medium @3;
	sd @4;
	hd @5;
	twoK @6;
	fourK @7;
  }
}

struct Video {
  videoId @0 :Text;
  url @1 :Text;
  channel @2 :Channel;
  socialInfo @3 :SocialInfo;
  videoInfo @4 :VideoInfo;
}
