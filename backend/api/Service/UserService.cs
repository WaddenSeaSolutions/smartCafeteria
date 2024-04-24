using backend.DAL;
using backend.Interface;
using backend.Model;

namespace backend.Service;

public class UserService : IUserService
{
    private IUserDAL _userDAL;
    
    public UserService(IUserDAL userDAL)
    {
        _userDAL = userDAL;
    }
    
    public User loginUser(string loginDataUsername, string loginDataPassword)
    {
        try
        {
            User userToCheck = _userDAL.userFromUsername(loginDataUsername);

            if (BCrypt.Net.BCrypt.Verify(loginDataPassword, userToCheck.Password))
            {
                return userToCheck;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred during login" + e.Message);
            throw new Exception("Login failed");
        }

        return null;
    }

    public void registerPersonnel(string username, string password, string role)
    {

        try
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 12);
            
            _userDAL.registerPersonnel(username, hashedPassword, role);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception( "Failed to register");
        }
    }
}