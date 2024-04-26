using backend.Model;

namespace backend.Interface;

public interface IOrderService
{
    public OrderOptionDTO CreateOrderOption(OrderOptionDTO optionToCreate);

}