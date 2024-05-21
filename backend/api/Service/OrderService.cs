using backend.Interface;
using backend.Model;

namespace backend.Service;

public class OrderService : IOrderService
{
    private readonly IOrderDAL _orderDal;
    
    public OrderService(IOrderDAL orderDal)
    {
        _orderDal = orderDal;
    }
    public Order CreateOrder(OrderDTO orderDto)
    {
        return _orderDal.CreateOrder(orderDto);
    }

    public List<Order> GetOrders()
    {
        return _orderDal.GetOrders();
    }
}