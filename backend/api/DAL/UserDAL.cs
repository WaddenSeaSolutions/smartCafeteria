using backend.Interface;
using backend.Model;
using Dapper;
using Npgsql;

namespace backend.DAL;

public class UserDAL : IUserDAL
{
    private readonly NpgsqlDataSource _dataSource;


    public UserDAL(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    public User userFromUsername(string username)
    {
        try
        {
            var sql = $@"
            SELECT id as {nameof(User.Id)},
            username as {nameof(User.Username)},
            password as {nameof(User.Password)},
            role as {nameof(User.Role)},
            deleted as {nameof(User.Deleted)}
            FROM cafeteria.user
            WHERE username = @Username AND deleted = false";
        
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<User>(sql, new {Username = username});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("User is deleted/banned");
        }
    }

    public void registerPersonnel(string username, string hashedPassword, string role)
    {
        try
        {
            var sql = $@"
            INSERT INTO cafeteria.user (username, password, role, deleted)
            VALUES (@Username, @Password, @Role, @Deleted)";
        
            using (var conn = _dataSource.OpenConnection())
            {
                conn.Execute(sql, new {Username = username, Password = hashedPassword, Role = role, Deleted = false});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Failed to register personnel");
        }
    }
}