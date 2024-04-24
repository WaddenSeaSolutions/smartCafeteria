using backend.Model;

namespace backend.Interface;

public interface ITokenService
{
    string createToken(User user);
    User validateTokenAndReturnUser(string token);
}