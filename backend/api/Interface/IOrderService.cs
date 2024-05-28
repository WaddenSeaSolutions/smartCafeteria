using backend.Model;

namespace backend.Interface;

public interface IOrderService
{
    Order CreateOrder(OrderDTO orderDto);
    List<Order> GetOrders();
    Order UpdateDoneOnOrder(UpdateDoneOnOrderDTO order);
    Order UpdatePaymentOnOrder(UpdatePaymentOnOrderDTO updatePaymentOnOrderDto);
}