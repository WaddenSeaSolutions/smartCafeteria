using System.Text.Json.Serialization;

namespace backend.Model;

public class OrderOption
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("optionName")]
    public string OptionName { get; set; }
    [JsonPropertyName("active")]
    public Boolean Active { get; set; }
    [JsonPropertyName("deleted")]
    public Boolean Deleted { get; set; }
}