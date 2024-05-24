using System.Text.Json.Serialization;

namespace backend.Model;

public class OrderOption
{
    public int Id { get; set; }
    public string OptionName { get; set; }
    public Boolean Active { get; set; }
    public Boolean Deleted { get; set; }
}