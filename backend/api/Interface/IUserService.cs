using backend.Model;

namespace backend.Interface;

public interface IUserService
{
        User loginUser(string loginDataUsername, string loginDataPassword);
        void registerPersonnel(string username, string password, string role);
    
}