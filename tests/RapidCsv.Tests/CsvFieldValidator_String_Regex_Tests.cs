using System.Text.Json;

namespace RapidCsv.Tests;

public class CsvFieldValidator_String_Regex_Tests
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
            ""description"": ""String type, tests for FORMAT"",
            ""ordinal"": 1,
            ""type"": ""string"",
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""STR COL 2"",
            ""description"": ""String type, tests for FORMAT"",
            ""ordinal"": 2,
            ""type"": ""string"",
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    [Theory]
    [InlineData(1, 1, "STR COL 1", "[a-z][a-z][a-z]", "cat")]
    // [InlineData(1, 1, "STR COL 1", "[a-z][a-z][a-z]", "dog")]
    public void TestValidateStringField_Regex_Success(int rowNum, int colPos, string colName, string regex, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        var columnProfile = csvValidationProfile.Columns[colPos - 1];
        columnProfile.Regex = regex;
        
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Empty(messages);
    }
    
    [Theory]
    [InlineData(19, 1, 1, "STR COL 1", "[a-z][a-z][a-z]", "ca")]
    public void TestValidateStringField_NumberFormat_Fail(int expectedCode, int rowNum, int colPos, string colName, string regex, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        var columnProfile = csvValidationProfile.Columns[colPos - 1];
        columnProfile.Regex = regex;
        
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Single(messages);
        Assert.Equal(expectedCode, messages[0].Code);
    }
}