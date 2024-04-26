using backend.Model;
using Dapper;
using Npgsql;

namespace backend.DAL;

public class OrderDAL
{
    private readonly NpgsqlDataSource _dataSource;
    
    public OrderDAL(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public OrderOption CreateOrderOption(OrderOptionDTO optionToCreate)
    {
        try
        {
            var sql = "INSERT INTO cafeteria.orderoption (optionname, active) VALUES (@optionName, @active)";

            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<OrderOption>(sql, new {optionName = optionToCreate.OptionName, active = optionToCreate.active});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to create order option");
        }
    }
}