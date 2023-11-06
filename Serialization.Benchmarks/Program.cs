using BenchmarkDotNet.Running;
using Microsoft.Extensions.Hosting;
using Serialization.Benchmarks.Benchmarks;
using Serialization.Benchmarks.Configs;
using Serialization.Services;

//ServicePointManager.DefaultConnectionLimit = 5000;
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Limits.MaxConcurrentConnections = 10000;
//});

builder.Services.AddServices();
//Thread.Sleep(TimeSpan.FromSeconds(10));

//var config = new MultipleSerializationBenchmarkConfig();
//var config2 = new ConcurrencySerializationBenchmarkConfig();
var config3 = new RESTSerializationBenchmarkConfig();

//BenchmarkRunner.Run<MultipleSerializationBenchmark>(config);
//BenchmarkRunner.Run<ConcurrencySerializationBenchmark>(config2);
BenchmarkRunner.Run<RESTSerializationBenchmark>(config3);

//BenchmarkRunner.Run<MultipleSerializationBenchmark>(config);

var host = builder.Build();

//var ser = new ApacheAvroSerializer();
//var obj = new VideoInfoBuilder().Generate();
//ser.BenchmarkSerialize(obj.GetType(), obj);
//ser.BenchmarkDeserialize(obj.GetType(), obj);

//var myService = new WorkloadService();

//List<RequestMessage> flatBuffers = myService.GenerateRequests("FlatBuffers", 1000);
//List<RequestMessage> avro = myService.GenerateRequests("Avro", 1000);
//List<RequestMessage> thrift = myService.GenerateRequests("Thrift", 1000);
//List<RequestMessage> messagePack = myService.GenerateRequests("MessagePack-CSharp", 1000);
//List<RequestMessage> capnProto = myService.GenerateRequests("CapnProto", 1000);
//List<RequestMessage> newtonsoft = myService.GenerateRequests("Newtonsoft.Json", 1000);
//List<RequestMessage> protobuf = myService.GenerateRequests("Protobuf", 1000);

//string json = JsonSerializer.Serialize(flatBuffers);
//File.WriteAllText("flatbuffers.json", json);

//string avroJson = JsonSerializer.Serialize(avro);
//File.WriteAllText("avro.json", avroJson);

//string thriftJson = JsonSerializer.Serialize(thrift);
//File.WriteAllText("thrift.json", thriftJson);

//string messagePackJson = JsonSerializer.Serialize(messagePack);
//File.WriteAllText("messagePack.json", messagePackJson);

//string capnProtoJson = JsonSerializer.Serialize(capnProto);
//File.WriteAllText("capnProto.json", capnProtoJson);

//string newtonsoftJson = JsonSerializer.Serialize(newtonsoft);
//File.WriteAllText("newtonsoftJson.json", newtonsoftJson);

//string protobufJson = JsonSerializer.Serialize(protobuf);
//File.WriteAllText("protobuf.json", protobufJson);