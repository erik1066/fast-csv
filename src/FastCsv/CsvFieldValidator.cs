using System.Globalization;
using System.Text.Json;

namespace FastCsv;

public class CsvFieldValidator : IFieldValidator
{
    private readonly ValidationProfile? _profile = null;
    private readonly ProcessRowOptions _rowOptions;
    private readonly List<ValidationColumnProfile> _columnProfiles;
    
    public CsvFieldValidator(ValidationProfile profile, ProcessRowOptions rowOptions)
    {
        _profile = profile;
        _rowOptions = rowOptions;
        _columnProfiles = _profile?.Columns ?? [];
    }

    public List<ValidationMessage> ValidateField(ReadOnlySpan<char> field, int rowNumber, int fieldPosition)
    {
        if (_profile == null || _profile.Columns.Count == 0)
            return [];

        List<ValidationMessage> messages = new List<ValidationMessage>();

        ValidationColumnProfile columnProfile = _columnProfiles[fieldPosition - 1];
        
        // Do data type validation
        switch (columnProfile.Type)
        {                        
            case "enum":
                messages.AddRange(ValidateEnumField(field, rowNumber, fieldPosition, columnProfile));
                messages.AddRange(ValidateStringField(field, rowNumber, fieldPosition, columnProfile));
                break;
            case "string":
                messages.AddRange(ValidateStringField(field, rowNumber, fieldPosition, columnProfile));
                break;
            case "integer":
                messages.AddRange(ValidateIntegerField(field, rowNumber, fieldPosition, columnProfile));
                break;
        }

        return messages;
    }
    
    private List<ValidationMessage> ValidateEnumField(ReadOnlySpan<char> field, int rowNumber, int fieldPosition, ValidationColumnProfile columnProfile)
    {
        List<ValidationMessage> messages = new List<ValidationMessage>();

        // Check values
        if (field.Length == 0 && columnProfile.Required == false)
        { }
        else if (columnProfile.Values.Count > 0)
        {
            var comparisonType = StringComparison.OrdinalIgnoreCase;
            
            if (columnProfile.AreValuesCaseSensitive)
            {
                comparisonType = StringComparison.Ordinal;
            }
            
            string fieldValue = field.ToString();
            bool found = false;
            foreach (var allowedValue in columnProfile.Values)
            {
                if (fieldValue.Equals(allowedValue, comparisonType))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var errorMessage = new ValidationMessage()
                {
                    Code = 9,
                    Severity = Severity.Error,
                    Content = $"Field '{columnProfile.Name}' must contain one of {columnProfile.Values.Count} allowed values, but none of the allowed values were found.",
                    MessageType = ValidationMessageType.Content,
                    Row = rowNumber,
                    FieldNumber = fieldPosition,
                    FieldName = columnProfile.Name,
                    Character = -1
                };
                messages.Add(errorMessage);
            }
        }
        
        // TODO: Check values URL
        
        return messages;
    }

