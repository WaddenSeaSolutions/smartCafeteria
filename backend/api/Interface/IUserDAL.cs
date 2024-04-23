using backend.Model;

namespace backend.Interface;

public interface IUserDAL
{
    public void registerPersonnel(string username, string hashedPassword, string role);

    public User userFromUsername(string username);
}