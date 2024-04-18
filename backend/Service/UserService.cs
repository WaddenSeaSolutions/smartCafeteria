using backend.DAL;
using backend.Model;

namespace backend.Service;

public class UserService
{
    UserDAL _userDAL;
    
    public UserService(UserDAL userDAL)
    {
        _userDAL = userDAL;
    }
    
    public User loginUser(string loginDataUsername, string loginDataPassword)
    {
        User user = _userDAL.userFromUsername(loginDataUsername);
        
        
    }
}