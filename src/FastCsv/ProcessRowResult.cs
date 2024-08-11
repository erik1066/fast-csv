using System.Collections.ObjectModel;

namespace FastCsv;

public class ProcessRowResult
{
    public int FieldCount { get; init; } = -1;
    
    private readonly List<ValidationMessage> _messages = new();

    private readonly ReadOnlyCollection<ValidationMessage> _readOnlyMessages;

    public ReadOnlyCollection<ValidationMessage> Messages => _readOnlyMessages;
    
    private readonly List<string> _headerRowNames = new();

    private readonly ReadOnlyCollection<string> _readOnlyHeaderRowNames;

    public ReadOnlyCollection<string> HeaderRowNames => _readOnlyHeaderRowNames;
    
    public string Line { get; init; } = string.Empty;

    public bool ContainsLineBreakInQuotedField { get; init; } = false;

    public ProcessRowResult()
    {
        _readOnlyMessages = new ReadOnlyCollection<ValidationMessage>(_messages);
        _readOnlyHeaderRowNames = new ReadOnlyCollection<string>(_headerRowNames);
    }

    public void AddMessage(ValidationMessage message)
    {
        _messages.Add(message);
    }
    
    public void AddHeaderRowName(string headerRowName)
    {
        _headerRowNames.Add(headerRowName);
    }
        
}