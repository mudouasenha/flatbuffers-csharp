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
    public class MultipleSerializationBenchmarkConfig : ManualConfig
    {
        public MultipleSerializationBenchmarkConfig()
        {
            Add(DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default));

            var baseJob = new Job(Job.MediumRun
                .WithUnrollFactor(1)
                .WithWarmupCount(3)
                .WithIterationTime(TimeInterval.FromMilliseconds(100))
                .WithIterationCount(30)
                .WithRuntime(CoreRuntime.Core60)
                .WithToolchain(InProcessNoEmitToolchain.Instance));

            AddJob(baseJob.WithInvocationCount(1000).WithId("JOB-MULTIPLE-LIGHT"));
            //AddJob(baseJob.WithInvocationCount(10000).WithId("JOB-MULTIPLE-MEDIUM"));
            //AddJob(baseJob.WithInvocationCount(1000000).WithId("JOB-MULTIPLE-HEAVY"));

            AddAnalyser(EnvironmentAnalyser.Default);
            AddDiagnoser(MemoryDiagnoser.Default);
            ConfigHelper.AddDefaultColumns(this);
        }
    }
}


