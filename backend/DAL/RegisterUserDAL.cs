using backend.Model;
using backend.WebSockets.MessageHandlers;
using Dapper;
using Npgsql;

namespace backend.DAL;

public class RegisterUserDAL
{
    private readonly NpgsqlDataSource _dataSource;
    public bool RegisterUser(RegisterUserHandler.RegisterUserData registerUserData)
    {
        var sql = $@"INSERT INTO cafeteria.user (username, password, role, deleted)
        VALUES (@username, @password, @role, @deleted);";

        using (var conn = _dataSource.OpenConnection())
        {
            var result = conn.Execute(sql,
                new
                {
                    username = registerUserData.Username, password = registerUserData.Password, role = "customer",
                    deleted = false
                });
            if (result > 0)
                return true;
        }

        return false;
    }
}