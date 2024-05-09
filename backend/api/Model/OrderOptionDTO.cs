using System.ComponentModel.DataAnnotations;

namespace backend.Model;

public class OrderOptionDTO
{
    [Required]
    public string OptionName { get; set; }
    
    public Boolean Active { get; set; }
    
    public Boolean Deleted { get; set; }
}