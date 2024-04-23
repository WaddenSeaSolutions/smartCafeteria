using backend.Interface;
using backend.WebSockets.MessageHandlers;
using Dapper;
using Npgsql;

namespace backend.DAL;

public class RegisterCustomerDAL : IRegisterCustomerDAL
{
    private readonly NpgsqlDataSource _dataSource;

    public RegisterCustomerDAL(NpgsqlDataSource npgsqlDataSource)
    {
        _dataSource = npgsqlDataSource;
    }
    public bool RegisterCustomer(RegisterCustomerHandler.RegisterCustomerData registerCustomerData)
    {
        Console.WriteLine("dal");
        var sql = $@"INSERT INTO cafeteria.user (username, password, role, deleted)
        VALUES (@username, @password, @role, @deleted);";

        using (var conn = _dataSource.OpenConnection())
        {
            var result = conn.Execute(sql,
                new
                {
                    username = registerCustomerData.Username, password = registerCustomerData.Password, role = "customer",
                    deleted = false
                });
            if (result > 0)
                return true;
        }

        return false;
    }
}