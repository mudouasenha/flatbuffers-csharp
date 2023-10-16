using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using Perfolizer.Horology;
using Serialization.Benchmarks.Configs.Columns;

namespace Serialization.Benchmarks.Configs
{
    public class SingleSerializationBenchmarkConfig : ManualConfig
    {
        public SingleSerializationBenchmarkConfig()
        {
            Add(DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default));

            var baseJob = new Job(Job.MediumRun
                .WithUnrollFactor(1)
                .WithWarmupCount(3)
                .WithIterationTime(TimeInterval.FromMilliseconds(100))
                .WithLaunchCount(1)
                .WithIterationCount(30)
                .WithInvocationCount(1)
                .WithId("JOB-SINGLE")
                .WithRuntime(CoreRuntime.Core60)
                .WithToolchain(InProcessNoEmitToolchain.Instance));

            AddJob(baseJob);

            AddAnalyser(EnvironmentAnalyser.Default);
            AddDiagnoser(MemoryDiagnoser.Default);
            AddColumn(new DataSizeColumn());
            ConfigHelper.AddDefaultColumns(this);
        }
    }
}


