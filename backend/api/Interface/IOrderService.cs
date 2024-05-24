using backend.Model;

namespace backend.Interface;

public interface IOrderService
{
    Order CreateOrder(OrderDTO orderDto);
    List<Order> GetOrders();
    Order UpdateOrder(OrderDTO? orderDto);
}