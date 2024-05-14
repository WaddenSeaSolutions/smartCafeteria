namespace backend.Model;

public class Order
{
    public int Id { get; set; }
    
    public string Timestamp { get; set; }
    
    public Boolean Payment { get; set; }
    
    public Boolean Done { get; set; }
    
    public Array OrderOptions { get; set; }
}