// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serialization.Benchmarks;
using Serialization.CrossCutting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddCrossCutting();
builder.Services.AddScoped<FlatBuffersBenchmarkSimple>();
var config = DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default);
BenchmarkRunner.Run<FlatBuffersBenchmark>(config);


var host = builder.Build();

var myService = host.Services.GetRequiredService<FlatBuffersBenchmarkSimple>();
myService.Initialize();

host.Run();