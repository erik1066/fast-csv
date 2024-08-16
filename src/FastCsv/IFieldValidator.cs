namespace FastCsv;

public interface IFieldValidator
{
    public List<ValidationMessage> ValidateField(ReadOnlySpan<char> field, int rowNumber, int fieldPosition);
}