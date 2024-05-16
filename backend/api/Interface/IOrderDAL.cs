using backend.Model;

namespace backend.Interface;

public interface IOrderDAL
{
    Order CreateOrder(OrderDTO orderDto);
}