using System.Text.Json;

namespace RapidCsv.Tests;

public sealed class CsvContentValidation_String_Tests
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
    
    private static readonly string ProfileJson06 = @"{
    ""$schema"": ""rapid-csv/validator-config-schema.json"",
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
            ""max"": 45,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": ""###-###-####"",
            ""regex"": null
        },
{
            ""name"": ""STR COL 2"",
            ""description"": """",
            ""ordinal"": 2,
            ""type"": ""string"",
            ""max"": 45,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": ""mm/dd/yyyy"",
            ""regex"": null
        }
    ]
}";

    [Theory]
    [InlineData(true, "FIRST NAME,STATUS,AGE,TEMP,ACTIVE\nJohn,Confirmed,23,99.8,true", 1, 5)]
    public void TestSchemaValidationSimple(bool isValid, string csvContent, int expectedDataRows, int expectedFieldCount)
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
    [InlineData("STR COL 1,STR COL 2,STR COL 3\nfirst fiel,second field value,third field value", 1, 3)]
    [InlineData("STR COL 1,STR COL 2,STR COL 3\r\nfirst field val,second field value,third field value", 1, 3)]
    [InlineData("STR COL 1,STR COL 2,STR COL 3\r\nfirst fiel,second field value,third field value", 1, 3)]
    [InlineData("STR COL 1,STR COL 2,STR COL 3\r\nfirst field val,second field value,third field value\r\nfirst field val,second field value,third field value", 2, 3)]
    [InlineData("STR COL 1,STR COL 2,STR COL 3\r\nfirst fiel,second field value,third field value\r\nfirst fiel,second field value,third field value", 2, 3)]
    public void TestSchemaValidationForStringFields_Success(
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
    [InlineData(1, 3, 1, 1, 10, "STR COL 1,STR COL 2,STR COL 3\nfirst field valu,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 10, "STR COL 1,STR COL 2,STR COL 3\nfirst field value,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 10, "STR COL 1,STR COL 2,STR COL 3\n1234567890123456,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 10, "STR COL 1,STR COL 2,STR COL 3\n                ,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 10, "STR COL 1,STR COL 2,STR COL 3\n12345678901234567890,second field value,third field value")]
    public void TestSchemaValidationForStringFields_Fail_MaxLength_Single(
        int expectedDataRows, 
        int expectedFieldCount,
        int expectedRowErrorPosition,
        int expectedFieldErrorPosition,
        int expectedErrorCode,
        string csvContent)
    {
        /*
         * Tests the CSV schema-based validation
         * For CSV column type = string
         * To ensure values exceeding the max length property are properly error'd
         * When there is just one max length violation in the CSV file
         */
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
    
    [Theory]
    [InlineData(10, "STR COL 1,STR COL 2,STR COL 3\nfirst field valu,second field value,third field value\nfirst field valu,second field value,third field value")]
    [InlineData(10, "STR COL 1,STR COL 2,STR COL 3\nfirst field value,second field value,third field value\nfirst field value,second field value,third field value")]
    [InlineData(10, "STR COL 1,STR COL 2,STR COL 3\n1234567890123456,second field value,third field value\n1234567890123456,second field value,third field value")]
    [InlineData(10, "STR COL 1,STR COL 2,STR COL 3\n                ,second field value,third field value\n                ,second field value,third field value")]
    [InlineData(10, "STR COL 1,STR COL 2,STR COL 3\n12345678901234567890,second field value,third field value\n12345678901234567890,second field value,third field value")]
    public void TestSchemaValidationForStringFields_Fail_MaxLength_Multiple(
        int expectedErrorCode,
        string csvContent)
    {
        /*
         * Tests the CSV schema-based validation
         * For CSV column type = string
         * To ensure values exceeding the max length property are properly error'd
         * When there are more than one max length violations in the CSV file
         */
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
        foreach (var message in result.Messages)
        {
            Assert.Equal(expectedErrorCode, message.Code);
        }

        Assert.False(result.IsValid);
    }
    
    [Theory]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\nfirst fie,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\nfirst fi,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n123456789,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n         ,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n12345678,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n1234567,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n123456,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n12345,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n1234,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n123,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n12,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n1,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\n,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\nfirst fie,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\nfirst fi,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n123456789,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n         ,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n12345678,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n1234567,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n123456,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n12345,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n1234,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n123,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n12,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n1,second field value,third field value")]
    [InlineData(1, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n,second field value,third field value")]
    [InlineData(2, 3, 2, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n1234567890,second field value,third field value\r\n123456789,second field value,third field value")]
    [InlineData(3, 3, 3, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n1234567890,second field value,third field value\r\n1234567890,second field value,third field value\r\n123456789,second field value,third field value")]
    [InlineData(3, 3, 1, 1, 11, "STR COL 1,STR COL 2,STR COL 3\r\n,second field value,third field value\r\n1234567890,second field value,third field value\r\n1234567890,second field value,third field value")]
    public void TestSchemaValidationForStringFields_Fail_MinLength_Single(
        int expectedDataRows, 
        int expectedFieldCount,
        int expectedRowErrorPosition,
        int expectedFieldErrorPosition,
        int expectedErrorCode,
        string csvContent)
    {
        /*
         * Tests the CSV schema-based validation
         * For CSV column type = string
         * To ensure values below the min length property are properly error'd
         * When there is just one min length violation in the CSV file
         */
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
    
    [Theory]
    [InlineData(11, "STR COL 1,STR COL 2,STR COL 3\nfirst fie,second field value,third field value\nfirst fie,second field value,third field value")]
    [InlineData(11, "STR COL 1,STR COL 2,STR COL 3\n,second field value,third field value\n,second field value,third field value")]
    public void TestSchemaValidationForStringFields_Fail_MinLength_Multiple(
        int expectedErrorCode,
        string csvContent)
    {
        /*
         * Tests the CSV schema-based validation
         * For CSV column type = string
         * To ensure values below the min length property are properly error'd
         * When there are more than one min length violations in the CSV file
         */
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
        foreach (var message in result.Messages)
        {
            Assert.Equal(expectedErrorCode, message.Code);
        }

        Assert.False(result.IsValid);
    }
    
    [Theory]
    [InlineData("STR COL 1,STR COL 2\r\n555-555-1234,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData("STR COL 1,STR COL 2\r\n555-555-0000,01/02/1991\r\n555-555-2345,12/12/1968")]
    public void TestSchemaValidationForStringFields_Success_Format(string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson06
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.True(result.IsValid);
    }
    
    [Theory]
    [InlineData(15, "STR COL 1,STR COL 2\r\n555-555-123,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData(18, "STR COL 1,STR COL 2\r\n555_555_1234,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData(17, "STR COL 1,STR COL 2\r\n555-555-123A,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData(17, "STR COL 1,STR COL 2\r\n555-555-A234,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData(17, "STR COL 1,STR COL 2\r\n555-555-$234,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData(17, "STR COL 1,STR COL 2\r\n555-555-#234,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData(17, "STR COL 1,STR COL 2\r\nA55-555-1234,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData(17, "STR COL 1,STR COL 2\r\n$55-555-1234,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData(17, "STR COL 1,STR COL 2\r\n#55-555-1234,12/12/2004\r\n555-555-2345,12/12/1972")]
    [InlineData(16, "STR COL 1,STR COL 2\r\n555-555-0000,01/02/199\r\n555-555-2345,12/12/1968")]
    [InlineData(16, "STR COL 1,STR COL 2\r\n555-555-0000,01/02/19\r\n555-555-2345,12/12/1968")]
    [InlineData(16, "STR COL 1,STR COL 2\r\n555-555-0000,1/2/1991\r\n555-555-2345,12/12/1968")]
    [InlineData(16, "STR COL 1,STR COL 2\r\n555-555-0000,12/2/1991\r\n555-555-2345,12/12/1968")]
    [InlineData(16, "STR COL 1,STR COL 2\r\n555-555-0000,1/12/1991\r\n555-555-2345,12/12/1968")]
    public void TestSchemaValidationForStringFields_Fail_Format(int expectedErrorCode, string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true,
            Quote = '\"',
            ValidationProfile = ProfileJson06
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.Equal(expectedErrorCode, result.Messages[0].Code);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
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