using backend.Model;

namespace backend.Interface;

public interface IOrderService
{
    public OrderOption CreateOrderOption(OrderOptionDTO optionToCreate);

}