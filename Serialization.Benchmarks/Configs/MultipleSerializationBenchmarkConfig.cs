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
                .WithLaunchCount(1)
                .WithIterationCount(30)
                .WithInvocationCount(1)
                .WithId("JOB-MULTIPLE")
                .WithRuntime(CoreRuntime.Core60)
                .WithToolchain(InProcessNoEmitToolchain.Instance));

            AddJob(baseJob);

            //AddJob(baseJob.WithInvocationCount(10).WithId("JOB-MULTIPLE-LIGHT"));
            //AddJob(baseJob.WithInvocationCount(100).WithId("JOB-MULTIPLE-MEDIUM"));
            //AddJob(baseJob.WithInvocationCount(1000).WithId("JOB-MULTIPLE-HEAVY"));

            //// Check the OS platform and set the profiler accordingly
            //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //{
            //    AddDiagnoser(new EtwProfiler());
            //}
            //else if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
            //{
            //    AddDiagnoser(new PerfCollectProfiler(new PerfCollectProfilerConfig(false)));
            //}

            AddAnalyser(EnvironmentAnalyser.Default);
            AddDiagnoser(MemoryDiagnoser.Default);
            ConfigHelper.AddDefaultColumns(this);
        }
    }
}


