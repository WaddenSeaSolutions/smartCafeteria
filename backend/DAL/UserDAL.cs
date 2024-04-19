using backend.Model;
using Dapper;
using Npgsql;

namespace backend.DAL;

public class UserDAL
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
            FROM forum.users
            WHERE username = @Username
            WHERE deleted = false";
        
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<User>(sql, new {Username = username});
            }
        }
        catch (Exception e)
        {
            throw new Exception("User is deleted/banned");
        }
    }
}