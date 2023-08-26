using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;

namespace Serialization.Benchmarks.Configs
{
    public class DebugBenchmarkConfig : ManualConfig
    {
        public DebugBenchmarkConfig()
        {
            Add(DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default));

            AddJob(Job.InProcess
                .RunOncePerIteration()
                .WithGcServer(true)
                .WithGcConcurrent(true)
                .WithGcForce(false));

            AddLogger(ConsoleLogger.Default);
            AddColumnProvider(DefaultColumnProviders.Instance);
            //AddColumn(new DataSizeColumn());
        }
    }
}
