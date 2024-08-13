using System.Text.Json.Serialization;

namespace FastCsv;

public class ValidationColumnProfile
{
    private Int64 _min = Int64.MinValue;
    private Int64 _max = Int64.MaxValue;
    
    
    [JsonRequired]
    [JsonPropertyName("name")]
    [JsonPropertyOrder(0)]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    [JsonPropertyOrder(1)]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("ordinal")]
    [JsonPropertyOrder(2)]
    public int Ordinal { get; set; } = -1;
    
    [JsonPropertyName("type")]
    [JsonPropertyOrder(3)]
    public string Type { get; set; } = string.Empty;
    
    [JsonPropertyName("values")]
    [JsonPropertyOrder(4)]
    public List<string> Values { get; set; } = new List<string>(0);

    [JsonPropertyName("min")]
    [JsonPropertyOrder(5)]
    public long? Min
    {
        get => _min;
        set
        {
            if (value == null)
            {
                _min = Int64.MinValue;
            }
            else
            {
                _min = (long)value;
            }
        }
    }

    [JsonPropertyName("max")]
    [JsonPropertyOrder(6)]
    public long? Max
    {
        get => _max;
        set
        {
            if (value == null)
            {
                _max = Int64.MaxValue;
            }
            else
            {
                _max = (long)value;
            }
        }
    }
    
    [JsonPropertyName("required")]
    [JsonPropertyOrder(7)]
    public bool Required { get; set; } = false;
    
    [JsonPropertyName("null_or_empty")]
    [JsonPropertyOrder(8)]
    public bool CanBeNullOrEmpty { get; set; } = true;
    
    [JsonPropertyName("format")]
    [JsonPropertyOrder(9)]
    public string Format { get; set; } = string.Empty;
    
    [JsonPropertyName("regex")]
    [JsonPropertyOrder(10)]
    public string Regex { get; set; } = string.Empty;
}