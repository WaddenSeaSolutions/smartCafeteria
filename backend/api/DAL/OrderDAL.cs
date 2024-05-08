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
            var sql = "INSERT INTO cafeteria.orderoption (optionname, active) VALUES (@optionname, @active) RETURNING *";
            using (var conn = _dataSource.OpenConnection())
            {
                OrderOption orderOption = conn.QueryFirst<OrderOption>(sql, new {optionname = optionToCreate.OptionName, active = optionToCreate.Active});
                return orderOption;
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
            var sql = "DELETE FROM cafeteria.orderoption WHERE id = @id RETURNING *";
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
            var sql = "UPDATE cafeteria.orderoption SET active = @active WHERE id = @id RETURNING *";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<OrderOption>(sql, new {active = orderOption.Active, id = orderOption.Id});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to update order option");
        }
    }

    public List<OrderOption> GetOrderOptions()
    {
        try
        {
            var sql = "SELECT * FROM cafeteria.orderoption";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.Query<OrderOption>(sql).ToList();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to get order options");
        }
    }
}