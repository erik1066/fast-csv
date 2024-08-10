using System.Collections.ObjectModel;

namespace FastCsv;

public class ProcessRowResult
{
    public int FieldCount { get; init; } = -1;
    
    private readonly List<ValidationMessage> _messages = new();

    private readonly ReadOnlyCollection<ValidationMessage> _readOnlyMessages;

    public ReadOnlyCollection<ValidationMessage> Messages => _readOnlyMessages;
    
    public string Line { get; init; } = string.Empty;

    public bool ContainsLineBreakInQuotedField { get; init; } = false;

    public ProcessRowResult()
    {
        _readOnlyMessages = new ReadOnlyCollection<ValidationMessage>(_messages);
    }

    public void AddMessage(ValidationMessage message)
    {
        _messages.Add(message);
    }
        
}