using System.Text.Json;

namespace RapidCsv.Tests;

public class CsvFieldValidator_String_Format_Tests
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
            ""max"": 45,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": ""###-###-####"",
            ""regex"": null
        },
        {
            ""name"": ""STR COL 2"",
            ""description"": ""String type, tests for FORMAT"",
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
    [InlineData(1, 1, "STR COL 1", "###-###-####", "555-555-5555")]
    [InlineData(1, 1, "STR COL 1", "###-##-####", "555-55-5555")]
    [InlineData(1, 1, "STR COL 1", "#", "5")]
    [InlineData(1, 1, "STR COL 1", "##", "00")]
    [InlineData(1, 1, "STR COL 1", "##", "99")]
    [InlineData(1, 1, "STR COL 1", "###", "990")]
    [InlineData(1, 1, "STR COL 1", "###-", "990-")]
    [InlineData(1, 1, "STR COL 1", "-", "-")]
    [InlineData(1, 1, "STR COL 1", "A-", "A-")]
    [InlineData(1, 1, "STR COL 1", "A-B", "A-B")]
    [InlineData(1, 1, "STR COL 1", "---", "---")]
    [InlineData(1, 1, "STR COL 1", "TIM", "TIM")]
    [InlineData(1, 1, "STR COL 1", "tim", "tim")]
    [InlineData(1, 1, "STR COL 2", "###-###-####", "555-555-5555")]
    [InlineData(1, 1, "STR COL 2", "###-##-####", "555-55-5555")]
    [InlineData(1, 1, "STR COL 2", "#", "5")]
    [InlineData(1, 1, "STR COL 2", "##", "00")]
    [InlineData(1, 1, "STR COL 2", "##", "99")]
    [InlineData(1, 1, "STR COL 2", "###", "990")]
    [InlineData(1, 1, "STR COL 2", "###-", "990-")]
    [InlineData(1, 1, "STR COL 2", "-", "-")]
    [InlineData(1, 1, "STR COL 2", "A-", "A-")]
    [InlineData(1, 1, "STR COL 2", "A-B", "A-B")]
    [InlineData(1, 1, "STR COL 2", "---", "---")]
    [InlineData(1, 1, "STR COL 2", "TIM", "TIM")]
    [InlineData(1, 1, "STR COL 2", "tim", "tim")]
    public void TestValidateStringField_NumberFormat_Success(int rowNum, int colPos, string colName, string format, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        var columnProfile = csvValidationProfile.Columns[colPos - 1];
        columnProfile.Format = format;
        
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Empty(messages);
    }
    
    [Theory]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555-555-55555")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555-555-555")]
    [InlineData(17, 1, 1, "STR COL 1", "###-###-####", "###-###-####")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555-555-55")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555-555-5")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555-555-")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555-555")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555-55")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555-5")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555-")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "555")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "55")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "5")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "")]
    [InlineData(17, 1, 1, "STR COL 1", "###-###-####", "555-555-555A")]
    [InlineData(17, 1, 1, "STR COL 1", "###-###-####", "A55-555-5555")]
    [InlineData(18, 1, 1, "STR COL 1", "###-###-####", "555_555-5555")]
    [InlineData(18, 1, 1, "STR COL 1", "###-###-####", "555A555-5555")]
    [InlineData(18, 1, 1, "STR COL 1", "###-###-####", "555/555-5555")]
    [InlineData(18, 1, 1, "STR COL 1", "###-###-####", "555$555-5555")]
    [InlineData(15, 1, 1, "STR COL 1", "###-###-####", "\r\n")]
    [InlineData(15, 1, 1, "STR COL 1", "#", "555-555-55555")]
    [InlineData(15, 1, 1, "STR COL 1", "#", "55")]
    [InlineData(17, 1, 1, "STR COL 1", "#", "A")]
    [InlineData(17, 1, 1, "STR COL 1", "#", "-")]
    [InlineData(17, 1, 1, "STR COL 1", "#", "#")]
    [InlineData(17, 1, 1, "STR COL 1", "##", "##")]
    [InlineData(17, 1, 1, "STR COL 1", "##", "EE")]
    [InlineData(17, 1, 1, "STR COL 1", "##", "E5")]
    [InlineData(17, 1, 1, "STR COL 1", "##", "5E")]
    [InlineData(18, 1, 1, "STR COL 1", "TIM", "tim")]
    [InlineData(18, 1, 1, "STR COL 1", "tim", "TIM")]
    [InlineData(15, 1, 1, "STR COL 1", "---", "--")]
    [InlineData(15, 1, 1, "STR COL 1", "---", "----")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555-555-55555")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555-555-555")]
    [InlineData(17, 1, 1, "STR COL 2", "###-###-####", "###-###-####")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555-555-55")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555-555-5")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555-555-")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555-555")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555-55")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555-5")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555-")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "555")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "55")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "5")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "")]
    [InlineData(17, 1, 1, "STR COL 2", "###-###-####", "555-555-555A")]
    [InlineData(17, 1, 1, "STR COL 2", "###-###-####", "A55-555-5555")]
    [InlineData(18, 1, 1, "STR COL 2", "###-###-####", "555_555-5555")]
    [InlineData(18, 1, 1, "STR COL 2", "###-###-####", "555A555-5555")]
    [InlineData(18, 1, 1, "STR COL 2", "###-###-####", "555/555-5555")]
    [InlineData(18, 1, 1, "STR COL 2", "###-###-####", "555$555-5555")]
    [InlineData(15, 1, 1, "STR COL 2", "###-###-####", "\r\n")]
    [InlineData(15, 1, 1, "STR COL 2", "#", "555-555-55555")]
    [InlineData(15, 1, 1, "STR COL 2", "#", "55")]
    [InlineData(17, 1, 1, "STR COL 2", "#", "A")]
    [InlineData(17, 1, 1, "STR COL 2", "#", "-")]
    [InlineData(17, 1, 1, "STR COL 2", "#", "#")]
    [InlineData(17, 1, 1, "STR COL 2", "##", "##")]
    [InlineData(17, 1, 1, "STR COL 2", "##", "EE")]
    [InlineData(17, 1, 1, "STR COL 2", "##", "E5")]
    [InlineData(17, 1, 1, "STR COL 2", "##", "5E")]
    [InlineData(18, 1, 1, "STR COL 2", "TIM", "tim")]
    [InlineData(18, 1, 1, "STR COL 2", "tim", "TIM")]
    [InlineData(15, 1, 1, "STR COL 2", "---", "--")]
    [InlineData(15, 1, 1, "STR COL 2", "---", "----")]
    public void TestValidateStringField_NumberFormat_Fail(int expectedCode, int rowNum, int colPos, string colName, string format, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        var columnProfile = csvValidationProfile.Columns[colPos - 1];
        columnProfile.Format = format;
        
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Single(messages);
        Assert.Equal(expectedCode, messages[0].Code);
    }
    
    [Theory]
    [InlineData(1, 1, "STR COL 1", "mm/dd/yyyy", "01/01/2000")]
    [InlineData(1, 1, "STR COL 1", "mm/dd/yyyy", "12/12/2000")]
    [InlineData(1, 1, "STR COL 1", "mm/dd/yyyy", "13/12/2000")] // Yes, this is correct C# behavior... the '13' gets turned to a 1
    [InlineData(1, 1, "STR COL 1", "m/d/yyyy", "01/01/2000")]
    [InlineData(1, 1, "STR COL 1", "m/d/yyyy", "1/1/2000")]
    [InlineData(1, 1, "STR COL 1", "m/d/yyyy", "1/31/2000")]
    [InlineData(1, 1, "STR COL 1", "m/d/yyyy", "12/12/2000")]
    [InlineData(1, 1, "STR COL 1", "m/d/yyyy", "12/25/2000")]
    [InlineData(1, 1, "STR COL 1", "M/d/yyyy hh:mm", "5/3/2009 08:22")]
    [InlineData(1, 1, "STR COL 1", "M/dd/yyyy hh:mm", "5/22/2009 08:22")]
    [InlineData(1, 1, "STR COL 1", "MM/dd/yyyy hh:mm", "05/22/2009 08:22")]
    [InlineData(1, 1, "STR COL 1", "M/d/yyyy hh:mm:ss", "5/3/2009 08:22:45")]
    [InlineData(1, 1, "STR COL 1", "M/dd/yyyy hh:mm:ss", "5/22/2009 08:22:45")]
    [InlineData(1, 1, "STR COL 1", "MM/dd/yyyy hh:mm:ss", "05/22/2009 08:22:45")]
    [InlineData(1, 1, "STR COL 1", "M/d/yyyy hh:mm:ss tt", "5/3/2009 08:22:45 AM")]
    [InlineData(1, 1, "STR COL 1", "M/dd/yyyy hh:mm:ss tt", "5/22/2009 08:22:45 AM")]
    [InlineData(1, 1, "STR COL 1", "MM/dd/yyyy hh:mm:ss tt", "05/22/2009 08:22:45 AM")]
    [InlineData(1, 1, "STR COL 1", "M/d/yyyy hh:mm:ss tt", "5/3/2009 08:22:45 PM")]
    [InlineData(1, 1, "STR COL 1", "M/dd/yyyy hh:mm:ss tt", "5/22/2009 08:22:45 PM")]
    [InlineData(1, 1, "STR COL 1", "MM/dd/yyyy hh:mm:ss tt", "05/22/2009 08:22:45 PM")]
    [InlineData(1, 1, "STR COL 1", "dd/mm/yyyy", "01/01/2000")]
    [InlineData(1, 1, "STR COL 1", "dd/mm/yyyy", "12/12/2000")]
    [InlineData(1, 1, "STR COL 1", "d/m/yyyy", "01/01/2000")]
    [InlineData(1, 1, "STR COL 1", "d/m/yyyy", "1/1/2000")]
    [InlineData(1, 1, "STR COL 1", "d/m/yyyy", "31/1/2000")]
    [InlineData(1, 1, "STR COL 1", "d/m/yyyy", "12/12/2000")]
    [InlineData(1, 1, "STR COL 1", "d/m/yyyy", "12/25/2000")]
    [InlineData(1, 1, "STR COL 1", "d/M/yyyy hh:mm", "3/5/2009 08:22")]
    [InlineData(1, 1, "STR COL 1", "dd/M/yyyy hh:mm", "22/5/2009 08:22")]
    [InlineData(1, 1, "STR COL 1", "dd/MM/yyyy hh:mm", "22/05/2009 08:22")]
    [InlineData(1, 1, "STR COL 1", "d/M/yyyy hh:mm:ss", "25/5/2009 08:22:45")]
    [InlineData(1, 1, "STR COL 1", "yyyy-mm-dd", "2020-03-25")]
    [InlineData(1, 1, "STR COL 1", "yyyy-mm-dd", "2020-12-25")]
    [InlineData(1, 1, "STR COL 1", "yyyy-mm-dd", "2020-01-01")]
    [InlineData(1, 1, "STR COL 1", "yyyy/mm/dd", "2020/01/01")]
    [InlineData(1, 1, "STR COL 1", "yyyy/MM/dd hh:mm:ss", "2020/01/01 02:00:00")]
    [InlineData(1, 1, "STR COL 1", "MMM dd yyyy", "Jan 25 2004")]
    [InlineData(1, 1, "STR COL 1", "MMM dd yyyy", "Jan 05 2004")]
    [InlineData(1, 1, "STR COL 1", "dd MMM yyyy", "17 Jul 1973")]
    [InlineData(1, 1, "STR COL 1", "dd MMM yyyy", "07 Jul 1973")]
    [InlineData(1, 1, "STR COL 1", "MMM d yyyy", "Jan 25 2004")]
    [InlineData(1, 1, "STR COL 1", "MMM d yyyy", "Jan 5 2004")]
    [InlineData(1, 1, "STR COL 1", "d MMM yyyy", "17 Jul 1981")]
    [InlineData(1, 1, "STR COL 1", "d MMM yyyy", "7 Jul 1981")]
    [InlineData(1, 1, "STR COL 1", "d MMMM yyyy", "31 July 1992")]
    [InlineData(1, 1, "STR COL 1", "d MMMM yyyy", "3 July 1992")]
    public void TestValidateStringField_DateFormat_Success(int rowNum, int colPos, string colName, string format, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        var columnProfile = csvValidationProfile.Columns[colPos - 1];
        columnProfile.Format = format;
        
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Empty(messages);
    }
    
    [Theory]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "A1/01/1980")]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "01/A1/1980")]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "01-01-1980")]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "01.01.1980")]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "01A01A1980")]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "01|01|1980")]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "01/01/19801")]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "01/01/198")]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "001/01/1988")]
    [InlineData(16, 1, 1, "STR COL 1", "mm/dd/yyyy", "01/001/1988")]
    public void TestValidateStringField_DateFormat_Fail(int expectedCode, int rowNum, int colPos, string colName, string format, string fieldContent)
    {
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            DataRowCount = 1,
            IsHeaderRow = false,
        };

        rowOptions.AddHeader(colName, colPos);
        
        var csvValidationProfile = JsonSerializer.Deserialize<ValidationProfile>(ProfileJson01) ?? new ValidationProfile();
        var columnProfile = csvValidationProfile.Columns[colPos - 1];
        columnProfile.Format = format;
        
        CsvFieldValidator fieldValidator = new CsvFieldValidator(csvValidationProfile, rowOptions);
        
        List<ValidationMessage> messages = fieldValidator.ValidateField(fieldContent.AsSpan(), rowNum, colPos);
        
        Assert.Single(messages);
        Assert.Equal(expectedCode, messages[0].Code);
    }
}