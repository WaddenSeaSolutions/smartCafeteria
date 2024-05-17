namespace backend.Model;

public class OrderOptionMqtt
{
    public int Id { get; set; }
    public string OptionName { get; set; }
    public Boolean Active { get; set; }
    public Boolean Deleted { get; set; }
    public int Number { get; set; }
}