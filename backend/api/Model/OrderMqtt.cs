namespace backend.Model;

public class OrderMqtt
{
    public int Id { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
    
    public Boolean Payment { get; set; }
    
    public Boolean Done { get; set; }
    
    public string Options { get; set; }
}