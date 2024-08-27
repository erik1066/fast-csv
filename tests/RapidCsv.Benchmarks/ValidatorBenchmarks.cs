using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace RapidCsv.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[MarkdownExporterAttribute.GitHub]
public class ValidatorBenchmarks
{
    private readonly CsvValidator _validator = new CsvValidator();
    private readonly ValidationOptions _options = new()
    {
        Separator = ',',
        HasHeaderRow = true
    };

    private void ValidateWithNoErrors(string path, int expectedHeaderCount)
    {
        using FileStream fs = File.OpenRead(path);
        var result = _validator.Validate(fs, _options);
        Trace.Assert(result.IsValid == true);
        Trace.Assert(result.FieldCount == expectedHeaderCount);
        Trace.Assert(result.ErrorCount == 0);
    }
    
    private void ValidateWithExpectedErrors(string path)
    {
        using FileStream fs = File.OpenRead(path);
        var result = _validator.Validate(fs, _options);
        Trace.Assert(result.IsValid == false);
        Trace.Assert(result.ErrorCount > 0);
    }

    [Benchmark]
    public void Validate_Failed_10Cols_by_100Rows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "invalid_data_10col_x_100row.csv");
        ValidateWithExpectedErrors(path: path);
    }
    
    [Benchmark]
    public void Validate_Failed_10Cols_by_1kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "invalid_data_10col_x_1000row.csv");
        ValidateWithExpectedErrors(path: path);
    }
    
    [Benchmark]
    public void Validate_Failed_10Cols_by_10kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "invalid_data_10col_x_10000row.csv");
        ValidateWithExpectedErrors(path: path);
    }
    
    [Benchmark]
    public void Validate_Failed_10Cols_by_100kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "invalid_data_10col_x_100000row.csv");
        ValidateWithExpectedErrors(path: path);
    }

    [Benchmark]
    public void Validate_Success_10Cols_by_100Rows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_100row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 10);
    }
    
    [Benchmark]
    public void Validate_Success_10Cols_by_1kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_1000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 10);
    }
    
    [Benchmark]
    public void Validate_Success_10Cols_by_10kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_10000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 10);
    }
    
    [Benchmark]
    public void Validate_Success_10Cols_by_100kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_100000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 10);
    }
    
    [Benchmark]
    public void Validate_Success_20Cols_by_100Rows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_20col_x_100row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 20);
    }
    
    [Benchmark]
    public void Validate_Success_20Cols_by_1kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_20col_x_1000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 20);
    }
    
    [Benchmark]
    public void Validate_Success_20Cols_by_10kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_20col_x_10000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 20);
    }
    
    [Benchmark]
    public void Validate_Success_20Cols_by_100kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_20col_x_100000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 20);
    }
    
    [Benchmark]
    public void Validate_Success_40Cols_by_100Rows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_40col_x_100row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 40);
    }
    
    [Benchmark]
    public void Validate_Success_40Cols_by_1kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_40col_x_1000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 40);
    }
    
    [Benchmark]
    public void Validate_Success_40Cols_by_10kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_40col_x_10000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 40);
    }
    
    [Benchmark]
    public void Validate_Success_40Cols_by_100kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_40col_x_100000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 40);
    }
}