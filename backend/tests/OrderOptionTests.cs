using backend.DAL;
using backend.Interface;
using backend.Model;
using backend.Service;
using Moq;
using NUnit.Framework;

namespace tests;

public class OrderOptionTests
{
    private Mock<IOrderDAL> _mockOrderDAL;
    private OrderOptionService _orderOptionService;
    [SetUp]
    public void Setup()
    {
        _mockOrderDAL = new Mock<IOrderDAL>();
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

        var expectedOrderOption = new OrderOption
        {
            OptionName = "TestOption",
            Active = false,
            Deleted = false
        };

        _mockOrderDAL.Setup(dal => dal.CreateOrderOption(It.IsAny<OrderOptionDTO>()))
            .Returns(expectedOrderOption);

        // Act
        var createdOrderOption = _orderOptionService.CreateOrderOption(optionToCreate);

        // Assert
        Assert.IsNotNull(createdOrderOption);
        Assert.AreEqual(expectedOrderOption, createdOrderOption);
        // You might want to add more assertions based on the expected behavior
        // of your CreateOrderOption method.
    }

    [Test]
    public void DeleteOptionTest()
    {
        
    }
}
