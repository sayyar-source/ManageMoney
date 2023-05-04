using ManageMoney.Api.Controllers;
using ManageMoney.Api.ViewModels.Requests;
using ManageMoney.Api.ViewModels.Responses;
using ManageMoney.Application.Services;
using ManageMoney.Data.Dtos;
using ManageMoney.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManageMoney.UnitTest
{
    public class AccountControllerTest
    {
        private readonly AccountController _controller;
        private readonly IAccountService _accountService;

        public AccountControllerTest()
        {
          
            _accountService = new AccountServiceFacke();
            _controller = new AccountController(_accountService);
        }
        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult =await _controller.GetAllAccounts();

            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult =await _controller.GetAllAccounts() as OkObjectResult;

            // Assert
            var items = Assert.IsType<AccountResponse[]>(okResult.Value);
            Assert.Equal(3, items.Count());
        }

        [Fact]
        public async void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult =await _controller.GetAccountById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetById_ExistingGuidPassed_ReturnsOkResult()
        {
            // Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResult =await _controller.GetAccountById(testGuid);

            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task GetById_ExistingGuidPassed_ReturnsRightItem()
        {
            // Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResult =await _controller.GetAccountById(testGuid) as OkObjectResult;

            // Assert
            Assert.IsType<AccountResponse>(okResult.Value);
            Assert.Equal(testGuid, (okResult.Value as AccountResponse).Id);
        }

        [Fact]
        public async Task Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var account = new AccountRequest
            {
               AccountType=AccountType.TR
                
            };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse =await _controller.AddAccount(account);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        //[Fact]
        //public async Task Add_ValidObjectPassed_ReturnsCreatedResponse()
        //{
        //    // Arrange
        //    var account = new AccountRequest
        //    {
        //        AccountType = AccountType.TR,
        //        Name="Lira"
                
        //    };

        //    // Act
        //    var createdResponse =await _controller.AddAccount(account);

        //    // Assert
        //    Assert.IsType<CreatedAtActionResult>(createdResponse);
        //}

        //[Fact]
        //public async Task Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        //{
        //    // Arrange
        //    var account = new AccountRequest
        //    {
        //        AccountType = AccountType.TR,
        //        Name = "Lira"

        //    };

        //    // Act
        //    var createdResponse =await _controller.AddAccount(account);
        //    var item = createdResponse;

        //    // Assert
        //    Assert.IsType<AccountResponse>(item);
        //    Assert.Equal("Lira", item.);
        //}

        [Fact]
        public async Task Remove_NotExistingGuidPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var notExistingGuid = Guid.NewGuid();

            // Act
            var badResponse =await _controller.DeleteAccount(notExistingGuid);

            // Assert
            Assert.IsType<NotFoundResult>(badResponse);
        }

        [Fact]
        public async Task Remove_ExistingGuidPassed_ReturnsNoContentResult()
        {
            // Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var noContentResponse =await _controller.DeleteAccount(existingGuid);

            // Assert
            Assert.IsType<NoContentResult>(noContentResponse);
        }

        [Fact]
        public async void Remove_ExistingGuidPassed_RemovesOneItem()
        {
            // Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResponse =await _controller.DeleteAccount(existingGuid);

            // Assert
            Assert.Equal(2, _accountService.GetAllAccountsAsync().Result.Count());
        }
    }
}