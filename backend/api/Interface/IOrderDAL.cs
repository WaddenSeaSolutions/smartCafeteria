using backend.Model;

namespace backend.Interface;

public interface IOrderDAL
{
    Order CreateOrder(OrderDTO orderDto);
    List<Order> GetOrders();
    Order UpdateDoneOnOrder(UpdateDoneOnOrderDTO orderDto);
    Order UpdatePaymentOnOrder(UpdatePaymentOnOrderDTO updatePaymentOnOrderDto);
}