using System.Text.Json;

namespace FastCsv;

public class CsvFieldValidator : IFieldValidator
{
    private readonly ValidationProfile? _profile = null;
    private readonly ProcessRowOptions _rowOptions;
    private readonly List<ValidationColumnProfile> _columnProfiles;
    
    public CsvFieldValidator(string csvValidationProfile, ProcessRowOptions rowOptions)
    {
        _profile = JsonSerializer.Deserialize<ValidationProfile>(csvValidationProfile);
        _rowOptions = rowOptions;
        _columnProfiles = _profile?.Columns;
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
        
        // TODO: Check format
        
        // TODO: Check regular expression
        
        return messages;
    }
}