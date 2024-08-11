using System.Collections.ObjectModel;

namespace FastCsv;

public class ValidationResult
{
    private readonly List<ValidationMessage> _messages = new();

    private readonly ReadOnlyCollection<ValidationMessage> _readOnlyMessages;
    
    private readonly List<string> _headers = new();

    private readonly ReadOnlyCollection<string> _readOnlyHeaders;
    
    public Guid Id { get; } = System.Guid.NewGuid();
    public double ElapsedMilliseconds { get; init; } = default(double);
    public int DataRowCount { get; init; } = default(int);
    public int FieldCount { get; init; } = default(int);

    public bool IsValid
    {
        get
        {
            return !(_messages.Any(m => m.Severity == Severity.Error));
        }
    }
    
    public int ErrorCount
    {
        get
        {
            return _messages.Count(m => m.Severity == Severity.Error);
        }
    }
    
    public int WarningCount
    {
        get
        {
            return _messages.Count(m => m.Severity == Severity.Warning);
        }
    }

    public ReadOnlyCollection<ValidationMessage> Messages => _readOnlyMessages;
    public ReadOnlyCollection<string> Headers => _readOnlyHeaders;
    
    public ValidationResult()
    {
        _readOnlyMessages = new ReadOnlyCollection<ValidationMessage>(_messages);
        _readOnlyHeaders = new ReadOnlyCollection<string>(_headers);
    }

    public void AddMessage(ValidationMessage message)
    {
        _messages.Add(message);
    }
    
    public void AddHeader(string headerRowName)
    {
        _headers.Add(headerRowName);
    }
}