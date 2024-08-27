using System.Text.Json;

namespace RapidCsv.Tests;

public sealed class CsvContentValidation_Enum_Tests
{
    private static readonly string ProfileJson03 = @"{
    ""$schema"": ""fast-csv/validator-config-schema.json"",
    ""name"": ""Case Records"",
    ""description"": ""Case Records for Bureaucratitis 2020"",
    ""filename"": ""abc123.csv"",
    ""separator"": "","",
    ""has_header"": true,
    ""columns"": [
        {
            ""name"": ""STR COL 1"",
            ""description"": """",
            ""ordinal"": 1,
            ""type"": ""enum"",
            ""values"": [ ""Confirmed"", ""Probable"", ""Suspect"" ],
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    private static readonly string ProfileJson04 = @"{
    ""$schema"": ""fast-csv/validator-config-schema.json"",
    ""name"": ""Case Records"",
    ""description"": ""Case Records for Bureaucratitis 2020"",
    ""filename"": ""abc123.csv"",
    ""separator"": "","",
    ""has_header"": true,
    ""columns"": [
        {
            ""name"": ""STR COL 1"",
            ""description"": """",
            ""ordinal"": 1,
            ""type"": ""enum"",
            ""values"": [ ""Confirmed"", ""Probable"", ""Suspect"" ],
            ""max"": 25,
            ""min"": 0,
            ""required"": true,
            ""null_or_empty"": false,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    private static readonly string ProfileJson05 = @"{
    ""$schema"": ""fast-csv/validator-config-schema.json"",
    ""name"": ""Case Records"",
    ""description"": ""Case Records for Bureaucratitis 2020"",
    ""filename"": ""abc123.csv"",
    ""separator"": "","",
    ""has_header"": true,
    ""columns"": [
        {
            ""name"": ""STR COL 1"",
            ""description"": """",
            ""ordinal"": 1,
            ""type"": ""enum"",
            ""values"": [ ""Confirmed"", ""Probable"", ""Suspect"" ],
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""STR COL 2"",
            ""description"": """",
            ""ordinal"": 2,
            ""type"": ""enum"",
            ""values"": [ ""Confirmed"", ""Probable"", ""Suspect"" ],
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    [Theory]
    [InlineData("STR COL 1")]
    [InlineData("STR COL 1\n\nProbable")]
    [InlineData("STR COL 1\nConfirmed")]
    [InlineData("STR COL 1\nConfirmed\nProbable")]
    [InlineData("STR COL 1\r\nConfirmed")]
    [InlineData("STR COL 1\r\nConfirmed\r\nProbable")]
    [InlineData("STR COL 1\r\nConfirmed\r\nProbable\r\nSuspect")]
    [InlineData("STR COL 1\r\nConfirmed\r\nProbable\r\nSuspect\r\nProbable")]
    public void TestSchemaValidationForEnumFields_Success_01(string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson03
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.True(result.IsValid);
    }
    
    [Theory]
    [InlineData("STR COL 1,STR COL 2\r\n,")]
    [InlineData("STR COL 1,STR COL 2\r\n,\r\n,Probable")]
    [InlineData("STR COL 1,STR COL 2\r\n,Confirmed")]
    [InlineData("STR COL 1,STR COL 2\r\nConfirmed,\r\nProbable,")]
    [InlineData("STR COL 1,STR COL 2\r\nConfirmed,Probable")]
    [InlineData("STR COL 1,STR COL 2\r\nConfirmed,Probable\r\nProbable,Suspect")]
    public void TestSchemaValidationForEnumFields_Success_02(string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson05
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.True(result.IsValid);
    }
    
    [Theory]
    [InlineData("STR COL 1\nConfirme")]
    [InlineData("STR COL 1\nConfirme\nProbable")]
    [InlineData("STR COL 1\r\nConfirme")]
    [InlineData("STR COL 1\r\nConfirme\r\nProbable")]
    [InlineData("STR COL 1\nConfirmedd")]
    [InlineData("STR COL 1\nConfirmedd\nProbable")]
    [InlineData("STR COL 1\r\nConfirmedd")]
    [InlineData("STR COL 1\r\nConfirmedd\r\nProbable")]
    [InlineData("STR COL 1\nCConfirmed")]
    [InlineData("STR COL 1\nCConfirmed\nProbable")]
    [InlineData("STR COL 1\r\nCConfirmed")]
    [InlineData("STR COL 1\r\nCConfirmed\r\nProbable")]
    public void TestSchemaValidationForEnumFields_Fail(string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson03
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Single(result.Messages);
        foreach (var message in result.Messages)
        {
            Assert.Equal(9, message.Code);
        }
        Assert.False(result.IsValid);
    }
    
    [Theory]
    [InlineData("STR COL 1\nConfirme\nProbabl")]
    [InlineData("STR COL 1\r\nConfirme\r\nProbabl")]
    [InlineData("STR COL 1\nConfirmedd\nProbablee")]
    [InlineData("STR COL 1\r\nConfirmedd\r\nProbablee")]
    public void TestSchemaValidationForEnumFields_Multiple_Fail(string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson03
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(2, result.Messages.Count);
        foreach (var message in result.Messages)
        {
            Assert.Equal(9, message.Code);
        }
        Assert.False(result.IsValid);
    }
    
    [Theory]
    [InlineData("STR COL 1\nConfirmed")]
    [InlineData("STR COL 1\nConfirmed\nProbable")]
    [InlineData("STR COL 1\r\nConfirmed")]
    [InlineData("STR COL 1\r\nConfirmed\r\nProbable")]
    public void TestSchemaValidationForEnumFields_Required_Fail(string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson04
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.True(result.IsValid);
    }
    
    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}