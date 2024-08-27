using System.Globalization;
using System.Text.Json;

namespace RapidCsv.Tests;

public class CsvFieldValidator_Decimal_Tests
{
    private static readonly string ProfileJson01 = @"{
    ""$schema"": ""fast-csv/validator-config-schema.json"",
    ""name"": ""Case Records"",
    ""description"": ""Case Records for Bureaucratitis 2020"",
    ""filename"": ""abc123.csv"",
    ""separator"": "","",
    ""has_header"": true,
    ""columns"": [
        {
            ""name"": ""STR COL 1"",
            ""description"": ""Decimal type, tests for MIN/MAX length"",
            ""ordinal"": 1,
            ""type"": ""decimal"",
            ""max"": 32,
            ""min"": 4,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    [Theory]
    [InlineData(1, 1, "STR COL 1", 4.0)]
    [InlineData(1, 1, "STR COL 1", 5.0)]
    [InlineData(1, 1, "STR COL 1", 31.5)]
    [InlineData(1, 1, "STR COL 1", 32.0)]
    public void TestValidateDecimalField_Length_Success(int rowNum, int colPos, string colName, double fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.ToString().AsSpan(), rowNum, colPos);
        
        Assert.Empty(messages);
    }
    
    [Theory]
    [InlineData(32, 1, 1, "STR COL 1", double.MinValue)]
    [InlineData(32, 1, 1, "STR COL 1", -1.0)]
    [InlineData(32, 1, 1, "STR COL 1", 0.0)]
    [InlineData(32, 1, 1, "STR COL 1", 3.0)]
    [InlineData(32, 1, 1, "STR COL 1", 3.9)]
    [InlineData(32, 1, 1, "STR COL 1", 3.99999999999)]
    [InlineData(31, 1, 1, "STR COL 1", 33.0)]
    [InlineData(31, 1, 1, "STR COL 1", 33.1)]
    [InlineData(31, 1, 1, "STR COL 1", double.MaxValue)]
    public void TestValidateDecimalField_Length_Fail(int expectedCode, int rowNum, int colPos, string colName, double fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.ToString(CultureInfo.CurrentCulture).AsSpan(), rowNum, colPos);
        
        Assert.Single(messages);
        Assert.Equal(expectedCode, messages[0].Code);
    }
    
    [Theory]
    [InlineData(1, 1, "STR COL 1", "-1")]
    [InlineData(1, 1, "STR COL 1", "0")]
    [InlineData(1, 1, "STR COL 1", "-0")]
    [InlineData(1, 1, "STR COL 1", "1")]
    [InlineData(1, 1, "STR COL 1", "4200200200")]
    [InlineData(1, 1, "STR COL 1", "-4200200200")]
    [InlineData(1, 1, "STR COL 1", "1.0")]
    [InlineData(1, 1, "STR COL 1", "-1.0")]
    [InlineData(1, 1, "STR COL 1", " 1.0")]
    [InlineData(1, 1, "STR COL 1", "1.0 ")]
    [InlineData(1, 1, "STR COL 1", " 1.0 ")]
    [InlineData(1, 1, "STR COL 1", " -1.0 ")]
    [InlineData(1, 1, "STR COL 1", "-1.0 ")]
    [InlineData(1, 1, "STR COL 1", " -1.0")]
    [InlineData(1, 1, "STR COL 1", "0.0")]
    [InlineData(1, 1, "STR COL 1", "0.1")]
    [InlineData(1, 1, "STR COL 1", "3.1415926535897932384626433")]
    [InlineData(1, 1, "STR COL 1", "-3.1415926535897932384626433")]
    public void TestValidateDecimalField_TypeCheck_Success(int rowNum, int colPos, string colName, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        csvValidationProfile.Columns[0].Min = long.MinValue;
        csvValidationProfile.Columns[0].Max = long.MaxValue;
        ;
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Empty(messages);
    }
    
    [Theory]
    [InlineData(30, 1, 1, "STR COL 1", "ABCD")]
    [InlineData(30, 1, 1, "STR COL 1", "-1A")]
    [InlineData(30, 1, 1, "STR COL 1", "--1")]
    [InlineData(30, 1, 1, "STR COL 1", "0.1.1")]
    [InlineData(30, 1, 1, "STR COL 1", "0.12.1")]
    [InlineData(30, 1, 1, "STR COL 1", "09.1.2")]
    [InlineData(30, 1, 1, "STR COL 1", "--3.14")]
    [InlineData(30, 1, 1, "STR COL 1", "32AF")]
    [InlineData(30, 1, 1, "STR COL 1", "0x00FF")]
    public void TestValidateDecimalField_TypeCheck_Fail(int expectedCode, int rowNum, int colPos, string colName, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Single(messages);
        Assert.Equal(expectedCode, messages[0].Code);
    }
}