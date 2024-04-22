using backend.DAL;
using backend.Model;
using backend.WebSockets.MessageHandlers;

namespace backend.Service;

public class RegisterCustomerService
{
    private RegisterCustomerDAL _registerCustomerDal;

    public RegisterCustomerService(RegisterCustomerDAL registerCustomerDal)
    {
        _registerCustomerDal = registerCustomerDal;
    }
    public bool RegisterCustomer(RegisterCustomerHandler.RegisterCustomerData? registerCustomerData)
    {
        try
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerCustomerData.Password, 12);
            registerCustomerData.Password = hashedPassword;

            return _registerCustomerDal.RegisterCustomer(registerCustomerData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Failed to register user");
        }
    }
}