    private List<ValidationMessage> ValidateStringField(ReadOnlySpan<char> field, int rowNumber, int fieldPosition, ValidationColumnProfile columnProfile)
    {
        List<ValidationMessage> messages = new List<ValidationMessage>();
        
        // Check Max
        if (field.Length > columnProfile.Max)
        {
            var errorMessage = new ValidationMessage()
            {
                Code = 10,
                Severity = Severity.Error,
                Content = $"Field '{columnProfile.Name}' must not be more than {columnProfile.Max} characters.",
                MessageType = ValidationMessageType.Content,
                Row = rowNumber,
                FieldNumber = fieldPosition,
                FieldName = columnProfile.Name,
                Character = -1
            };
            messages.Add(errorMessage);
        }
        
        // Check Min
        if (field.Length < columnProfile.Min)
        {
            var errorMessage = new ValidationMessage()
            {
                Code = 11,
                Severity = Severity.Error,
                Content = $"Field '{columnProfile.Name}' must not be less than {columnProfile.Min} characters.",
                MessageType = ValidationMessageType.Content,
                Row = rowNumber,
                FieldNumber = fieldPosition,
                FieldName = columnProfile.Name,
                Character = -1
            };
            messages.Add(errorMessage);
        }
        
        messages.AddRange(ValidateGenericFieldProperties(field, rowNumber, fieldPosition, columnProfile));
        
        if (!string.IsNullOrEmpty(columnProfile.Format))
        {
            string format = columnProfile.Format;
            
            if (format.StartsWith("mm", StringComparison.OrdinalIgnoreCase) || 
                format.StartsWith("m/", StringComparison.OrdinalIgnoreCase) ||
                format.StartsWith("m-", StringComparison.OrdinalIgnoreCase) ||
                format.StartsWith("m.", StringComparison.OrdinalIgnoreCase) ||
                format.StartsWith("dd", StringComparison.OrdinalIgnoreCase) || 
                format.StartsWith("d/", StringComparison.OrdinalIgnoreCase) ||
                format.StartsWith("d-", StringComparison.OrdinalIgnoreCase) ||
                format.StartsWith("d.", StringComparison.OrdinalIgnoreCase) ||
                format.StartsWith("d ", StringComparison.OrdinalIgnoreCase) ||
                format.StartsWith("yyyy", StringComparison.OrdinalIgnoreCase) 
                )
            {
                string fieldValue = field.ToString();
                
                bool success = DateTime.TryParseExact(
                    s: fieldValue, 
                    format: format, 
                    System.Globalization.CultureInfo.InvariantCulture, 
                    style: DateTimeStyles.None,
                    out DateTime dateTime);

                if (!success)
                {
                    var errorMessage = new ValidationMessage()
                    {
                        Code = 16,
                        Severity = Severity.Error,
                        Content = $"Field '{columnProfile.Name}' requires data to be in '{format}' date format.",
                        MessageType = ValidationMessageType.Content,
                        Row = rowNumber,
                        FieldNumber = fieldPosition,
                        FieldName = columnProfile.Name,
                        Character = -1
                    };
                    messages.Add(errorMessage);
                }
            }
            // Step 2: Check if lengths are off. If they are then we don't need to check any further.
            else if (field.Length != format.Length)
            {
                var errorMessage = new ValidationMessage()
                {
                    Code = 15,
                    Severity = Severity.Error,
                    Content = $"Field '{columnProfile.Name}' requires data to be in '{format}' format.",
                    MessageType = ValidationMessageType.Content,
                    Row = rowNumber,
                    FieldNumber = fieldPosition,
                    FieldName = columnProfile.Name,
                    Character = -1
                };
                messages.Add(errorMessage);
            }
            else
            {
                for (int i = 0; i < format.Length; i++)
                {
                    char formatChar = format[i];
                    char fieldChar = field[i];
                    
                    // We expected a number, but we got a non-number character
                    if (formatChar == '#' && !char.IsDigit(fieldChar))
                    {
                        var errorMessage = new ValidationMessage()
                        {
                            Code = 17,
                            Severity = Severity.Error,
                            Content = $"Field '{columnProfile.Name}' requires data to be in '{format}' format. Character mismatch at position {i + 1}. Expected a digit but found a non-digit.",
                            MessageType = ValidationMessageType.Content,
                            Row = rowNumber,
                            FieldNumber = fieldPosition,
                            FieldName = columnProfile.Name,
                            Character = -1
                        };
                        messages.Add(errorMessage);
                        break;
                    }
                    else if (formatChar != '#' && formatChar != fieldChar)
                    {
                        var errorMessage = new ValidationMessage()
                        {
                            Code = 18,
                            Severity = Severity.Error,
                            Content = $"Field '{columnProfile.Name}' requires data to be in '{format}' format. Character mismatch at position {i + 1}.",
                            MessageType = ValidationMessageType.Content,
                            Row = rowNumber,
                            FieldNumber = fieldPosition,
                            FieldName = columnProfile.Name,
                            Character = -1
                        };
                        messages.Add(errorMessage);
                        break;
                    }
                }
            }
        }
        
        if (!string.IsNullOrWhiteSpace(columnProfile.Regex))
        {
            string fieldValue = field.ToString();
            
            var match = columnProfile.RegularExpression.Match(fieldValue);

            if (match.Success == false)
            {
                var errorMessage = new ValidationMessage()
                {
                    Code = 19,
                    Severity = Severity.Error,
                    Content = $"Field '{columnProfile.Name}' has data that does not match the required regular expression.",
                    MessageType = ValidationMessageType.Content,
                    Row = rowNumber,
                    FieldNumber = fieldPosition,
                    FieldName = columnProfile.Name,
                    Character = -1
                };
                messages.Add(errorMessage);
            }
        }
        
        return messages;
    }

