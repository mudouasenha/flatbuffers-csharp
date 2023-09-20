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
    public class RESTSerializationBenchmarkConfig : ManualConfig
    {
        public RESTSerializationBenchmarkConfig()
        {
            Add(DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default));

            var baseJob = new Job(Job.MediumRun
                .WithUnrollFactor(16)
                .WithWarmupCount(3)
                .WithIterationTime(TimeInterval.FromMilliseconds(100))
                .WithIterationCount(30)
                .WithInvocationCount(1)
                .WithId("JOB-REST")
                .WithRuntime(CoreRuntime.Core60)
                .WithToolchain(InProcessNoEmitToolchain.Instance));

            AddJob(baseJob);

            AddAnalyser(EnvironmentAnalyser.Default);
            AddDiagnoser(MemoryDiagnoser.Default);
            ConfigHelper.AddDefaultColumns(this);
        }
    }
}


