using System.Diagnostics;

namespace RapidCsv;

[DebuggerDisplay("{Severity} : {Content}")]
public sealed class ValidationMessage
{
    public Guid Id { get; } = System.Guid.NewGuid();

    public int Code { get; init; } = -1;
    
    public string Content { get; init; } = string.Empty;

    public Severity Severity { get; init; } = Severity.Information;

    public ValidationMessageType MessageType { get; init; } = ValidationMessageType.Other;

    public int Row { get; init; } = -1;

    public int Character { get; init; } = -1;

    public int FieldNumber { get; init; } = -1;

    public string FieldName { get; init; } = string.Empty;

}