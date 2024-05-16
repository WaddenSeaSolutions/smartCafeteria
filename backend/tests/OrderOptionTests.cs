using backend.DAL;
using backend.Interface;
using backend.Model;
using backend.Service;
using Moq;
using NUnit.Framework;

namespace tests;

[TestFixture]
public class OrderOptionTests
{
    private Mock<IOrderOptionDAL> _mockOrderDAL;
    private OrderOptionService _orderOptionService;
    [SetUp]
    public void Setup()
    {
        _mockOrderDAL = new Mock<IOrderOptionDAL>();
        _orderOptionService = new OrderOptionService(_mockOrderDAL.Object);
    }
    
    [Test]
    public void CreateOptionTest()
    {
        // Arrange
        var optionToCreate = new OrderOptionDTO
        {
            OptionName = "TestOption",
            Active = false,
            Deleted = false
        };

        var expectedOrderOption = new OrderOptionDTO
        {
            OptionName = "TestOption",
            Active = false,
            Deleted = false
        };

        _mockOrderDAL.Setup(dal => dal.CreateOrderOption(It.IsAny<OrderOptionDTO>()))
            .Returns((OrderOptionDTO optionToCreate) =>
            {
                var actualOrderOption = new OrderOption
                {
                    OptionName = optionToCreate.OptionName,
                    Active = optionToCreate.Active,
                    Deleted = optionToCreate.Deleted,
                };
                return actualOrderOption;
            });

        // Act
        var createdOrderOption = _orderOptionService.CreateOrderOption(optionToCreate);

        // Assert
        Assert.IsNotNull(createdOrderOption);
        Assert.AreEqual(expectedOrderOption.OptionName, createdOrderOption.OptionName);
        
    }

    [Test]
    public void DeleteOptionTest()
    {
        // Arrange
        var orderOptionToDelete = new OrderOption
        {
            Id = 55, 
        };

        var expectedDeletedOrderOption = new OrderOption
        {
            Id = 55,
        };

        _mockOrderDAL.Setup(dal => dal.DeleteOrderOption(It.IsAny<OrderOption>()))
            .Returns((OrderOption orderOptionToDelete) =>
            {
                var actualOrderOption = new OrderOption()
                {
                    Id = orderOptionToDelete.Id
                };
                return actualOrderOption;
            }) ;

        // Act
        var deletedOrderOption = _orderService.DeleteOrderOption(orderOptionToDelete);

        // Assert
        Assert.IsNotNull(deletedOrderOption);
        Assert.AreEqual(expectedDeletedOrderOption.Id, deletedOrderOption.Id);
        
        _mockOrderDAL.Verify(dal => dal.DeleteOrderOption(orderOptionToDelete), Times.Once);
    }
    [Test]
    public void UpdateOptionTest()
    {
        // Arrange
        var orderOptionToUpdate = new OrderOption
        {
            Id = 45,
            Active = true, 
        };

        var expectedUpdatedOrderOption = new OrderOption
        {
            Id = 45, 
            Active = true, 
        };

        _mockOrderDAL.Setup(dal => dal.UpdateOrderOption(It.IsAny<OrderOption>()))
            .Returns((OrderOption updatedOrderOption) =>
            {
                return updatedOrderOption;
            });

        // Act
        var updatedOrderOption = _orderService.UpdateOrderOption(orderOptionToUpdate);

        // Assert
        Assert.IsNotNull(updatedOrderOption);
        Assert.AreEqual(expectedUpdatedOrderOption.Id, updatedOrderOption.Id);
        Assert.AreEqual(expectedUpdatedOrderOption.Active, updatedOrderOption.Active);
    }

    [Test]
    public void GetOrderOptionsTest()
    {
        // Arrange
        var expectedOrderOptions = new List<OrderOption>
        {
            new OrderOption { Id = 1, OptionName = "Option 1", Active = true, Deleted = false },
            new OrderOption { Id = 2, OptionName = "Option 2", Active = true, Deleted = false },
            
        };

        _mockOrderDAL.Setup(dal => dal.GetOrderOptions())
            .Returns(expectedOrderOptions);

        // Act
        var actualOrderOptions = _orderService.GetOrderOptions();

        // Assert
        Assert.IsNotNull(actualOrderOptions);
        Assert.AreEqual(expectedOrderOptions.Count, actualOrderOptions.Count);
    }


}
