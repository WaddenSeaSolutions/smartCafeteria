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

    public List<OrderOptionMqtt> GetOrderOptions()
    {
        var sql = $@"SELECT * FROM cafeteria.orderoption WHERE deleted = false AND active = true LIMIT 7";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<OrderOptionMqtt>(sql).AsList();
        }
    }

    public OrderMqtt CreateNewOrderFromMqtt(OrderMqtt order, List<OrderOptionMqtt> orderOptionList)
    {
        Console.WriteLine(orderOptionList.ToString());
        using (var conn = _dataSource.OpenConnection())
        using (var transaction = conn.BeginTransaction())
        {
            try
            {
                var insertOrderSql = @"
                INSERT INTO cafeteria.order (timestamp, payment, done, userId) 
                VALUES (@timestamp, @payment, @done, @userId) RETURNING *;";
            
                var createdOrder = conn.QueryFirst<OrderMqtt>(insertOrderSql,
                    new { timestamp = order.Timestamp, payment = order.Payment, done = order.Done, userId = 1 },
                    transaction);
                
                var insertOrderOptionsSql = @"
                INSERT INTO cafeteria.userorder (orderid, orderoptionid) 
                VALUES (@orderid, @orderoptionid);";
            
                foreach (var orderOption in orderOptionList)
                {
                    var parameters = new { orderid = createdOrder.Id, orderoptionid = orderOption.Id };
                    conn.Execute(insertOrderOptionsSql, parameters, transaction);
                }

                transaction.Commit();

                // Return the created order
                return createdOrder;
            }
            catch (Exception e)
            {
                // Rollback the transaction in case of an error
                transaction.Rollback();
                Console.WriteLine(e);
                throw new Exception("Failed to create a new order with content from MQTT");
            }
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