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
            var sql = "INSERT INTO cafeteria.orderoption (optionname, active) VALUES (@optionName, @active) RETURNING id";
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
    
    public OrderOption DeleteOrderOption(OrderOption orderOption)
    {
        try
        {
            var sql = "DELETE FROM cafeteria.orderoption WHERE id = @id RETURNING id";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<OrderOption>(sql, new {id = orderOption.Id});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to delete order option");
        }
    }

    public OrderOption UpdateOrderOption(OrderOption orderOption)
    {
        try
        {
            var sql = "UPDATE cafeteria.orderoption SET active = @active WHERE id = @id RETURNING id";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<OrderOption>(sql, new {active = orderOption.active, id = orderOption.Id});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to update order option");
        }
    }
}