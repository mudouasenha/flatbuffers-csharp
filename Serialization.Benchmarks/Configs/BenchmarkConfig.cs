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
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default));

            AddJob(Job.MediumRun
                .WithUnrollFactor(8)
                .WithWarmupCount(3)
                .WithIterationTime(TimeInterval.FromMilliseconds(250))
                .WithMinIterationCount(15)
                .WithMaxIterationCount(20)
                //.WithGcServer(true)
                //.WithGcConcurrent(true)
                //.WithGcForce(false)
                .WithRuntime(CoreRuntime.Core60)
                .WithToolchain(InProcessNoEmitToolchain.Instance));

            AddAnalyser(EnvironmentAnalyser.Default);

            AddDiagnoser(MemoryDiagnoser.Default);

            //AddColumn(new DataSizeColumn());
            ConfigHelper.AddDefaultColumns(this);
        }
    }
}


