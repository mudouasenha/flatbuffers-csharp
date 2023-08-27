using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serialization.Benchmarks;
using Serialization.Benchmarks.Configs;
using Serialization.CrossCutting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddCrossCutting().AddScoped<FlatBuffersBenchmarkSimple>();


var config = new BenchmarkConfig();
Thread.Sleep(TimeSpan.FromSeconds(10));


BenchmarkRunner.Run<FlatBuffersBenchmark>(config);


//var host = builder.Build();
////var myService = host.Services.GetRequiredService<FlatBuffersBenchmarkSimple>();
////myService.Initialize();
//host.Run();