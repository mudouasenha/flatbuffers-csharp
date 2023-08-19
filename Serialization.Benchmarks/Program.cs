// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Serialization.Benchmarks;

Console.WriteLine("Hello, World!");

//HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddCrossCutting();
var config = DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default);
BenchmarkRunner.Run<FlatBuffersBenchmark>(config);
