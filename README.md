# Fast CSV Validator and Transformer

A .NET library for fast and efficient parsing, validation, and transformation of CSV files. 

Usage:

```cs
ValidationOptions options = new()
{
    Separator = ',',
    HasHeaderRow = true
};

using (FileStream fs = File.OpenRead(path))
{
    ValidationResult result = _validator.Validate(fs, options);
    Console.WriteLine(result.IsValid);
    Console.WriteLine(result.FieldCount);
    Console.WriteLine(result.ErrorCount);
}
```