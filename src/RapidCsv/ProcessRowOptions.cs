using System.Collections.ObjectModel;

namespace RapidCsv;

public class ProcessRowOptions
{
    private bool _isHeaderRow = true;
    private int _dataRowCount = -1;

    public int DataRowCount
    {
        get => _dataRowCount;
        set
        {
            if (value < -1) throw new ArgumentOutOfRangeException(nameof(DataRowCount));
            if (value >= _dataRowCount) _dataRowCount = value;
            else
            {
                throw new InvalidOperationException(
                    "Cannot set the row count flag to a value lower than it was previously");
            }
        }
    }

    public bool IsHeaderRow
    {
        get => _isHeaderRow;
        set
        {
            if (_isHeaderRow == true) _isHeaderRow = value;
            else if (_isHeaderRow == false && value == true)
            {
                throw new InvalidOperationException(
                    "Cannot set the header flag to true after it's already been set to false");
            }
        }
    }

    private readonly Dictionary<int, string> _headers = new();

    private readonly ReadOnlyDictionary<int, string> _readOnlyHeaders;
    
    public ReadOnlyDictionary<int, string> Headers => _readOnlyHeaders;
    
    public ProcessRowOptions()
    {
        _readOnlyHeaders = new ReadOnlyDictionary<int, string>(_headers);
    }

    public void AddHeader(string header, int headerPosition)
    {
        if (headerPosition < 0) throw new ArgumentOutOfRangeException(nameof(DataRowCount));
        
        if (!_headers.TryAdd(headerPosition, header))
        {
            throw new InvalidOperationException("Cannot add two fields with the same position");
        }
    }

}