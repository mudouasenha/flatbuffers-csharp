using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using Perfolizer.Horology;

namespace Serialization.Benchmarks.Configs
{
    public class ConcurrencySerializationBenchmarkConfig : ManualConfig
    {
        public ConcurrencySerializationBenchmarkConfig()
        {
            Add(DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default));

            var baseJob = new Job(Job.MediumRun
                .WithUnrollFactor(1)
                .WithWarmupCount(3)
                .WithIterationTime(TimeInterval.FromMilliseconds(100))
                .WithLaunchCount(1)
                .WithIterationCount(1)
                //.WithIterationCount(30)
                .WithInvocationCount(1)
                .WithId("JOB-CONCURRENCY")
                .WithRuntime(CoreRuntime.Core60)
                .WithToolchain(InProcessNoEmitToolchain.Instance));

            AddJob(baseJob);

            AddAnalyser(EnvironmentAnalyser.Default);
            AddDiagnoser(MemoryDiagnoser.Default);
            ConfigHelper.AddDefaultColumns(this);
        }
    }
}


