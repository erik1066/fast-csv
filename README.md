# Fast CSV Validator and Transformer

[![NuGet version (RapidCsv)](https://img.shields.io/nuget/v/RapidCsv?style=flat-square)](https://www.nuget.org/packages/RapidCsv/)

A .NET library for fast and efficient validation and transformation of CSV files. 

Structural CSV validation rules adhere to [RFC 4180](https://www.rfc-editor.org/rfc/rfc4180). 

Additional content validation rules can be configured by supplying an *optional* JSON [validation profile](validator-config-schema.json). A validation profile allows specifying column names, data types, column rules (e.g. if data for that column are required, what the min/max length should be, and so on). 

## Performance

RFC 4180 validation on a 40 column, 100,000 row CSV file takes 235 ms and allocates a total of 100 MB of memory on an old Intel laptop CPU from the 2010s. See [benchmark results](./Benchmarks.md) for more.

You can run benchmarks using a special benchmarking project by navigating to `tests/RapidCsv.Benchmarks` and running:

```bash
dotnet run -c Release`
```

## Basic Usage - Validate a CSV file against [RFC 4180](https://www.rfc-editor.org/rfc/rfc4180)

1. Add a reference to RapidCsv in your `.csproj` file:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <PackageReference Include="RapidCsv" Version="0.0.1" />
  </ItemGroup>

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

2. Add a `using RapidCsv;` directive at the top of your class file.

3. Create a `CsvValidator` object and call its `Validate` method, passing both a stream and a `ValidationOptions` object into that method.

```cs
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

static Stream GenerateStreamFromString(string s)
{
    var stream = new MemoryStream();
    var writer = new StreamWriter(stream);
    writer.Write(s);
    writer.Flush();
    stream.Position = 0;
    return stream;
}
```

## Examples

The [examples](/examples/) folder contains example code that demonstrates how to use RapidCsv.

### Example #1: RFC 4180 validation in a .NET Console App

Let's look at the `RapidCsv.ConsoleDemo` project. This app shows how you how to validate a CSV file against just the RFC 4180 specification.

1. Navigate to [examples/demo-console/](examples/demo-console/) in a terminal of your choice. 
1. Enter the following into the terminal:

```bash
dotnet run
```

3. Observe for the following output:

```
Valid File = True
 Data Rows         = 4
 Elapsed time (ms) = 3ms
 Columns           = 3
 Error count       = 0
 Warning count     = 0
 Headers = 
  Column 1 = NAME
  Column 2 = AGE
  Column 3 = DOB
```

That's all there is to it.

> This console app includes a hard-coded CSV file in `program.cs` to make it as simple as possible to run the example. A CSV input file is therefore not required.

### Example #2: Profile-driven content validation

See [demo-console-content-validation](./examples/demo-console-content-validation/) for working and runnable code that uses the example below.

Let's say you want more than RFC 4180 validation. Perhaps you have a CSV file like the one below:


```
NAME,AGE,DOB,PHONE,STATUS
John,23,1/1/2012,555-555-5555,actv
Mary,34,1/1/1990,555-555-5555,inac
Jane,25,1/1/2010,555-555-5555,actv
Hana,55,1/1/1970,555-555-555X,unkn
```

And let's suppose we want to validate this CSV file on the following rules:

1. `NAME` must be 0-25 characters
1. `AGE` must be an integer
1. `DOB` must use `m/d/yyyy` format
1. `PHONE` must be a valid 10-digit US phone number
1. `STATUS` must be one of two values, `actv` or `inac`; all other values are invalid

We can create an optional validation profile in JSON that implements these rules:

```json
{
    "$schema": "rapid-csv/validator-config-schema.json",
    "name": "Acme Bookstore Customer Records",
    "description": "Validation profile for the CSV records of our Acme bookstore customers",
    "filename": "abc123.csv",
    "separator": ",",
    "has_header": true,
    "columns": [
        {
            "name": "NAME",
            "description": "The customer's name",
            "ordinal": 1,
            "type": "string",
            "max": 25,
            "min": 0,
            "required": false,
            "null_or_empty": true,
            "format": null,
            "regex": null
        },
        {
            "name": "AGE",
            "description": "The customer's age",
            "ordinal": 2,
            "type": "integer",
            "max": 125,
            "min": 7,
            "required": false,
            "null_or_empty": true,
            "format": null,
            "regex": null
        },
        {
            "name": "DOB",
            "description": "The customer's date of birth",
            "ordinal": 3,
            "type": "string",
            "required": false,
            "null_or_empty": true,
            "format": "m/d/yyyy",
            "regex": null
        },
        {
            "name": "PHONE",
            "description": "The customer's phone number",
            "ordinal": 4,
            "type": "string",
            "required": false,
            "null_or_empty": true,
            "format": null,
            "regex": "^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$"
        },
        {
            "name": "STATUS",
            "description": "Customer status",
            "ordinal": 5,
            "type": "enum",
            "values": [ "actv", "inac" ],
            "required": false,
            "null_or_empty": true,
            "format": null,
            "regex": null
        }
    ]
};
```
Note the use of the `format` property in the `DOB` column definition, the `regex` for the `PHONE` column, the use of `min` and `max` for `NAME`, and the `enum` with `values` in the `STATUS` column. These are how we define the five rules outlined earlier.

Using the profile is straightforward:

```csharp
// Create the validator object
CsvValidator validator = new CsvValidator();

// Create the validation options
var options = new ValidationOptions()
{
    Separator = ',',
    HasHeaderRow = true,
    ValidationProfile = validationProfile
};

Stream content = GenerateStreamFromString(csvContent);

// Validate the file using the validator, and return the result to the caller
ValidationResult result = validator.Validate(content: content, options: options);
```

In other words, we read the raw JSON into memory and assign it to the `ValidationProfile` property of the `ValidationOptions` object. The validator will then use the profile to execute these content checks. 

> Since `ValidationProfile` is optional and can be empty, leaving it empty will conduct basic RFC 4180 checks only and apply no content validation rules.

The added overhead of these profile-driven content checks can be significant in terms of performance when running the validator at scale. Use caution in applying these rules and only apply them when real-time content validation is required for the use case.


## Architecture and Design Decisions

RapidCsv is meant to be used in situations where one needs speed and memory efficiency _at scale_. For instance, if you're required to process CSV files in near real-time at high volume, where validation results are viewable by clients almost instantly after file submission, then this is a library worth considering. 

This is also why the library was built and shapes the design decisions around why the code is written the way it is.

### High performance and memory efficiency

The use of `ReadOnlySpan<T>` in the library is intentional. A simpler way of dealing with CSV files might be to use `string.Split(',')` but this presents issues, namely that splitting strings copies the string's contents into new memory (the array of string fragments that the `Split()` method generates). This increases memory use, the extra allocations result in slightly slower code, and it increases the amount of garbage collection that must occur to clean up all that duplicated memory.

By using `ReadOnlySpan<T>`, a lower-level API in .NET, we can get a view into a subset of the string instead of creating copies. Spans are harder to work with from a practical standpoint and make the code harder to read and maintain. 

A state machine-like algorithm is needed to parse each line in a CSV file. The algorithm goes character-by-character over the `ReadOnlySpan<char>` and must keep track of things like whether it's in a quoted field or not in order to know how to interpret the current character. Meanwhile, it must validate what it finds.

### No limits on file size

RapidCsv operates on streams. The whole CSV file does not need to be read at once, unlike some other competing libraries, and the fast performance means even larger files (e.g. 100k rows) can be validated in under 1 second.

### Human-readable error messages

Readable and understandable error messages are critical. Detected errors will give human-understandable outputs that even users with low technical skills should be able to understand, within reason.

### Ease of use by developers

The library is meant to be simple and easy to use by developers. It's one function call in one class:

```cs
CsvValidator validator = new CsvValidator();

var options = new ValidationOptions()
{
    Separator = ',',
    HasHeaderRow = true
};

ValidationResult result = validator.Validate(content: content, options: options);
```

In the code snippet above, we create a `validator` class, pass it some very basic `options`, and then call the validator's `validate` method. Without more advanced options this will validate the file against RFC 4180 specifications.

The `content` in this case is of type `Stream`. You can then do useful things with the `result` type you get back, such as iterate over all the errors/warnings or read a boolean flag to see if the file is valid or invalid.

There are more advanced things you can do with the `Validate` method such as specify a JSON content validation configuration, which will go beyond RFC 4180 and do things like check field content against your supplied regular expressions, data type specifications, min/max values, and other rules, but it is not required to supply such a configuration.

### Few to no dependencies

The software supply chain is hard to secure today. RapidCsv currently uses no dependencies. 

### Configurable content validation rules

Do you need to go beyond RFC 4180 rules for your real-time CSV validation needs? The [validation rules](./validator-config-schema.json) allow you to specify some basic content validation checks, such as min/max length, regular expression checks, formatting checks, and data types. These show up as error type _Content_ to distinguish them from RFC 4180 errors, which show up as error type _Structural_. 