using System.Text.Json;

namespace FastCsv.Tests;

public class CsvFieldValidator_Integer_Tests
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
            ""description"": ""Integer type, tests for MIN/MAX length"",
            ""ordinal"": 1,
            ""type"": ""integer"",
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
    [InlineData(1, 1, "STR COL 1", 4)]
    [InlineData(1, 1, "STR COL 1", 5)]
    [InlineData(1, 1, "STR COL 1", 31)]
    [InlineData(1, 1, "STR COL 1", 32)]
    public void TestValidateIntegerField_Length_Success(int rowNum, int colPos, string colName, int fieldContent)
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
    [InlineData(22, 1, 1, "STR COL 1", Int32.MinValue)]
    [InlineData(22, 1, 1, "STR COL 1", -1)]
    [InlineData(22, 1, 1, "STR COL 1", 0)]
    [InlineData(22, 1, 1, "STR COL 1", 3)]
    [InlineData(21, 1, 1, "STR COL 1", 33)]
    [InlineData(21, 1, 1, "STR COL 1", Int32.MaxValue)]
    public void TestValidateIntegerField_Length_Fail(int expectedCode, int rowNum, int colPos, string colName, int fieldContent)
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
    public void TestValidateIntegerField_TypeCheck_Success(int rowNum, int colPos, string colName, string fieldContent)
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
    [InlineData(20, 1, 1, "STR COL 1", "ABCD")]
    [InlineData(20, 1, 1, "STR COL 1", "-1A")]
    [InlineData(20, 1, 1, "STR COL 1", "--1")]
    [InlineData(20, 1, 1, "STR COL 1", "0.1")]
    [InlineData(20, 1, 1, "STR COL 1", "0.12")]
    [InlineData(20, 1, 1, "STR COL 1", "09.12")]
    [InlineData(20, 1, 1, "STR COL 1", "3.14")]
    [InlineData(20, 1, 1, "STR COL 1", "32AF")]
    [InlineData(20, 1, 1, "STR COL 1", "0x00FF")]
    public void TestValidateIntegerField_TypeCheck_Fail(int expectedCode, int rowNum, int colPos, string colName, string fieldContent)
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