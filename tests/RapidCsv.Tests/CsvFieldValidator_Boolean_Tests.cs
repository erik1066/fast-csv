using System.Text.Json;

namespace RapidCsv.Tests;

public class CsvFieldValidator_Boolean_Tests
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
            ""description"": ""Boolean type"",
            ""ordinal"": 1,
            ""type"": ""boolean"",
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    [Theory]
    [InlineData(1, 1, "STR COL 1", "")]
    public void TestValidateBooleanField_TypeCheck_Required_Success(int rowNum, int colPos, string colName, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        csvValidationProfile.Columns[0].Required = false;
        ;
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Empty(messages);
    }
    
    [Theory]
    [InlineData(1, 1, "STR COL 1", "true")]
    [InlineData(1, 1, "STR COL 1", "True")]
    [InlineData(1, 1, "STR COL 1", "TRUE")]
    [InlineData(1, 1, "STR COL 1", "false")]
    [InlineData(1, 1, "STR COL 1", "False")]
    [InlineData(1, 1, "STR COL 1", "FALSE")]
    [InlineData(1, 1, "STR COL 1", " true")]
    [InlineData(1, 1, "STR COL 1", " true ")]
    [InlineData(1, 1, "STR COL 1", "true ")]
    [InlineData(1, 1, "STR COL 1", " false")]
    [InlineData(1, 1, "STR COL 1", " false ")]
    [InlineData(1, 1, "STR COL 1", "false ")]
    [InlineData(1, 1, "STR COL 1", " True")]
    [InlineData(1, 1, "STR COL 1", " True ")]
    [InlineData(1, 1, "STR COL 1", "True ")]
    [InlineData(1, 1, "STR COL 1", " False")]
    [InlineData(1, 1, "STR COL 1", " False ")]
    [InlineData(1, 1, "STR COL 1", "False ")]
    public void TestValidateBooleanField_TypeCheck_Success(int rowNum, int colPos, string colName, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        ;
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Empty(messages);
    }
    
    [Theory]
    [InlineData(40, 1, 1, "STR COL 1", "YES")]
    [InlineData(40, 1, 1, "STR COL 1", "NO")]
    [InlineData(40, 1, 1, "STR COL 1", "Yes")]
    [InlineData(40, 1, 1, "STR COL 1", "No")]
    [InlineData(40, 1, 1, "STR COL 1", "yes")]
    [InlineData(40, 1, 1, "STR COL 1", "no")]
    [InlineData(40, 1, 1, "STR COL 1", "tru")]
    [InlineData(40, 1, 1, "STR COL 1", "Tru")]
    [InlineData(40, 1, 1, "STR COL 1", "TRU")]
    [InlineData(40, 1, 1, "STR COL 1", "truee")]
    [InlineData(40, 1, 1, "STR COL 1", "Truee")]
    [InlineData(40, 1, 1, "STR COL 1", "TRUEE")]
    [InlineData(40, 1, 1, "STR COL 1", "fals")]
    [InlineData(40, 1, 1, "STR COL 1", "Fals")]
    [InlineData(40, 1, 1, "STR COL 1", "FALS")]
    [InlineData(40, 1, 1, "STR COL 1", "ABCD")]
    [InlineData(40, 1, 1, "STR COL 1", "-1A")]
    [InlineData(40, 1, 1, "STR COL 1", "--1")]
    [InlineData(40, 1, 1, "STR COL 1", "1")]
    [InlineData(40, 1, 1, "STR COL 1", "0")]
    [InlineData(40, 1, 1, "STR COL 1", "0.1")]
    [InlineData(40, 1, 1, "STR COL 1", "0.12")]
    [InlineData(40, 1, 1, "STR COL 1", "09.12")]
    [InlineData(40, 1, 1, "STR COL 1", "3.14")]
    [InlineData(40, 1, 1, "STR COL 1", "32AF")]
    [InlineData(40, 1, 1, "STR COL 1", "0x00FF")]
    public void TestValidateBooleanField_TypeCheck_Fail(int expectedCode, int rowNum, int colPos, string colName, string fieldContent)
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