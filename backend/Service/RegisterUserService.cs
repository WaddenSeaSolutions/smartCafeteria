using backend.DAL;
using backend.Model;
using backend.WebSockets.MessageHandlers;

namespace backend.Service;

public class RegisterUserService
{
    private RegisterUserDAL _registerUserDal;

    public RegisterUserService(RegisterUserDAL registerUserDal)
    {
        _registerUserDal = registerUserDal;
    }
    public bool RegisterUser(RegisterUserHandler.RegisterUserData? registerUserData)
    {
        try
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUserData.Password, 12);
            registerUserData.Password = hashedPassword;

            return _registerUserDal.RegisterUser(registerUserData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Failed to register user");
        }
    }
}