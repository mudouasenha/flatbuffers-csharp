using BenchmarkDotNet.Running;
using Microsoft.Extensions.Hosting;
using Serialization.Benchmarks.Benchmarks;
using Serialization.Benchmarks.Configs;
using Serialization.CrossCutting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddCrossCutting();



Thread.Sleep(TimeSpan.FromSeconds(10));

var config = new ConcurrencySerializationBenchmarkConfig();
BenchmarkRunner.Run<ConcurrencySerializationBenchmark>(config);


//var host = builder.Build();
//var myService = host.Services.GetRequiredService<FlatBuffersBenchmarkSimple>();
//await myService.Initialize();
//host.Run();