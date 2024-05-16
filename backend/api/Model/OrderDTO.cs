namespace backend.Model;

public class OrderDTO
{
    public DateTimeOffset Timestamp { get; set; }
    
    public Boolean Payment { get; set; }
    
    public Boolean Done { get; set; }
    
    public Array OrderOptions { get; set; }
    
    public int UserId { get; set; }
}