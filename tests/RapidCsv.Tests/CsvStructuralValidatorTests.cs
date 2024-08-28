using System.Diagnostics;
using RapidCsv;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

namespace RapidCsv.Tests;

public class CsvStructuralValidatorTests
{
    [Theory]
    [InlineData(true, "NAME,AGE,DOB\nJohn,23,1/1/2000\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\rJohn,23,1/1/2000\rMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000\r\n", 2, 3)]
    [InlineData(true, "\"First,Last Names\",AGE,DOB\r\nJohn Doe,23,1/1/2000\r\nMary Sue,34,1/1/2000", 2, 3)]
    [InlineData(false, "NAME,AGE,DOB\n\rJohn,23,1/1/2000\n\rMary,34,1/1/2000", 4, 3)]
    public void TestLineEndings(bool isValid, string csvContent, int expectedDataRows, int expectedFieldCount)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(expectedDataRows, result.DataRowCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData(true, "NAME,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"First,Last Names\",AGE,DOB\r\nJohn Doe,23,1/1/2000\r\nMary Sue,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"First,Last,Names\",AGE,DOB\r\nJohn Doe,23,1/1/2000\r\nMary Sue,34,1/1/2000", 2, 3)]
    [InlineData(true, "\",,,,,\",AGE,DOB\r\nJohn Doe,23,1/1/2000\r\nMary Sue,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\r\n\"Doe,John\",23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\r\n\"Doe,John\",23,1/1/2000\r\n\"Sue,Mary\",34,1/1/2000", 2, 3)]
    [InlineData(true, "\"First,Last,Names\",AGE,DOB\r\n\"Doe,John\",23,1/1/2000\r\n\"Sue,Mary\",34,1/1/2000", 2, 3)]
    public void TestSeparatorsInQuotedFields(bool isValid, string csvContent, int expectedDataRows, int expectedFieldCount)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(expectedDataRows, result.DataRowCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData(true, "\"NAME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"FIRST NAME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"LAST,FIRST NAME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"LAST,FIRST NAME\",AGE,DOB\r\n\"Doe, John\",23,1/1/2000\r\n\"Sue, Mary\",34,1/1/2000", 2, 3)]
    [InlineData(true, "\"FULL NAME\",AGE,DOB\r\n\"John Doe\",23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"FULL NAME\",\"AGE\",\"DOB\"\r\n\"John Doe\",\"23\",\"1/1/2000\"\r\n\"Mary Sue\",\"34\",\"1/1/2000\"", 2, 3)]
    [InlineData(true, "ʺNAMEʺ,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "ˮNAMEˮ,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "˵NAME˵,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "˶NAME˶,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "˝NAME˝,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    public void TestQuotedFields(bool isValid, string csvContent, int expectedDataRows, int expectedFieldCount)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(expectedDataRows, result.DataRowCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData(true, "\"N\"\"AME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(false, "\"N\"\"A\"ME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(false, "\"NA\"\"\"ME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(false, "NA\"ME,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(false, "\"NA\"ME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    public void TestQuotesInQuotedFields(bool isValid, string csvContent, int expectedDataRows, int expectedFieldCount)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(expectedDataRows, result.DataRowCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData(true, "NAME,AGE,DOB\r\n\"John\r\nDoe\",23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\n\"John\r\nDoe\",23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    public void TestLineBreaksInQuotedFields(bool isValid, string csvContent, int expectedDataRows, int expectedFieldCount)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(expectedDataRows, result.DataRowCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData(true, "NAME,AGE,DOB", 3, 0, "NAME", 1, "AGE", 2, "DOB")]
    [InlineData(true, "NAME,AGE,DOB\n", 3, 0, "NAME", 1, "AGE", 2, "DOB")]
    [InlineData(true, "NAME,AGE,DOB\r\n", 3, 0, "NAME", 1, "AGE", 2, "DOB")]
    [InlineData(true, "NAME,AGE,DOB\nJohn,23,1/1/2000\nMary,34,1/1/2000", 3, 0, "NAME", 1, "AGE", 2, "DOB")]
    [InlineData(true, "NAME,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 3, 0, "NAME", 1, "AGE", 2, "DOB")]
    [InlineData(true, "\"NAME\",AGE,DOB\nJohn,23,1/1/2000\nMary,34,1/1/2000", 3, 0, "\"NAME\"", 1, "AGE", 2, "DOB")]
    [InlineData(true, "\"NAME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 3, 0, "\"NAME\"", 1, "AGE", 2, "DOB")]
    [InlineData(true, "\"FIRST NAME\",AGE,DOB\nJohn,23,1/1/2000\nMary,34,1/1/2000", 3, 0, "\"FIRST NAME\"", 1, "AGE", 2, "DOB")]
    [InlineData(true, "\"FIRST NAME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 3, 0, "\"FIRST NAME\"", 1, "AGE", 2, "DOB")]
    [InlineData(true, "\"LAST,FIRST NAME\",AGE,DOB\nJohn,23,1/1/2000\nMary,34,1/1/2000", 3, 0, "\"LAST,FIRST NAME\"", 1, "AGE", 2, "DOB")]
    [InlineData(true, "\"LAST, FIRST NAME\",AGE,DOB\nJohn,23,1/1/2000\nMary,34,1/1/2000", 3, 0, "\"LAST, FIRST NAME\"", 1, "AGE", 2, "DOB")]
    [InlineData(true, ",AGE,DOB\nJohn,23,1/1/2000\nMary,34,1/1/2000", 3, 0, "", 1, "AGE", 2, "DOB")]
    [InlineData(true, ",,DOB\nJohn,23,1/1/2000\nMary,34,1/1/2000", 3, 0, "", 1, "", 2, "DOB")]
    [InlineData(true, ",,\nJohn,23,1/1/2000\nMary,34,1/1/2000", 3, 0, "", 1, "", 2, "")]
    [InlineData(true, ",,\nJohn,23,1/1/2000", 3, 0, "", 1, "", 2, "")]
    [InlineData(true, ",,\r\nJohn,23,1/1/2000", 3, 0, "", 1, "", 2, "")]
    [InlineData(true, ",,\r\n", 3, 0, "", 1, "", 2, "")]
    [InlineData(true, ",,\n", 3, 0, "", 1, "", 2, "")]
    [InlineData(true, ",,", 3, 0, "", 1, "", 2, "")]
    [InlineData(true, ",", 2, 0, "", 1, "", 2, "")]
    [InlineData(true, "\r\nMARY\r\nSUSAN", 1, 0, "", 1, "", 2, "")]
    [InlineData(true, " \r\nMARY\r\nSUSAN", 1, 0, " ", 1, "", 2, "")]
    [InlineData(true, "  \r\nMARY\r\nSUSAN", 1, 0, "  ", 1, "", 2, "")]
    [InlineData(true, " -- \r\nMARY\r\nSUSAN", 1, 0, " -- ", 1, "", 2, "")]
    public void TestHeaderNameDetection(bool isValid, string csvContent, int totalHeaderCount, int header1Position, string header1, int header2Position, string header2, int header3Position, string header3)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        if (totalHeaderCount - 1 >= header1Position)
        {
            Assert.Equal(header1, result.Headers[header1Position]);
        }

        if (totalHeaderCount - 1 >= header2Position)
        {
            Assert.Equal(header2, result.Headers[header2Position]);
        }

        if (totalHeaderCount - 1 >= header3Position)
        {
            Assert.Equal(header3, result.Headers[header3Position]);
        }

        Assert.Equal(totalHeaderCount, result.Headers.Count);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData(1, "\r\nMARY\r\nSUSAN")]
    [InlineData(2, "NAME,,DOB\r\nMARY,23,1/1/2000")]
    [InlineData(3, "NAME,AGE,\r\nMARY,23,1/1/2000")]
    public void TestHeaderNameDetectionWithWarnings(int problemFieldNumber, string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(0, result.ErrorCount);
        Assert.Equal(1, result.WarningCount);
        Assert.Single(result.Messages);
        Assert.Equal(ValidationMessageType.Structural, result.Messages[0].MessageType);
        Assert.Equal(1, result.Messages[0].Row);
        Assert.Equal(problemFieldNumber, result.Messages[0].FieldNumber);
        Assert.Equal(4, result.Messages[0].Code);
        Assert.True(result.IsValid);
    }
    
    [Theory]
    [InlineData(1, " \r\nMARY")]
    [InlineData(1, "  \r\nMARY")]
    [InlineData(1, "   \r\nMARY")]
    [InlineData(2, "NAME, ,DOB\r\nMARY,23,1/1/2000")]
    [InlineData(2, "NAME,  ,DOB\r\nMARY,23,1/1/2000")]
    [InlineData(3, "NAME,AGE, \r\nMARY,23,1/1/2000")]
    [InlineData(3, "NAME,AGE,  \r\nMARY,23,1/1/2000")]
    public void TestHeaderNameDetectionWithInformation(int problemFieldNumber, string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = ',',
            HasHeaderRow = true
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(0, result.ErrorCount);
        Assert.Equal(0, result.WarningCount);
        Assert.Single(result.Messages);
        Assert.Equal(ValidationMessageType.Structural, result.Messages[0].MessageType);
        Assert.Equal(Severity.Information, result.Messages[0].Severity);
        Assert.Equal(1, result.Messages[0].Row);
        Assert.Equal(problemFieldNumber, result.Messages[0].FieldNumber);
        Assert.Equal(5, result.Messages[0].Code);
        Assert.True(result.IsValid);
        Assert.Equal(1, result.DataRowCount);
    }
    
    [Theory]
    [InlineData(',', 1, "NAME\r\nMARY")]
    [InlineData(',', 2, "NAME,AGE\r\nMARY,25")]
    [InlineData(';', 2, "NAME;AGE\r\nMARY;25")]
    [InlineData('|', 2, "NAME|AGE\r\nMARY|25")]
    [InlineData('/', 2, "NAME/AGE\r\nMARY/25")]
    [InlineData('-', 2, "NAME-AGE\r\nMARY-25")]
    [InlineData(',', 2, "\"FIRST NAME, LAST NAME\",AGE\r\n\"MARY SUAREZ\",25")]
    [InlineData(';', 2, "\"FIRST NAME, LAST NAME\";AGE\r\n\"MARY SUAREZ\";25")]
    [InlineData(';', 2, "\"FIRST NAME; LAST NAME\";AGE\r\n\"MARY SUAREZ\";25")]
    [InlineData(',', 2, "\"LAST NAME, FIRST NAME\",AGE\r\n\"SUAREZ, MARY\",25")]
    [InlineData(';', 2, "\"LAST NAME, FIRST NAME\";AGE\r\n\"SUAREZ, MARY\";25")]
    [InlineData(';', 2, "\"LAST NAME; FIRST NAME\";AGE\r\n\"SUAREZ; MARY\";25")]
    [InlineData('\t', 2, "NAME\tAGE\r\nMARY\t25")]
    [InlineData(';', 3, "NAME;AGE;ADDRESS\r\nMARY;25;\"123 Main Street Anytown, NY 12345\"")]
    [InlineData(',', 3, "NAME,AGE,ADDRESS\r\nMARY,25,\"123 Main Street Anytown, NY 12345\"")]
    [InlineData('\t', 3, "NAME\tAGE\tADDRESS\r\nMARY\t25\t\"123 Main Street Anytown, NY 12345\"")]
    [InlineData('\t', 3, "NAME\tAGE\tADDRESS\r\nMARY\t25\t\"123 Main Street\r\nAnytown, NY 12345\"")]
    public void TestSeparatorWithHeaderRow(char separator, int expectedFieldCount, string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = separator,
            HasHeaderRow = true
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(0, result.ErrorCount);
        Assert.Equal(0, result.WarningCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.Empty(result.Messages);
        Assert.True(result.IsValid);
        Assert.Equal(1, result.DataRowCount);
    }
    
    [Theory]
    [InlineData(',', 1, 1, "MARY")]
    [InlineData(',', 2, 1, "MARY\r\nJOHN")]
    [InlineData(',', 3, 1, "MARY\r\nJANA\r\nHANA")]
    [InlineData(',', 1, 2, "MARY,25")]
    [InlineData(',', 2, 2, "MARY,25\r\nJOHN,25")]
    [InlineData(',', 3, 2, "MARY,25\r\nJANA,25\r\nHANA,25")]
    [InlineData(';', 1, 2, "MARY;25")]
    [InlineData(';', 2, 2, "MARY;25\r\nJOSE;35")]
    [InlineData(';', 3, 2, "MARY;25\r\nJOSE;35\r\nJUAN;45")]
    [InlineData('|', 1, 2, "MARY|25")]
    [InlineData('|', 2, 2, "MARY|25\r\nSUSAN|35")]
    [InlineData('|', 3, 2, "MARY|25\r\nSUSAN|35\r\nKAREN|45")]
    [InlineData('/', 1, 2, "MARY/25")]
    [InlineData('-', 1, 2, "MARY-25")]
    [InlineData(',', 1, 2, "\"MARY SUAREZ\",25")]
    [InlineData(',', 2, 2, "\"MARY SUAREZ\",25\r\n\"JOSÉ MARTINEZ\",35")]
    [InlineData(';', 1, 2, "\"MARY SUAREZ\";25")]
    [InlineData(';', 2, 2, "\"MARY SUAREZ\";25\r\n\"JOSÉ MARTINEZ\";35")]
    [InlineData(',', 1, 2, "\"SUAREZ, MARY\",25")]
    [InlineData(';', 1, 2, "\"SUAREZ, MARY\";25")]
    [InlineData(';', 1, 2, "\"SUAREZ; MARY\";25")]
    [InlineData('\t', 1, 1, "MARY")]
    [InlineData('\t', 1, 2, "MARY\t25")]
    [InlineData(';', 1, 3, "MARY;25;\"123 Main Street Anytown, NY 12345\"")]
    [InlineData(',', 1, 3, "MARY,25,\"123 Main Street Anytown, NY 12345\"")]
    [InlineData('\t', 1, 3, "MARY\t25\t\"123 Main Street Anytown, NY 12345\"")]
    [InlineData('\t', 1, 3, "MARY\t25\t\"123 Main Street\r\nAnytown, NY 12345\"")]
    public void TestSeparatorWithoutHeaderRow(char separator, int expectedRowCount, int expectedFieldCount, string csvContent)
    {
        CsvValidator validator = new CsvValidator();
        var options = new ValidationOptions()
        {
            Separator = separator,
            HasHeaderRow = false
        };

        Stream content = GenerateStreamFromString(csvContent);
        ValidationResult result = validator.Validate(content: content, options: options);
        
        Assert.True(result.ElapsedMilliseconds >= 0.0);
        Assert.Equal(0, result.ErrorCount);
        Assert.Equal(0, result.WarningCount);
        Assert.Equal(expectedFieldCount, result.FieldCount);
        Assert.Empty(result.Messages);
        Assert.True(result.IsValid);
        Assert.Equal(expectedRowCount, result.DataRowCount);
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