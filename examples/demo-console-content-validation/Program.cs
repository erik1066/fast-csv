using RapidCsv;

string csvContent = @"NAME,AGE,DOB,PHONE,STATUS
John,23,1/1/2012,555-555-5555,actv
Mary,34,1/1/1990,555-555-5555,inac
Jane,25,1/1/2010,555-555-5555,actv
Hana,55,1/1/1970,555-555-555X,unkn";

string validationProfile = @"{
    ""$schema"": ""rapid-csv/validator-config-schema.json"",
    ""name"": ""Acme Bookstore Customer Records"",
    ""description"": ""Validation profile for the CSV records of our Acme bookstore customers"",
    ""filename"": ""abc123.csv"",
    ""separator"": "","",
    ""has_header"": true,
    ""columns"": [
        {
            ""name"": ""NAME"",
            ""description"": ""The customer's name"",
            ""ordinal"": 1,
            ""type"": ""string"",
            ""max"": 25,
            ""min"": 0,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""AGE"",
            ""description"": ""The customer's age"",
            ""ordinal"": 2,
            ""type"": ""integer"",
            ""max"": 125,
            ""min"": 7,
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        },
        {
            ""name"": ""DOB"",
            ""description"": ""The customer's date of birth"",
            ""ordinal"": 3,
            ""type"": ""string"",
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": ""m/d/yyyy"",
            ""regex"": null
        },
        {
            ""name"": ""PHONE"",
            ""description"": ""The customer's phone number"",
            ""ordinal"": 4,
            ""type"": ""string"",
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": ""^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$""
        },
        {
            ""name"": ""STATUS"",
            ""description"": ""Customer status"",
            ""ordinal"": 5,
            ""type"": ""enum"",
            ""values"": [ ""actv"", ""inac"" ],
            ""required"": false,
            ""null_or_empty"": true,
            ""format"": null,
            ""regex"": null
        }
    ]
}";

CsvValidator validator = new CsvValidator();
var options = new ValidationOptions()
{
    Separator = ',',
    HasHeaderRow = true,
    Quote = '\"',
    ValidationProfile = validationProfile
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
        if (msg.Character >=0) Console.WriteLine($"   Character  : {msg.Character}");
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
