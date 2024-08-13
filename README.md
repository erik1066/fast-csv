# Fast CSV Validator and Transformer

A .NET library for fast and efficient parsing, validation, and transformation of CSV files. 

## Usage

1. Add a reference to FastCsv in your `.csproj` file:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <ProjectReference Include="..\..\src\FastCsv\FastCsv.csproj" />
  </ItemGroup>

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

2. Add a `using FastCsv;` directive at the top of your class file.

3. Create a `CsvValidator` object and call its `Validate` method, passing both a stream and a `ValidationOptions` object into that method.

```cs
using FastCsv;

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

The [examples](/examples/) folder contains example code that demonstrates how to use FastCsv.

### Simplest Example: .NET Console App

Let's look at the `FastCsv.ConsoleDemo` project. 

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
