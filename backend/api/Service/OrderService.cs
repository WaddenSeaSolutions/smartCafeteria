using backend.DAL;
using backend.Interface;
using backend.Model;

namespace backend.Service;

public class OrderService : IOrderService
{
    private readonly IOrderDAL _orderDal;
    
    public OrderService(IOrderDAL orderDal)
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