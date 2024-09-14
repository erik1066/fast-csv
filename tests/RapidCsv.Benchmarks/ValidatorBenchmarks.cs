using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace RapidCsv.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[MarkdownExporterAttribute.GitHub]
public class ValidatorBenchmarks
{
    private const string profile = @"{
    ""$schema"": ""rapid-csv/validator-config-schema.json"",
    ""name"": ""Acme Bookstore Customer Records"",
    ""description"": ""Validation profile for the CSV records of our Acme bookstore customers"",
    ""filename"": ""abc123.csv"",
    ""separator"": "","",
    ""has_header"": true,
    ""columns"": [
        {
            ""name"": ""FIRST"",
            ""description"": ""The customer's first name"",
            ""ordinal"": 1,
            ""type"": ""string"",
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""LAST"",
            ""description"": ""The customer's last name"",
            ""ordinal"": 2,
            ""type"": ""string"",
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""MI"",
            ""description"": ""The customer's middle initial"",
            ""ordinal"": 3,
            ""type"": ""string"",
            ""max"": 1,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""AGE"",
            ""description"": ""The customer's age"",
            ""ordinal"": 4,
            ""type"": ""integer"",
            ""max"": 125,
            ""min"": 7,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""DOB"",
            ""description"": ""The customer's date of birth"",
            ""ordinal"": 5,
            ""type"": ""string"",
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": ""m/d/yyyy"",
            ""regex"": null
        },
        {
            ""name"": ""GENDER"",
            ""description"": ""The customer's gender"",
            ""ordinal"": 6,
            ""type"": ""enum"",
            ""values"": [ ""M"", ""F"" ],
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""STREET"",
            ""description"": ""The customer's address"",
            ""ordinal"": 7,
            ""type"": ""string"",
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""CITY"",
            ""description"": ""The customer's address"",
            ""ordinal"": 8,
            ""type"": ""string"",
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""STATE"",
            ""description"": ""The customer's address"",
            ""ordinal"": 9,
            ""type"": ""string"",
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""ZIP"",
            ""description"": ""The customer's zip code"",
            ""ordinal"": 10,
            ""type"": ""string"",
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": ""######"",
            ""regex"": null
        }
    ]
}";
    private readonly CsvValidator _validator = new CsvValidator();
    private readonly ValidationOptions _options = new()
    {
        Separator = ',',
        HasHeaderRow = true
    };
    
    private readonly ValidationOptions _optionsWithProfile = new()
    {
        Separator = ',',
        HasHeaderRow = true,
        ValidationProfile = profile
    };

    private void ValidateWithNoErrors(string path, int expectedHeaderCount)
    {
        using FileStream fs = File.OpenRead(path);
        var result = _validator.Validate(fs, _options);
        Trace.Assert(result.IsValid == true);
        Trace.Assert(result.FieldCount == expectedHeaderCount);
        Trace.Assert(result.ErrorCount == 0);
    }
    
    private void ValidateContentWithNoErrors(string path, int expectedHeaderCount)
    {
        using FileStream fs = File.OpenRead(path);
        var result = _validator.Validate(fs, _optionsWithProfile);
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
    public void RFC4180_Validate_Failed_10Cols_by_100Rows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "invalid_data_10col_x_100row.csv");
        ValidateWithExpectedErrors(path: path);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Failed_10Cols_by_1kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "invalid_data_10col_x_1000row.csv");
        ValidateWithExpectedErrors(path: path);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Failed_10Cols_by_10kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "invalid_data_10col_x_10000row.csv");
        ValidateWithExpectedErrors(path: path);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Failed_10Cols_by_100kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "invalid_data_10col_x_100000row.csv");
        ValidateWithExpectedErrors(path: path);
    }

    [Benchmark]
    public void RFC4180_Validate_Success_10Cols_by_100Rows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_100row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 10);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_10Cols_by_1kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_1000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 10);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_10Cols_by_10kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_10000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 10);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_10Cols_by_100kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_100000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 10);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_20Cols_by_100Rows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_20col_x_100row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 20);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_20Cols_by_1kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_20col_x_1000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 20);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_20Cols_by_10kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_20col_x_10000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 20);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_20Cols_by_100kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_20col_x_100000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 20);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_40Cols_by_100Rows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_40col_x_100row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 40);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_40Cols_by_1kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_40col_x_1000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 40);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_40Cols_by_10kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_40col_x_10000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 40);
    }
    
    [Benchmark]
    public void RFC4180_Validate_Success_40Cols_by_100kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_40col_x_100000row.csv");
        ValidateWithNoErrors(path: path, expectedHeaderCount: 40);
    }
    
    [Benchmark]
    public void Content_Validate_Success_10Cols_by_100Rows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_100row.csv");
        ValidateContentWithNoErrors(path: path, expectedHeaderCount: 10);
    }
    
    [Benchmark]
    public void Content_Validate_Success_10Cols_by_1kRows()
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Data", "valid_data_10col_x_1000row.csv");
        ValidateContentWithNoErrors(path: path, expectedHeaderCount: 10);
    }
}