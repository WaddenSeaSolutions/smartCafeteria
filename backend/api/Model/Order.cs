namespace backend.Model;

public class Order
{
    public int Id { get; set; }

    public string Timestamp { get; set; }

    public Boolean Payment { get; set; }

    public Boolean Done { get; set; }

    public int UserId { get; set; }

    private List<OrderOption> _orderOptions;
    public List<OrderOption> OrderOptions
    {
        get { return _orderOptions; }
        set
        {
            if (value.Count <= 4)
            {
                _orderOptions = value;
            }
            else
            {
                throw new InvalidOperationException("Cannot have more than 4 order options.");
            }
        }
    }
}