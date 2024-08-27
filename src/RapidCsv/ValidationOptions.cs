namespace RapidCsv;

public class ValidationOptions
{
    public char Separator { get; init; } = ',';
    
    public bool HasHeaderRow { get; init; } = false;

    public char Quote { get; init; } = '\"';

    public string ValidationProfile { get; set; } = string.Empty;


}