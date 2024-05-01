// using System.IdentityModel.Tokens.Jwt;
// using backend.Interface;
// using backend.Model;
// using backend.Service;
// using Moq;
// using NUnit.Framework;
// using FluentAssertions;
//
// namespace tests;
//
// [TestFixture]
// public class TokenServiceTest
// {
//     private Mock<ITokenDAL> _mockTokenDal;
//
//     [SetUp]
//     public void Setup()
//     {
//         _mockTokenDal = new Mock<ITokenDAL>();
//     }
//     
//     [Test]
//     public void TestCreateToken()
//     {
//         // Arrange
//         var user = new User
//         {
//             Id = 10,
//             Username = "jones",
//             Role = "customer"
//         };
//         
//         _mockTokenDal.Setup(dal => dal.userFromUsername(It.Is<string>(x => x == "jones"))).Returns(user);
//         
//         // Act
//         var token = new TokenService(_mockTokenDal.Object).createToken(user);
//         
//         // Assert
//         var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
//         // Extract information from the decoded token
//         string userId = jwtToken.Payload.GetValueOrDefault("nameid")?.ToString();
//         string username = jwtToken.Payload.GetValueOrDefault("unique_name")?.ToString();
//         string role = jwtToken.Payload.GetValueOrDefault("role")?.ToString();
//
//         // Perform assertions using FluentAssertions
//         userId.Should().Be("10");
//         username.Should().Be("jones");
//         role.Should().Be("customer");
//     }
// }