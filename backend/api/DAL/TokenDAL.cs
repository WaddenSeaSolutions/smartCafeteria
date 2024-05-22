using backend.Interface;
using backend.Model;
using Dapper;
using Npgsql;

namespace backend.DAL;


public class TokenDAL : ITokenDAL
{
    
    private readonly NpgsqlDataSource _dataSource;


    public TokenDAL(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    public User userFromUsername(string nameClaimValue)
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
            WHERE username = @Username";
        
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<User>(sql, new {Username = nameClaimValue});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
}