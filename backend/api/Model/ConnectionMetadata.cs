namespace backend.Model;

public class ConnectionMetadata
{
    public string ConnectionId { get; set; }
    
    public int UserId { get; set; }
    
    public string Username { get; set; }
    
    public Boolean Authenticated { get; set; }
    
    public string Role { get; set; }
    
    public bool IsAdmin { get; set; }
}