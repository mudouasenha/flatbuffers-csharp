using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System.Runtime.InteropServices;

namespace Serialization.Benchmarks.Configs.Columns
{
    /// <summary>
	/// Static column presenting the value of an environment variable of the system.
	/// Helpful for tagging machines with specific names.
	/// </summary>
	public class EnvironmentVariableColumn : IColumn
    {
        public string Id { get; }
        public string ColumnName { get; }

        private readonly string environmentVariableKey;
        private readonly string cellValue;

        public EnvironmentVariableColumn(string columnName, string environmentVariableKey, string defaultValue = "Not set")
        {
            this.environmentVariableKey = environmentVariableKey;
            ColumnName = columnName;
            cellValue = Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.Process);

            // Fallbacks for windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                cellValue ??= Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.User);
                cellValue ??= Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.Machine);
            }

            cellValue ??= defaultValue;

            Id = nameof(EnvironmentVariableColumn) + "." + ColumnName;
        }

        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
        public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => cellValue;
        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) => GetValue(summary, benchmarkCase);

        public bool IsAvailable(Summary summary) => true;

        public bool AlwaysShow { get; set; } = false;
        public ColumnCategory Category => ColumnCategory.Custom;
        public int PriorityInCategory { get; set; } = 0;
        public bool IsNumeric => false;
        public UnitType UnitType => UnitType.Dimensionless;
        public string Legend => $"Environment variable {environmentVariableKey} has value '{cellValue}'";
        public override string ToString() => ColumnName;
    }
}
