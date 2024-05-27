namespace backend.Model;

public class Order
{
    public int Id { get; set; }

    public string Timestamp { get; set; }

    public bool Payment { get; set; }

    public bool Done { get; set; }

    public int UserId { get; set; }

    public List<OrderOption> OrderOptions { get; set; }
    
}