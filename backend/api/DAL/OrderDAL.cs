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
                    
                        foreach (var id in orderDto.OrderOptionId)
                        {
                            conn.Execute(sqlOrderOption, new { orderid = order.Id, orderoptionid = id }, transaction: transaction);
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
                }
            }
        }

        return null;
    }

    public List<Order> GetOrders()
    {
        var sql = $@"SELECT * FROM cafeteria.order WHERE timestamp::date = current_date";
        var sqlFetchOrderOptions = $@"SELECT * FROM cafeteria.orderoption o
        WHERE o.id IN (SELECT uo.orderoptionid FROM cafeteria.userorder uo WHERE uo.orderid = @orderid)";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                var orders = conn.Query<Order>(sql).ToList();

                foreach (var order in orders)
                {
                    var orderOptions = conn.Query<OrderOption>(sqlFetchOrderOptions, new { orderid = order.Id }).ToList();
                    order.OrderOptions = orderOptions;
                }

                return orders;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        return null;
    }
}