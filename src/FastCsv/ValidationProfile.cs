using System.Text.Json.Serialization;

namespace FastCsv;

public class ValidationProfile
{
    [JsonPropertyName("name")]
    [JsonPropertyOrder(0)]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    [JsonPropertyOrder(1)]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("filename")]
    [JsonPropertyOrder(2)]
    public string FileName { get; set; } = string.Empty;
    
    [JsonPropertyName("has_header")]
    [JsonPropertyOrder(3)]
    public bool HasHeader { get; set; } = true;

    [JsonPropertyName("separator")]
    [JsonPropertyOrder(4)]
    public string Separator { get; set; } = string.Empty;
    
    [JsonPropertyName("columns")]
    [JsonPropertyOrder(100)]
    public List<ValidationColumnProfile> Columns { get; set; } = new List<ValidationColumnProfile>();
}