    private List<ValidationMessage> ValidateGenericFieldProperties(ReadOnlySpan<char> field, int rowNumber, int fieldPosition,
        ValidationColumnProfile columnProfile)
    {
        List<ValidationMessage> messages = new List<ValidationMessage>();
        
        // check required
        if (field.Length == 0 && columnProfile.Required)
        {
            var errorMessage = new ValidationMessage()
            {
                Code = 12,
                Severity = Severity.Error,
                Content = $"Field '{columnProfile.Name}' is required.",
                MessageType = ValidationMessageType.Content,
                Row = rowNumber,
                FieldNumber = fieldPosition,
                FieldName = columnProfile.Name,
                Character = -1
            };
            messages.Add(errorMessage);
        }
        else if (field.Length == 0 && columnProfile.CanBeNullOrEmpty == false)
        {
            var errorMessage = new ValidationMessage()
            {
                Code = 13,
                Severity = Severity.Error,
                Content = $"Field '{columnProfile.Name}' must not be null or empty.",
                MessageType = ValidationMessageType.Content,
                Row = rowNumber,
                FieldNumber = fieldPosition,
                FieldName = columnProfile.Name,
                Character = -1
            };
            messages.Add(errorMessage);
        }
        // check null or empty
        else if (field.Length == 4 && field[0] == 'n' && field[1] == 'u' && field[2] == 'l' && field[3] == 'l' &&
                 columnProfile.CanBeNullOrEmpty == false)
        {
            var errorMessage = new ValidationMessage()
            {
                Code = 14,
                Severity = Severity.Error,
                Content = $"Field '{columnProfile.Name}' must not be null.",
                MessageType = ValidationMessageType.Content,
                Row = rowNumber,
                FieldNumber = fieldPosition,
                FieldName = columnProfile.Name,
                Character = -1
            };
            messages.Add(errorMessage);
        }

        return messages;
    }
    
    private List<ValidationMessage> ValidateIntegerField(ReadOnlySpan<char> field, int rowNumber, int fieldPosition, ValidationColumnProfile columnProfile)
    {
        List<ValidationMessage> messages = new List<ValidationMessage>();
        
        bool success = Int64.TryParse(field, NumberStyles.Integer, CultureInfo.CurrentCulture, out Int64 fieldValue);

        if (!success)
        {
            messages.Add(new ValidationMessage()
            {
                Code = 20,
                Severity = Severity.Error,
                Content = $"Field '{columnProfile.Name}' must be an integer.",
                MessageType = ValidationMessageType.Content,
                Row = rowNumber,
                FieldNumber = fieldPosition,
                FieldName = columnProfile.Name,
                Character = -1
            });

            return messages;
        }
        
        // Check Max
        if (fieldValue > columnProfile.Max)
        {
            var errorMessage = new ValidationMessage()
            {
                Code = 21,
                Severity = Severity.Error,
                Content = $"Field '{columnProfile.Name}' must not be greater than {columnProfile.Max}.",
                MessageType = ValidationMessageType.Content,
                Row = rowNumber,
                FieldNumber = fieldPosition,
                FieldName = columnProfile.Name,
                Character = -1
            };
            messages.Add(errorMessage);
        }
        
        // Check Min
        if (fieldValue < columnProfile.Min)
        {
            var errorMessage = new ValidationMessage()
            {
                Code = 22,
                Severity = Severity.Error,
                Content = $"Field '{columnProfile.Name}' must not be less than {columnProfile.Min}.",
                MessageType = ValidationMessageType.Content,
                Row = rowNumber,
                FieldNumber = fieldPosition,
                FieldName = columnProfile.Name,
                Character = -1
            };
            messages.Add(errorMessage);
        }

        messages.AddRange(ValidateGenericFieldProperties(field, rowNumber, fieldPosition, columnProfile));
        
        return messages;
    }
}