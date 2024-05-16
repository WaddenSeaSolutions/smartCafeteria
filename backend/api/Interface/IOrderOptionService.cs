using backend.Model;

namespace backend.Interface;

public interface IOrderOptionService
{
    public OrderOption CreateOrderOption(OrderOptionDTO optionToCreate);

    public OrderOption DeleteOrderOption(OrderOption orderOption);
    public OrderOption UpdateOrderOption(OrderOption orderOption);
    public List<OrderOption> GetOrderOptions();
}