using System.Text.Json;

namespace FastCsv.Tests;

public sealed class ValidationProfileTests
{
    private static string ProfileJson01 = @"{
    ""$schema"": ""fast-csv/validator-config-schema.json"",
    ""name"": ""Case Records"",
    ""description"": ""Case Records for Bureaucratitis 2020"",
    ""filename"": ""abc123.csv"",
    ""separator"": "","",
    ""has_header"": true,
    ""columns"": [
        {
            ""name"": ""FIRST NAME"",
            ""description"": ""First name of the patient"",
            ""ordinal"": 1,
            ""type"": ""string"",
            ""enum"": null,
            ""max"": 255,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""STATUS"",
            ""description"": ""Case status of the patient"",
            ""ordinal"": 2,
            ""type"": ""enum"",
            ""values"": [ ""Confirmed"", ""Probable"", ""Suspect"" ],
            ""max"": 255,
            ""min"": 0,
            ""required"": true,
            ""null_or_empty"": false,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""AGE"",
            ""description"": ""Age of the patient"",
            ""ordinal"": 3,
            ""type"": ""integer"",
            ""values"": null,
            ""max"": 150,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""TEMP"",
            ""description"": ""Temperature of the patient"",
            ""ordinal"": 4,
            ""type"": ""decimal"",
            ""values"": null,
            ""max"": 150,
            ""min"": 20,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""ACTIVE"",
            ""description"": ""Is an active record"",
            ""ordinal"": 5,
            ""type"": ""boolean"",
            ""values"": null,
            ""max"": null,
            ""min"": null,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    private static string ProfileJson02 = @"{
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
            ""type"": ""string"",
            ""max"": 15,
            ""min"": 10,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""STR COL 2"",
            ""description"": """",
            ""ordinal"": 2,
            ""type"": ""string"",
            ""max"": 255,
            ""min"": 0,
            ""required"": true,
            ""null_or_empty"": false,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""STR COL 3"",
            ""description"": """",
            ""ordinal"": 3,
            ""type"": ""string"",
            ""max"": 255,
            ""min"": 1,
            ""required"": false,
            ""null_or_empty"": false,
            ""format"": null,
            ""regex"": null
        }
    ]
}";
    
    [Fact]
    public void TestProfileCreation01()
    {
        ValidationProfile? profile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01);
        List<ValidationColumnProfile>? columnProfiles = profile?.Columns;
        
        Assert.NotNull(profile);
        Assert.NotNull(columnProfiles);
        
        Assert.Equal("Case Records", profile.Name);
        Assert.Equal("Case Records for Bureaucratitis 2020", profile.Description);
        Assert.Equal("abc123.csv", profile.FileName);
        Assert.Equal(",", profile.Separator);
        Assert.True(profile.HasHeader);
        Assert.Equal(5, columnProfiles.Count);
        
        Assert.Equal("FIRST NAME", columnProfiles[0].Name);
        Assert.Equal("First name of the patient", columnProfiles[0].Description);
        Assert.Equal(1, columnProfiles[0].Ordinal);
        Assert.Equal("string", columnProfiles[0].Type);
        Assert.Empty(columnProfiles[0].Values);
        Assert.Equal(0, columnProfiles[0].Min);
        Assert.Equal(255, columnProfiles[0].Max);
        Assert.False(columnProfiles[0].Required);
        Assert.True(columnProfiles[0].CanBeNullOrEmpty);
        Assert.Null(columnProfiles[0].Format);
        Assert.Null(columnProfiles[0].Regex);
        
        Assert.Equal("STATUS", columnProfiles[1].Name);
        Assert.Equal("Case status of the patient", columnProfiles[1].Description);
        Assert.Equal(2, columnProfiles[1].Ordinal);
        Assert.Equal("enum", columnProfiles[1].Type);
        Assert.Equal(3, columnProfiles[1].Values.Count);
        Assert.Equal("Confirmed", columnProfiles[1].Values[0]);
        Assert.Equal("Probable", columnProfiles[1].Values[1]);
        Assert.Equal("Suspect", columnProfiles[1].Values[2]);
        Assert.Equal(0, columnProfiles[1].Min);
        Assert.Equal(255, columnProfiles[1].Max);
        Assert.True(columnProfiles[1].Required);
        Assert.False(columnProfiles[1].CanBeNullOrEmpty);
        Assert.Null(columnProfiles[1].Format);
        Assert.Null(columnProfiles[1].Regex);
        
        Assert.Equal("AGE", columnProfiles[2].Name);
        Assert.Equal("Age of the patient", columnProfiles[2].Description);
        Assert.Equal(3, columnProfiles[2].Ordinal);
        Assert.Equal("integer", columnProfiles[2].Type);
        Assert.Null(columnProfiles[2].Values);
        Assert.Equal(0, columnProfiles[2].Min);
        Assert.Equal(150, columnProfiles[2].Max);
        Assert.False(columnProfiles[2].Required);
        Assert.True(columnProfiles[2].CanBeNullOrEmpty);
        Assert.Null(columnProfiles[2].Format);
        Assert.Null(columnProfiles[2].Regex);
        
        Assert.Equal("TEMP", columnProfiles[3].Name);
        Assert.Equal("Temperature of the patient", columnProfiles[3].Description);
        Assert.Equal(4, columnProfiles[3].Ordinal);
        Assert.Equal("decimal", columnProfiles[3].Type);
        Assert.Null(columnProfiles[3].Values);
        Assert.Equal(20, columnProfiles[3].Min);
        Assert.Equal(150, columnProfiles[3].Max);
        Assert.False(columnProfiles[3].Required);
        Assert.True(columnProfiles[3].CanBeNullOrEmpty);
        Assert.Null(columnProfiles[3].Format);
        Assert.Null(columnProfiles[3].Regex);
        
        Assert.Equal("ACTIVE", columnProfiles[4].Name);
        Assert.Equal("Is an active record", columnProfiles[4].Description);
        Assert.Equal(5, columnProfiles[4].Ordinal);
        Assert.Equal("boolean", columnProfiles[4].Type);
        Assert.Null(columnProfiles[4].Values);
        Assert.Equal(Int64.MinValue, columnProfiles[4].Min);
        Assert.Equal(Int64.MaxValue, columnProfiles[4].Max);
        Assert.False(columnProfiles[4].Required);
        Assert.True(columnProfiles[4].CanBeNullOrEmpty);
        Assert.Null(columnProfiles[4].Format);
        Assert.Null(columnProfiles[4].Regex);
    }
    
    [Theory]
    [InlineData(true, "FIRST NAME,STATUS,AGE,TEMP,ACTIVE\nJohn,Confirmed,23,99.8,true", 1, 5)]
    public void TestSchemaBasedValidationSimple(bool isValid, string csvContent, int expectedDataRows, int expectedFieldCount)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson01
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(expectedDataRows, result.DataRowCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData("STR COL 1,STR COL 2,STR COL 3\nfirst field val,second field value,third field value", 1, 3)]
    public void TestSchemaBasedValidationForStringFields_Success(
        string csvContent, 
        int expectedDataRows, 
        int expectedFieldCount)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson02
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(expectedDataRows, result.DataRowCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.True(result.IsValid);
    }
    
    [Theory]
    [InlineData(1, 3, 1, 1, 10, "STR COL 1,STR COL 2,STR COL 3\nfirst field value,second field value,third field value")]
    public void TestSchemaBasedValidationForStringFields_Fail(
        int expectedDataRows, 
        int expectedFieldCount,
        int expectedRowErrorPosition,
        int expectedFieldErrorPosition,
        int expectedErrorCode,
        string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson02
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(expectedDataRows, result.DataRowCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.Equal(expectedRowErrorPosition, result.Messages[0].Row);
        Assert.Equal(expectedFieldErrorPosition, result.Messages[0].FieldNumber);
        Assert.Equal(expectedErrorCode, result.Messages[0].Code);
        Assert.False(result.IsValid);
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