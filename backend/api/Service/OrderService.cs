using backend.DAL;
using backend.Interface;
using backend.Model;

namespace backend.Service;

public class OrderService : IOrderService
{
    private readonly OrderDAL _orderDal;
    
    public OrderService(OrderDAL orderDal)
    {
        _orderDal = orderDal;
    }
    
    public OrderOption CreateOrderOption(OrderOptionDTO optionToCreate)
    {
        Console.WriteLine("Hello there");
        return _orderDal.CreateOrderOption(optionToCreate);
    }
}