namespace FastCsv;

public class CsvValidator
{
    private char _quote = '\"';
    private IFieldValidator? _fieldValidator;
    
    public ValidationResult Validate(Stream content, ValidationOptions options) 
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        bool onHeaderRow = options.HasHeaderRow;
        int rowCount = 0;
        int initialFieldCount = -1;
        bool containsLineBreakInQuotedField = false;
        string previousLine = string.Empty;
        _quote = options.Quote;

        List<ValidationMessage> validationMessages = new List<ValidationMessage>();
        List<string> headers = new List<string>();
        ProcessRowOptions rowOptions = new ProcessRowOptions()
        {
            IsHeaderRow = onHeaderRow
        };

        if (!string.IsNullOrWhiteSpace(options.ValidationProfile))
        {
            _fieldValidator = new CsvFieldValidator(options.ValidationProfile, rowOptions);
        }

        using (StreamReader streamReader = new StreamReader(content))
        {
            while (!streamReader.EndOfStream)
            {
                string? line = streamReader.ReadLine();
                if (line == null) break; // no content found, just end

                if (containsLineBreakInQuotedField && !string.IsNullOrEmpty(previousLine))
                {
                    // we have content from a previous line to concatenate...
                    line = previousLine + "\r\n" + line;
                }
                
                ReadOnlySpan<char> row = line.AsSpan();
                rowOptions.IsHeaderRow = onHeaderRow;
                rowOptions.DataRowCount = rowCount;
                
                if (onHeaderRow)
                {
                    // TODO: Check column names against schema
                }
                
                ProcessRowResult processRowResult = ProcessRow(row, rowOptions, options);
                validationMessages.AddRange(processRowResult.Messages);
                containsLineBreakInQuotedField = processRowResult.ContainsLineBreakInQuotedField;

                if (onHeaderRow)
                {
                    headers.AddRange(processRowResult.HeaderRowNames);
                    for (int i = 0; i < processRowResult.HeaderRowNames.Count; i++)
                    {
                        string header = processRowResult.HeaderRowNames[i];
                        rowOptions.AddHeader(header, i);
                    }
                }
                
                onHeaderRow = false;

                int rowFieldCount = -1;
                
                if (initialFieldCount == -1)
                {
                    initialFieldCount = processRowResult.FieldCount;
                }
                else
                {
                    rowFieldCount = processRowResult.FieldCount;
                }

                if (containsLineBreakInQuotedField)
                {
                    previousLine = line;
                    continue;
                }
                
                rowCount++;

                if (initialFieldCount != -1 && rowFieldCount != -1 && rowFieldCount != initialFieldCount)
                {
                    // ERROR - RFC 4180 - field counts mismatch
                    var errorMessage = new ValidationMessage()
                    {
                        Code = 1,
                        Severity = Severity.Error,
                        Content = $"{rowFieldCount} detected; mismatch with first row's field count of {initialFieldCount}",
                        MessageType = ValidationMessageType.Structural,
                        Row = rowCount,
                        FieldNumber = -1,
                        FieldName = string.Empty,
                        Character = -1
                    };
                    validationMessages.Add(errorMessage);
                }
            }
        }

        sw.Stop();

        var result = new ValidationResult()
        {
            ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds,
            DataRowCount = options.HasHeaderRow ? rowCount - 1 : rowCount,
            FieldCount = initialFieldCount
        };

        for (int i = 0; i < headers.Count; i++)
        {
            string header = headers[i];
            if (string.IsNullOrEmpty(header))
            {
                // WARNING - RFC 4180 - empty field name
                var warningMessage = new ValidationMessage()
                {
                    Code = 4,
                    Severity = Severity.Warning,
                    Content = $"A field name with a length of 0 characters was detected",
                    MessageType = ValidationMessageType.Structural,
                    Row = 1,
                    FieldNumber = i + 1,
                    FieldName = string.Empty,
                    Character = -1
                };
                validationMessages.Add(warningMessage);
            }
            else if (string.IsNullOrWhiteSpace(header))
            {
                // INFO - RFC 4180 - field name with just whitespace
                var infoMessage = new ValidationMessage()
                {
                    Code = 5,
                    Severity = Severity.Information,
                    Content = $"A field name with only whitespace characters was detected",
                    MessageType = ValidationMessageType.Structural,
                    Row = 1,
                    FieldNumber = i + 1,
                    FieldName = string.Empty,
                    Character = -1
                };
                validationMessages.Add(infoMessage);
            }
            result.AddHeader(header);
        }
        
        foreach (var message in validationMessages)
        {
            result.AddMessage(message);
        }

