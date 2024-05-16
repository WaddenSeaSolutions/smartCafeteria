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

        var sqlOrderOption = $@"INSERT INTO cafeteria.userorder (orderid, Id) VALUES (@orderid, @Id)";

        using (var conn = _dataSource.OpenConnection())
        {
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var order = conn.QueryFirst<Order>(sqlOrder,
                        new { timestamp = orderDto.Timestamp, payment = orderDto.Payment, done = orderDto.Done, userId = orderDto.UserId },
                        transaction: transaction);

                    if (orderDto.OrderOptions != null)
                    {
                        foreach (var optionId in orderDto.OrderOptions)
                        {
                            conn.Execute(sqlOrderOption, new { orderid = order.Id, Id = optionId }, transaction: transaction);
                        }
                    }

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
}