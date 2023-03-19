// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatBuffers.Domain.VideoModel
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct VideoInfoFlatModel : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_23_3_3(); }
  public static VideoInfoFlatModel GetRootAsVideoInfoFlatModel(ByteBuffer _bb) { return GetRootAsVideoInfoFlatModel(_bb, new VideoInfoFlatModel()); }
  public static VideoInfoFlatModel GetRootAsVideoInfoFlatModel(ByteBuffer _bb, VideoInfoFlatModel obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public VideoInfoFlatModel __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Duration { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string Description { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetDescriptionBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetDescriptionBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetDescriptionArray() { return __p.__vector_as_array<byte>(6); }
  public long Size { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public FlatBuffers.Domain.VideoModel.VideoQualityFlatModel Qualities(int j) { int o = __p.__offset(10); return o != 0 ? (FlatBuffers.Domain.VideoModel.VideoQualityFlatModel)__p.bb.GetShort(__p.__vector(o) + j * 2) : (FlatBuffers.Domain.VideoModel.VideoQualityFlatModel)0; }
  public int QualitiesLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<FlatBuffers.Domain.VideoModel.VideoQualityFlatModel> GetQualitiesBytes() { return __p.__vector_as_span<FlatBuffers.Domain.VideoModel.VideoQualityFlatModel>(10, 2); }
#else
  public ArraySegment<byte>? GetQualitiesBytes() { return __p.__vector_as_arraysegment(10); }
#endif
  public FlatBuffers.Domain.VideoModel.VideoQualityFlatModel[] GetQualitiesArray() { int o = __p.__offset(10); if (o == 0) return null; int p = __p.__vector(o); int l = __p.__vector_len(o); FlatBuffers.Domain.VideoModel.VideoQualityFlatModel[] a = new FlatBuffers.Domain.VideoModel.VideoQualityFlatModel[l]; for (int i = 0; i < l; i++) { a[i] = (FlatBuffers.Domain.VideoModel.VideoQualityFlatModel)__p.bb.GetShort(p + i * 2); } return a; }

  public static Offset<FlatBuffers.Domain.VideoModel.VideoInfoFlatModel> CreateVideoInfoFlatModel(FlatBufferBuilder builder,
      int duration = 0,
      StringOffset descriptionOffset = default(StringOffset),
      long size = 0,
      VectorOffset qualitiesOffset = default(VectorOffset)) {
    builder.StartTable(4);
    VideoInfoFlatModel.AddSize(builder, size);
    VideoInfoFlatModel.AddQualities(builder, qualitiesOffset);
    VideoInfoFlatModel.AddDescription(builder, descriptionOffset);
    VideoInfoFlatModel.AddDuration(builder, duration);
    return VideoInfoFlatModel.EndVideoInfoFlatModel(builder);
  }

  public static void StartVideoInfoFlatModel(FlatBufferBuilder builder) { builder.StartTable(4); }
  public static void AddDuration(FlatBufferBuilder builder, int duration) { builder.AddInt(0, duration, 0); }
  public static void AddDescription(FlatBufferBuilder builder, StringOffset descriptionOffset) { builder.AddOffset(1, descriptionOffset.Value, 0); }
  public static void AddSize(FlatBufferBuilder builder, long size) { builder.AddLong(2, size, 0); }
  public static void AddQualities(FlatBufferBuilder builder, VectorOffset qualitiesOffset) { builder.AddOffset(3, qualitiesOffset.Value, 0); }
  public static VectorOffset CreateQualitiesVector(FlatBufferBuilder builder, FlatBuffers.Domain.VideoModel.VideoQualityFlatModel[] data) { builder.StartVector(2, data.Length, 2); for (int i = data.Length - 1; i >= 0; i--) builder.AddShort((short)data[i]); return builder.EndVector(); }
  public static VectorOffset CreateQualitiesVectorBlock(FlatBufferBuilder builder, FlatBuffers.Domain.VideoModel.VideoQualityFlatModel[] data) { builder.StartVector(2, data.Length, 2); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateQualitiesVectorBlock(FlatBufferBuilder builder, ArraySegment<FlatBuffers.Domain.VideoModel.VideoQualityFlatModel> data) { builder.StartVector(2, data.Count, 2); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateQualitiesVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<FlatBuffers.Domain.VideoModel.VideoQualityFlatModel>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartQualitiesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(2, numElems, 2); }
  public static Offset<FlatBuffers.Domain.VideoModel.VideoInfoFlatModel> EndVideoInfoFlatModel(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatBuffers.Domain.VideoModel.VideoInfoFlatModel>(o);
  }
}


}
