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
    public Order CreateOrder(OrderDTO orderDto)
    {
        var sqlOrder = $@"INSERT INTO cafeteria.order (timestamp, payment, done, userId)
    VALUES (@timestamp, @payment,@done,@userId) RETURNING *;";

        var sqlOrderOption = $@"INSERT INTO cafeteria.userorder (orderid, orderoptionid) VALUES (@orderid,@orderoptionid)";

        var sqlFetchOrderOptions = $@"SELECT * FROM cafeteria.orderoption o
        WHERE o.id IN (SELECT uo.orderoptionid FROM cafeteria.userorder uo WHERE uo.orderid = @orderid)";

        using (var conn = _dataSource.OpenConnection())
        {
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var order = conn.QueryFirst<Order>(sqlOrder,
                        new { timestamp = orderDto.Timestamp, payment = orderDto.Payment, done = orderDto.Done, userId = orderDto.UserId },
                        transaction: transaction);
                    
                        foreach (var option in orderDto.OrderOptions)
                        {
                            conn.Execute(sqlOrderOption, new { orderid = order.Id, orderoptionid = option.Id }, transaction: transaction);
                        }
                        
                        var orderOptions = conn.Query<OrderOption>(sqlFetchOrderOptions, new { orderid = order.Id }, transaction: transaction).ToList();

                        order.OrderOptions = orderOptions;

                    order.OrderOptions = conn.Query<OrderOption>(sqlFetchOrderOptions, new { orderid = order.Id }, transaction: transaction).ToList();

                    transaction.Commit();
                    
                    return order;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    throw new Exception("Failed to create a new order");
                }
            }
        }
    }

    public List<Order> ReadOrder()
    {
        
        
        throw new NotImplementedException();
    }
}