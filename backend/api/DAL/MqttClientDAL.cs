using backend.Model;
using Dapper;
using Npgsql;

namespace backend.DAL;

public class MqttClientDAL
{
    private readonly NpgsqlDataSource _dataSource;

    public MqttClientDAL(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public List<string> GetOrderOptions()
    {
        var sql = $@"SELECT optionname FROM cafeteria.orderoption WHERE deleted = false AND active = true LIMIT 7";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<string>(sql).AsList();
        }
    }

    public OrderMqtt CreateNewOrderFromMqtt(OrderMqtt order)
    {
        try
        {
            var sql = $@"INSERT INTO cafeteria.order (timestamp, payment, done, userId) 
            VALUES (@timestamp, @payment,@done,@userId) RETURNING *;";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<OrderMqtt>(sql,
                    new { timestamp = order.Timestamp, payment = order.Payment, done = order.Done, userId = 1 });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to create a new order from MQTT");
        }
    }

    public void AddContentToOrder(List<int> orderNumbers, int orderId)
    {
        try
        {
            var sql = $@"INSERT INTO cafeteria.userorder (orderid, orderoptionid) VALUES (@orderid,@orderoptionid)";
            using (var conn = _dataSource.OpenConnection())
            {
                foreach (var number in orderNumbers)
                {
                    var parameters = new { orderid = orderId, orderoptionid = number };
                
                    // Execute the insert query for each orderoptionid
                    conn.Execute(sql, parameters);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to add content to order");
        }
    }
}