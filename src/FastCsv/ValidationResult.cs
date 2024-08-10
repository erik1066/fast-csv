using System.Collections.ObjectModel;

namespace FastCsv;

public class ValidationResult
{
    private readonly List<ValidationMessage> _messages = new();

    private readonly ReadOnlyCollection<ValidationMessage> _readOnlyMessages;
    
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
    
    public ValidationResult()
    {
        _readOnlyMessages = new ReadOnlyCollection<ValidationMessage>(_messages);
    }

    public void AddMessage(ValidationMessage message)
    {
        _messages.Add(message);
    }
}