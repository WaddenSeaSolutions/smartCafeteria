using backend.DAL;
using backend.Interface;
using backend.Model;

namespace backend.Service;

public class OrderOptionService : IOrderOptionService
{
    private readonly IOrderOptionDAL _orderOptionDal;
    
    public OrderOptionService(IOrderOptionDAL orderOptionDal)
    {
        _orderOptionDal = orderOptionDal;
    }
    
    public OrderOption CreateOrderOption(OrderOptionDTO optionToCreate)
    {
        return _orderOptionDal.CreateOrderOption(optionToCreate);
    }

    public OrderOption DeleteOrderOption(OrderOption orderOption)
    {
       return _orderOptionDal.DeleteOrderOption(orderOption);
    }

    public OrderOption UpdateOrderOption(OrderOption orderOption)
    {
        return _orderOptionDal.UpdateOrderOption(orderOption);
    }

    public List<OrderOption> GetOrderOptions()
    {
        return _orderOptionDal.GetOrderOptions();
    }
}