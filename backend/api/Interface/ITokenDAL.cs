using backend.Model;

namespace backend.Interface;

public interface ITokenDAL
{
    User userFromUsername(string nameClaimValue);
}