namespace backend.Model;

public class OrderOption
{
    public int Id { get; set; }
    
    public string OptionName { get; set; }
    
    public Boolean active { get; set; }
    
    public bool IsUpdated { get; set; }
    
    public bool isNew { get; set; }
    
    public bool IsDeleted { get; set; }
}