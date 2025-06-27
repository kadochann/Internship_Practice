using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using BookStoresWebAPI.Controllers;
using BookStoresWebAPI.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UnitTestProject_BookStoresApp
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
        private readonly Mock<ILogger<AccountController>> _loggerMock;

        public AccountControllerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();

            
            _userManagerMock = new Mock<UserManager<AppUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            _signInManagerMock = new Mock<SignInManager<AppUser>>(
                _userManagerMock.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null, null, null, null
            );

            _loggerMock = new Mock<ILogger<AccountController>>();
        }

        [Fact]
        public async Task IsUserNameAvailable_ReturnsTrue_WhenUserDoesNotExist()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync("nonexisting"))
                            .ReturnsAsync((AppUser)null);

            var controller = new AccountController(
                _loggerMock.Object,
                _signInManagerMock.Object,
                _userManagerMock.Object
            );

            // Act
            var result = await controller.IsUserNameAvailable("nonexisting");

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value);
        }
    }
}
