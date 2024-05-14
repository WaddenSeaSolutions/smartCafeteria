using backend.Interface;
using backend.Model;
using Dapper;
using Npgsql;

namespace backend.DAL;

public class OrderDAL : IOrderDAL
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
            var sql = "INSERT INTO cafeteria.orderoption (optionname, active, deleted) VALUES (@optionname, @active, @deleted) RETURNING *";
            using (var conn = _dataSource.OpenConnection())
            {
                OrderOption orderOption = conn.QueryFirst<OrderOption>(sql, new {optionname = optionToCreate.OptionName, active = optionToCreate.Active, deleted = optionToCreate.Deleted});
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
            Console.WriteLine(orderOption.Id);
            Console.WriteLine(orderOption);
            var sql = "UPDATE cafeteria.orderoption set deleted = true WHERE id = @id RETURNING *";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirstOrDefault<OrderOption>(sql, new {id = orderOption.Id});
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
            var sql = "SELECT * FROM cafeteria.orderoption WHERE deleted = false";
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