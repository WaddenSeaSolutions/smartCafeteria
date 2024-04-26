using backend.DAL;
using backend.Model;

namespace backend.Service;

public class OrderService
{
    private readonly OrderDAL _orderDal;
    
    public OrderService(OrderDAL orderDal)
    {
        _orderDal = orderDal;
    }
    
    public void CreateOrderOption(OrderOptionDTO optionToCreate)
    {
        _orderDal.CreateOrderOption(optionToCreate);
    }
}