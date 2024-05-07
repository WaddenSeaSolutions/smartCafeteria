using backend.Model;
using Dapper;
using Npgsql;

namespace backend.DAL;

public class MQTTClientDAL
{
    private readonly NpgsqlDataSource _dataSource;

    public MQTTClientDAL(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public OrderOption GetOrderOptions(OrderOption orderOption)
    {
        return null;
    }

    public Order CreateNewOrderFromMQTT(OrderMQTT order)
    {
        try
        {
            var sql = $@"INSERT INTO cafeteria.order (timestamp, payment, done, userId) 
            VALUES (@timestamp, @payment,@done,@userId)";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<Order>(sql,
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
            var sql = $@"INSERT INTO cefeteria.ordercontent (orderid, orderoptionid) VALUES (@orderid,@orderoptionid)";
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