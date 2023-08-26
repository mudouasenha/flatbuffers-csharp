using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Reports;
using Perfolizer.Horology;
using System.Globalization;

namespace Serialization.Benchmarks.Configs
{
    public static class ConfigHelper
    {
        /// <summary>
        /// A summary style that makes processing of data more accessible.
        /// * Long column width to avoid names being truncated
        /// * units stay the same
        /// * No units in cell data (Always numbers)
        /// </summary>
        public static readonly SummaryStyle CsvStyle = new SummaryStyle(CultureInfo.InvariantCulture, false, SizeUnit.B, TimeUnit.Nanosecond,
            false, true, 100);

        public static void AddDefaultColumns(ManualConfig config)
        {
            config.AddColumn(FixedColumn.VersionColumn);
            config.AddColumn(FixedColumn.OperatingSystemColumn);
            config.AddColumn(FixedColumn.DateTimeColumn);
            config.AddColumn(new EnvironmentVariableColumn("SystemTag", "SYSTEM_TAG"));

            config.AddExporter(MarkdownExporter.GitHub);
            config.AddExporter(CsvMeasurementsExporter.Default);
            config.AddExporter(RPlotExporter.Default);
            config.AddDiagnoser(MemoryDiagnoser.Default);
            config.AddDiagnoser(ThreadingDiagnoser.Default);
            config.AddExporter(new CsvExporter(CsvSeparator.Comma, CsvStyle));
        }
    }
}
