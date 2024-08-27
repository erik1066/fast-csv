using System.Text.Json;

namespace RapidCsv.Tests;

public class CsvFieldValidator_String_Required_Tests
{
    private static readonly string ProfileJson01 = @"{
    ""$schema"": ""rapid-csv/validator-config-schema.json"",
    ""name"": ""Case Records"",
    ""description"": ""Case Records for Bureaucratitis 2020"",
    ""filename"": ""abc123.csv"",
    ""separator"": "","",
    ""has_header"": true,
    ""columns"": [
        {
            ""name"": ""STR COL 1"",
            ""description"": ""String type, tests for REQUIRED"",
            ""ordinal"": 1,
            ""type"": ""string"",
            ""max"": 4096,
            ""min"": 0,
            ""required"": true,
            ""null_or_empty"": false,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    private static readonly string ProfileJson02 = @"{
    ""$schema"": ""rapid-csv/validator-config-schema.json"",
    ""name"": ""Case Records"",
    ""description"": ""Case Records for Bureaucratitis 2020"",
    ""filename"": ""abc123.csv"",
    ""separator"": "","",
    ""has_header"": true,
    ""columns"": [
        {
            ""name"": ""STR COL 1"",
            ""description"": ""String type, tests for REQUIRED"",
            ""ordinal"": 1,
            ""type"": ""string"",
            ""max"": 4096,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": false,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    [Theory]
    [InlineData(1, 1, "STR COL 1", " ")]
    [InlineData(1, 1, "STR COL 1", "1")]
    [InlineData(1, 1, "STR COL 1", "A")]
    [InlineData(1, 1, "STR COL 1", "#")]
    [InlineData(1, 1, "STR COL 1", "$")]
    [InlineData(1, 1, "STR COL 1", "-")]
    [InlineData(1, 1, "STR COL 1", "123456789")]
    [InlineData(1, 1, "STR COL 1", "1234567890123456789012345678901234567890")]
    [InlineData(1, 1, "STR COL 1", "     ")]
    public void TestValidateStringField_Required_Success(int rowNum, int colPos, string colName, string fieldContent)
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
        
        Assert.Empty(messages);
    }
    
    [Theory]
    [InlineData(12, 1, 1, "STR COL 1", "")]
    public void TestValidateStringField_Required_Fail(int expectedCode, int rowNum, int colPos, string colName, string fieldContent)
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
    
    [Theory]
    [InlineData(1, 1, "STR COL 1", " ")]
    [InlineData(1, 1, "STR COL 1", "1")]
    [InlineData(1, 1, "STR COL 1", "A")]
    [InlineData(1, 1, "STR COL 1", "#")]
    [InlineData(1, 1, "STR COL 1", "$")]
    [InlineData(1, 1, "STR COL 1", "-")]
    [InlineData(1, 1, "STR COL 1", "123456789")]
    [InlineData(1, 1, "STR COL 1", "1234567890123456789012345678901234567890")]
    [InlineData(1, 1, "STR COL 1", "     ")]
    public void TestValidateStringField_NotNullOrEmpty_NotRequired_Success(int rowNum, int colPos, string colName, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson02) ?? new ValidationProfile();
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Empty(messages);
    }
    
    [Theory]
    [InlineData(13, 1, 1, "STR COL 1", "")]
    public void TestValidateStringField_NotNullOrEmpty_NotRequired_Fail(int expectedCode, int rowNum, int colPos, string colName, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson02) ?? new ValidationProfile();
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Single(messages);
        Assert.Equal(expectedCode, messages[0].Code);
    }
}