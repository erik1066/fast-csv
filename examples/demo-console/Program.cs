using RapidCsv;

string csvContent = @"NAME,AGE,DOB
John,23,1/1/2012
Mary,34,1/1/1990
Jane,25,1/1/2010
Hana,55,1/1/1970";

CsvValidator validator = new CsvValidator();
var options = new ValidationOptions()
{
    Separator = ',',
    HasHeaderRow = true
};

Stream content = GenerateStreamFromString(csvContent);
ValidationResult result = validator.Validate(content: content, options: options);

Console.WriteLine($"Valid File = {result.IsValid}");
Console.WriteLine($" Data Rows         = {result.DataRowCount}");
Console.WriteLine($" Elapsed time (ms) = {result.ElapsedMilliseconds.ToString("N0")}ms");
Console.WriteLine($" Columns           = {result.FieldCount}");
Console.WriteLine($" Error count       = {result.ErrorCount}");
Console.WriteLine($" Warning count     = {result.WarningCount}");

if (result.Headers.Count != 0)
{
    Console.WriteLine(" Headers = ");
    for (int i = 0; i < result.Headers.Count; i++)
    {
        Console.WriteLine($"  Column {i + 1} = {result.Headers[i]}");
    }
}

if (result.Messages.Count != 0)
{
    Console.WriteLine(" Validation Messages = ");
    for (int i = 0; i < result.Messages.Count; i++)
    {
        var msg = result.Messages[i];
        Console.WriteLine($"  Message {i + 1} = ");
        Console.WriteLine($"   Severity   : {msg.Severity}");
        Console.WriteLine($"   Code       : {msg.Code}");
        Console.WriteLine($"   Row        : {msg.Row}");
        Console.WriteLine($"   Character  : {msg.Character}");
        Console.WriteLine($"   Field Num  : {msg.FieldNumber}");
        Console.WriteLine($"   Field Name : {msg.FieldName}");
        Console.WriteLine($"   Err Content: {msg.Content}");
    }
}

static Stream GenerateStreamFromString(string s)
{
    var stream = new MemoryStream();
    var writer = new StreamWriter(stream);
    writer.Write(s);
    writer.Flush();
    stream.Position = 0;
    return stream;
}
