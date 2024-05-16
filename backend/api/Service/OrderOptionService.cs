using backend.DAL;
using backend.Interface;
using backend.Model;

namespace backend.Service;

public class OrderOptionService : IOrderOptionService
{
    private readonly IOrderDAL _orderDal;
    
    public OrderOptionService(IOrderDAL orderDal)
    {
        _orderDal = orderDal;
    }
    
    public OrderOption CreateOrderOption(OrderOptionDTO optionToCreate)
    {
        return _orderDal.CreateOrderOption(optionToCreate);
    }

    public OrderOption DeleteOrderOption(OrderOption orderOption)
    {
       return _orderDal.DeleteOrderOption(orderOption);
    }

    public OrderOption UpdateOrderOption(OrderOption orderOption)
    {
        return _orderDal.UpdateOrderOption(orderOption);
    }

    public List<OrderOption> GetOrderOptions()
    {
        return _orderDal.GetOrderOptions();
    }
}