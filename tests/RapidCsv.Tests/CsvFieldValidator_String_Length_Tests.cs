using System.Text.Json;

namespace RapidCsv.Tests;

public class CsvFieldValidator_String_Length_Tests
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
            ""description"": ""String type, tests for MIN/MAX length"",
            ""ordinal"": 1,
            ""type"": ""string"",
            ""max"": 10,
            ""min"": 5,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    [Theory]
    [InlineData(1, 1, "STR COL 1", "12345")]
    [InlineData(1, 1, "STR COL 1", "123456")]
    [InlineData(1, 1, "STR COL 1", "1234567")]
    [InlineData(1, 1, "STR COL 1", "12345678")]
    [InlineData(1, 1, "STR COL 1", "123456789")]
    [InlineData(1, 1, "STR COL 1", "1234567890")]
    [InlineData(1, 1, "STR COL 2", "12345")]
    [InlineData(1, 1, "STR COL 2", "123456")]
    [InlineData(1, 1, "STR COL 2", "1234567")]
    [InlineData(1, 1, "STR COL 2", "12345678")]
    [InlineData(1, 1, "STR COL 2", "123456789")]
    [InlineData(1, 1, "STR COL 2", "1234567890")]
    [InlineData(1, 1, "STR COL 1", "     ")]
    [InlineData(1, 1, "STR COL 1", "      ")]
    [InlineData(1, 1, "STR COL 1", "       ")]
    [InlineData(1, 1, "STR COL 1", "        ")]
    [InlineData(1, 1, "STR COL 1", "         ")]
    [InlineData(1, 1, "STR COL 1", "          ")]
    [InlineData(1, 1, "STR COL 1", "A    ")]
    [InlineData(1, 1, "STR COL 1", "A     ")]
    [InlineData(1, 1, "STR COL 1", "A      ")]
    [InlineData(1, 1, "STR COL 1", "A       ")]
    [InlineData(1, 1, "STR COL 1", "A        ")]
    [InlineData(1, 1, "STR COL 1", "A         ")]
    [InlineData(1, 1, "STR COL 1", "A   B")]
    [InlineData(1, 1, "STR COL 1", "A    B")]
    [InlineData(1, 1, "STR COL 1", "A     B")]
    [InlineData(1, 1, "STR COL 1", "A      B")]
    [InlineData(1, 1, "STR COL 1", "A       B")]
    [InlineData(1, 1, "STR COL 1", "A        B")]
    public void TestValidateStringField_Length_Success(int rowNum, int colPos, string colName, string fieldContent)
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
    [InlineData(11, 1, 1, "STR COL 1", "")]
    [InlineData(11, 1, 1, "STR COL 1", "1")]
    [InlineData(11, 1, 1, "STR COL 1", "12")]
    [InlineData(11, 1, 1, "STR COL 1", "123")]
    [InlineData(11, 1, 1, "STR COL 1", "1234")]
    [InlineData(10, 1, 1, "STR COL 1", "12345678901")]
    [InlineData(10, 1, 1, "STR COL 1", "123456789012")]
    [InlineData(10, 1, 1, "STR COL 1", "1234567890123")]
    [InlineData(11, 1, 1, "STR COL 1", "A")]
    [InlineData(11, 1, 1, "STR COL 1", "AB")]
    [InlineData(11, 1, 1, "STR COL 1", "ABC")]
    [InlineData(11, 1, 1, "STR COL 1", "ABCD")]
    [InlineData(11, 1, 1, "STR COL 1", "ÉÁÉÁ")]
    [InlineData(10, 1, 1, "STR COL 1", "ABCDEFGHIJK")]
    [InlineData(10, 1, 1, "STR COL 1", "ÉÁÉÁÇÈÉÊËÌÍÎÑÕÖÙÚ")]
    [InlineData(11, 1, 1, "STR COL 1", "    ")]
    [InlineData(10, 1, 1, "STR COL 1", "           ")]
    public void TestValidateStringField_Length_Fail(int expectedCode, int rowNum, int colPos, string colName, string fieldContent)
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