using BookStoresWebAPI.Controllers;
using BookStoresWebAPI.Models.Identity;
using BookStoresWebAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit; // Ensure Xunit is imported for [Fact] attribute  

namespace Register_Test__BookStoresApp
{
    public class RegisterTests // Corrected class declaration  
    {
        [Fact]
        public async Task Register_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange  
            var userManagerMock = new Mock<UserManager<AppUser>>(
                new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);

            var signInManagerMock = new Mock<SignInManager<AppUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
                null, null, null, null);

            var loggerMock = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(loggerMock.Object, signInManagerMock.Object, userManagerMock.Object);

            // Force ModelState to be invalid  
            controller.ModelState.AddModelError("UserName", "Required");

            var model = new RegisterViewModel
            {
                UserName = "",
                Email = "test@example.com",
                Password = "1234",
                ConfirmPassword = "1234"
            };

            // Act  
            var result = await controller.Register(model);

            // Assert  
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedModel = Assert.IsType<RegisterViewModel>(viewResult.Model);
            Assert.Equal(model, returnedModel);
        }
    }
}