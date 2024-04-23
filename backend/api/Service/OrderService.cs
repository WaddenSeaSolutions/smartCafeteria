using backend.DAL;

namespace backend.Service;

public class OrderService
{
    private readonly OrderDAL _orderDal;
    
    public OrderService(OrderDAL orderDal)
    {
        _orderDal = orderDal;
    }
}