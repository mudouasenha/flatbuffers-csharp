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
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default));

            var baseJob = new Job(Job.MediumRun
                .WithUnrollFactor(1)
                .WithWarmupCount(3)
                .WithIterationTime(TimeInterval.FromMilliseconds(100))
                .WithRuntime(CoreRuntime.Core60)
                .WithToolchain(InProcessNoEmitToolchain.Instance));

            AddJob(baseJob.WithIterationCount(30).WithInvocationCount(1).WithId("JOB-SINGLE"));
            AddJob(baseJob.WithIterationCount(30).WithInvocationCount(1000).WithId("JOB-MULTIPLE-LIGHT"));
            AddJob(baseJob.WithIterationCount(30).WithInvocationCount(10000).WithId("JOB-MULTIPLE-MEDIUM"));
            AddJob(baseJob.WithIterationCount(30).WithInvocationCount(1000000).WithId("JOB-MULTIPLE-HEAVY"));

            AddAnalyser(EnvironmentAnalyser.Default);

            AddDiagnoser(MemoryDiagnoser.Default);

            AddColumn(new DataSizeColumn());
            ConfigHelper.AddDefaultColumns(this);
        }
    }
}


