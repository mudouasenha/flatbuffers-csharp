using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace Serialization.Benchmarks.Configs
{
    public class RESTSerializationBenchmarkConfig : ManualConfig
    {
        public RESTSerializationBenchmarkConfig()
        {
            Add(DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default));

            var baseJob = new Job(Job.ShortRun
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.ColdStart)
                .WithUnrollFactor(1)
                .WithWarmupCount(0)
                .WithEvaluateOverhead(false)
                //.WithMinIterationTime(TimeInterval.FromMilliseconds(800))
                .WithLaunchCount(1)
                .WithIterationCount(1)
                //.WithIterationCount(100)
                .WithInvocationCount(1)
                .WithId("JOB-REST")
                .WithRuntime(CoreRuntime.Core60)
                .WithToolchain(InProcessNoEmitToolchain.Instance));

            AddJob(baseJob);

            //AddAnalyser(EnvironmentAnalyser.Default);
            //AddDiagnoser(MemoryDiagnoser.Default);
            ConfigHelper.AddDefaultColumns(this);
        }
    }
}