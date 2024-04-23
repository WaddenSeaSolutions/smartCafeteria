using Npgsql;

namespace backend.DAL;

public class OrderDAL
{
    private readonly NpgsqlDataSource _dataSource;
    
    public OrderDAL(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
}