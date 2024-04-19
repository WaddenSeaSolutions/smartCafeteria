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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            _userDAL.registerPersonnel(username, hashedPassword, role);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}