        return result;
    }

    private ProcessRowResult ProcessRow(ReadOnlySpan<char> row, ProcessRowOptions options, ValidationOptions validationOptions)
    {
        List<ValidationMessage> validationMessages = new();

        List<string>? headerRowNames = options.IsHeaderRow ? new List<string>() : null;

        bool inQuotedField = false;
        bool isEscapedQuote = false;
        int fieldCount = 1;
        int previousCommaPosition = 0;

        bool lineBreakInQuotedField = false;

        if (options.IsHeaderRow && row.Length == 0)
        {
            // special case - header row with zero length, so assume there is just one field name and make it an empty string
            headerRowNames?.Add(string.Empty);
        }

        for (int i = 0; i < row.Length; i++)
        {
            // get the current character
            ReadOnlySpan<char> currentCharacter = row.Slice(start: i, length: 1);
            ReadOnlySpan<char> previousCharacter = i == 0 ? null : row.Slice(start: i - 1, length: 1);
            ReadOnlySpan<char> nextCharacter = i == row.Length - 1 ? null : row.Slice(start: i + 1, length: 1);

            if (inQuotedField == false && currentCharacter[0] == _quote && (previousCharacter == null || previousCharacter[0] == validationOptions.Separator))
            {
                // Happy path. We've come upon a quote that is preceded by a separator or null (new row) - we're now going to be processing all remaining characters as if they're in a quoted field
                inQuotedField = true;
            }
            else if (inQuotedField == true && currentCharacter[0] == _quote && (nextCharacter == null || nextCharacter[0] == validationOptions.Separator))
            {
                // Happy path. We've come upon a quote that ends with either a new line, or a separator, so we stop processing all remaining characters as if they're in a quoted field
                inQuotedField = false;
            }
            else if (inQuotedField == true
                     && isEscapedQuote == false
                     && currentCharacter[0] == _quote
                     && (nextCharacter != null && nextCharacter[0] != validationOptions.Separator))
            {
                // We're in a quoted field, and we came across a quote.
                isEscapedQuote = true;
            }
            else if (inQuotedField == true
                     && isEscapedQuote == true
                     && currentCharacter[0] == _quote
                     && (previousCharacter != null && previousCharacter[0] == _quote))
            {
                // We're in a quoted field, and we came across a second quote in a row.
                isEscapedQuote = false;
            }
            else if (inQuotedField == true
                     && isEscapedQuote == true
                     && currentCharacter[0] != _quote)
            {
                // TODO: Throw an ERROR - RFC-4180
                var errorMessage = new ValidationMessage()
                {
                    Code = 2,
                    Severity = Severity.Error,
                    Content = $"Unescaped quote detected in a quoted field",
                    MessageType = ValidationMessageType.Structural,
                    Row = options.DataRowCount,
                    FieldNumber = fieldCount,
                    FieldName = string.Empty,
                    Character = i
                };
                validationMessages.Add(errorMessage);

                isEscapedQuote = false;
            }
            else if (inQuotedField == false && currentCharacter[0] == _quote &&
                     (previousCharacter != null && previousCharacter[0] != validationOptions.Separator))
            {
                /* Unhappy path. We've come upon a quote that occurred outside a quoted string, but where the
                 prior character is neither null nor a separator. This indicates an invalid use of quotes.
                 Example: NA"ME,AGE,DOB
                 */
                var errorMessage = new ValidationMessage()
                {
                    Code = 2,
                    Severity = Severity.Error,
                    Content = $"Quote detected outside of a quoted string",
                    MessageType = ValidationMessageType.Structural,
                    Row = options.DataRowCount,
                    FieldNumber = fieldCount,
                    FieldName = string.Empty,
                    Character = i
                };
                validationMessages.Add(errorMessage);
            }
            else if (inQuotedField == true && isEscapedQuote == false && nextCharacter == null)
            {
                // we're in a quoted field and came across a line break - we need to do a continuation
                lineBreakInQuotedField = true;
            }

            if (!inQuotedField)
            {
                if (currentCharacter[0] == validationOptions.Separator)
                {
                    if (options.IsHeaderRow)
                    {
                        // Deal with this being a column name in a header row
                        int charsToTake = i - previousCommaPosition;
                        headerRowNames?.Add(row.Slice(start: previousCommaPosition, length: charsToTake).ToString());
                        previousCommaPosition = i + 1;
                    }
                    else
                    {
                        // Deal with this being a field value in a data row
                        int charsToTake = i - previousCommaPosition;
                        ReadOnlySpan<char> field = row.Slice(start: previousCommaPosition, length: charsToTake);
                        previousCommaPosition = i + 1;
                        
                        var messages = _fieldValidator?.ValidateField(
                            field: field, 
                            rowNumber: options.DataRowCount, 
                            fieldPosition: fieldCount);

                        if (messages != null && messages.Count > 0) validationMessages.AddRange(messages);
                    }
                    fieldCount++;
                }
                
                // the following code ensures that we get the final column name
                if (options.IsHeaderRow && i == row.Length - 1)
                {
                    int charsToTake = i - previousCommaPosition + 1;
                    headerRowNames?.Add(row.Slice(start: previousCommaPosition, length: charsToTake).ToString());
                }
                else if (i == row.Length - 1)
                {
                    // get the last field
                    int charsToTake = i - previousCommaPosition + 1;
                    
                    ReadOnlySpan<char> field = row.Slice(start: previousCommaPosition, length: charsToTake);
                    previousCommaPosition = i + 1;
                    
                    var messages = _fieldValidator?.ValidateField(
                        field: field, 
                        rowNumber: options.DataRowCount, 
                        fieldPosition: fieldCount);

                    if (messages != null && messages.Count > 0) validationMessages.AddRange(messages);
                }
            }
        }

        if (row.Length == 0)
        {
            // special case, the CSV file has 1 column and this is a null or empty cell
            var messages = _fieldValidator?.ValidateField(
                field: row, 
                rowNumber: options.DataRowCount, 
                fieldPosition: fieldCount);

            if (messages != null && messages.Count > 0) validationMessages.AddRange(messages);
        }

        var result = new ProcessRowResult()
        {
            FieldCount = fieldCount,
            ContainsLineBreakInQuotedField = lineBreakInQuotedField,
            Line = lineBreakInQuotedField ? row.ToString() : string.Empty
        };

        foreach (var message in validationMessages)
        {
            result.AddMessage(message);
        }

        if (options.IsHeaderRow && headerRowNames != null)
        {
            foreach (string headerRowName in headerRowNames)
            {
                result.AddHeaderRowName(headerRowName);
            }
        }

        return result;
    }
}
