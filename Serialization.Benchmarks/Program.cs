using Microsoft.Extensions.Hosting;
using Serialization.Domain.Builders;
using Serialization.Serializers.ApacheAvro;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

Thread.Sleep(TimeSpan.FromSeconds(10));

//var config = new SingleSerializationBenchmarkConfig();
//BenchmarkRunner.Run<SingleSerializationBenchmark>(config);

var host = builder.Build();

var ser = new ApacheAvroSerializer();
var obj = new ChannelBuilder().Generate();
ser.BenchmarkSerialize(obj.GetType(), obj);
ser.BenchmarkDeserialize(obj.GetType(), obj);

//var myService = new WorkloadService();

//await myService.RunParallelRestAsync(
//    new FlatBuffersSerializer(),
//    new ChannelBuilder().Generate().GetType(),
//    24);

//await myService.Initialize();
//host.Run();