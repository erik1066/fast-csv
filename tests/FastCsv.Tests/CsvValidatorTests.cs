using System.Diagnostics;
using FastCsv;

namespace FastCsv.Tests;

public class CsvValidatorTests
{
    [Theory]
    [InlineData(true, "NAME,AGE,DOB\nJohn,23,1/1/2000\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\rJohn,23,1/1/2000\rMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000\r\n", 2, 3)]
    [InlineData(true, "\"First,Last Names\",AGE,DOB\r\nJohn Doe,23,1/1/2000\r\nMary Sue,34,1/1/2000", 2, 3)]
    [InlineData(false, "NAME,AGE,DOB\n\rJohn,23,1/1/2000\n\rMary,34,1/1/2000", 4, 3)]
    public void TestLineEndings(bool isValid, string csvContent, int actualDataRows, int actualFieldCount)
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
        Assert.Equal(actualDataRows, result.DataRowCount);
        Assert.Equal(actualFieldCount, result.FieldCount);
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
    public void TestSeparatorsInQuotedFields(bool isValid, string csvContent, int actualDataRows, int actualFieldCount)
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
        Assert.Equal(actualDataRows, result.DataRowCount);
        Assert.Equal(actualFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData(true, "\"NAME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"FIRST NAME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"LAST,FIRST NAME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"LAST,FIRST NAME\",AGE,DOB\r\n\"Doe, John\",23,1/1/2000\r\n\"Sue, Mary\",34,1/1/2000", 2, 3)]
    [InlineData(true, "\"FULL NAME\",AGE,DOB\r\n\"John Doe\",23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "\"FULL NAME\",\"AGE\",\"DOB\"\r\n\"John Doe\",\"23\",\"1/1/2000\"\r\n\"Mary Sue\",\"34\",\"1/1/2000\"", 2, 3)]
    public void TestQuotedFields(bool isValid, string csvContent, int actualDataRows, int actualFieldCount)
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
        Assert.Equal(actualDataRows, result.DataRowCount);
        Assert.Equal(actualFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData(true, "\"N\"\"AME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(false, "\"N\"\"A\"ME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(false, "\"NA\"\"\"ME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(false, "NA\"ME,AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(false, "\"NA\"ME\",AGE,DOB\r\nJohn,23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    public void TestQuotesInQuotedFields(bool isValid, string csvContent, int actualDataRows, int actualFieldCount)
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
        Assert.Equal(actualDataRows, result.DataRowCount);
        Assert.Equal(actualFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
    }
    
    [Theory]
    [InlineData(true, "NAME,AGE,DOB\r\n\"John\r\nDoe\",23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    [InlineData(true, "NAME,AGE,DOB\n\"John\r\nDoe\",23,1/1/2000\r\nMary,34,1/1/2000", 2, 3)]
    public void TestLineBreaksInQuotedFields(bool isValid, string csvContent, int actualDataRows, int actualFieldCount)
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
        Assert.Equal(actualDataRows, result.DataRowCount);
        Assert.Equal(actualFieldCount, result.FieldCount);
        Assert.Equal(isValid, result.IsValid);
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