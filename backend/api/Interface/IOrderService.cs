using backend.Model;

namespace backend.Interface;

public interface IOrderService
{
    public OrderOption CreateOrderOption(OrderOptionDTO optionToCreate);

    public OrderOption DeleteOrderOption(OrderOption orderOption);
    public OrderOption UpdateOrderOption(OrderOption orderOption);
    List<OrderOption> GetOrderOptions();
}