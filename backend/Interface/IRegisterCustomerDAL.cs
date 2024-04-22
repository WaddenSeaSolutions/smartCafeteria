using backend.WebSockets.MessageHandlers;

namespace backend.Interface;

public interface IRegisterCustomerDAL
{ 
    public bool RegisterCustomer(RegisterCustomerHandler.RegisterCustomerData registerCustomerData);
}