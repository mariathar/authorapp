using AuthorApp.ApiContracts.Request;
using AuthorApp.Controllers;
using AuthorApp.DataAccess.DataManagers.Authors;
using AuthorApp.DataAccessContracts.Models;
using AuthorApp.Mapper;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorApp.ApiTests.ControllerTests.AuthorControllerTests
{
    [TestFixture]
    public class AuthorControllerUpdateTests
    {
        private AuthorController _authorController;
        private Mock<IAuthorManager> _authorManager;
        private Fixture _fixture;
        private ClaimsPrincipal _user;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppMapper());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            _authorManager = new Mock<IAuthorManager>();
            _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.Name, "user")
                                   }, "TestAuthentication"));
            _authorController = new AuthorController(_authorManager.Object, mapper)
            {
                ControllerContext = new ControllerContext()
            };
            _authorController.ControllerContext.HttpContext = new DefaultHttpContext { User = _user };
        }

        [Test]
        public void UpdateAuthor_CheckAttributes()
        {
            var testControllerType = typeof(AuthorController);
            testControllerType.GetTypeInfo()
                .GetMethod("Update")
                .GetCustomAttribute<RouteAttribute>()
                .Should()
                .BeNull();

            testControllerType.GetTypeInfo()
                .GetMethod("Update")
                .GetCustomAttribute<HttpPutAttribute>()
                .Should()
                .NotBeNull();

            testControllerType.GetTypeInfo()
             .GetMethod("Update")
             .GetCustomAttribute<HttpPutAttribute>()
             .Template
             .Should()
             .Be("{id}");

            testControllerType.GetTypeInfo()
              .GetMethod("Update")
              .GetCustomAttribute<AuthorizeAttribute>()
              .Should()
              .NotBeNull();

            testControllerType.GetTypeInfo()
             .GetMethod("Update")
             .GetCustomAttribute<AuthorizeAttribute>()
             .Roles
             .Should()
             .Be("admin");
        }

        [Test]
        public async Task UpdateAuthor_AuthorsManagerError_ShouldReturnBadRequest()
        {
            // arrange 
            var erroMessage = _fixture.Create<string>();
            _authorManager.Setup(a => a.Update(It.IsAny<AuthorUpdateDto>(), It.IsAny<string>())).ReturnsAsync(new ResultData(erroMessage));

            // act 
            var result = await _authorController.Update(Guid.NewGuid(), new AuthorUpdateRequest());

            // assert 
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be(erroMessage);
            _authorManager.Verify(a => a.Update(It.IsAny<AuthorUpdateDto>(), It.IsAny<string>()), Times.Once);
        }


        [Test]
        public async Task UpdateAuthor_AuthorsManagerSuccess_ShouldReturnOkRequest()
        {
            // arrange 
            var author = _fixture.Create<AuthorUpdateRequest>();
            var id = _fixture.Create<Guid>();
            _authorManager.Setup(a => a.Update(It.IsAny<AuthorUpdateDto>(), It.IsAny<string>())).ReturnsAsync(new ResultData());

            // act 
            var result = await _authorController.Update(id, author);

            // assert 
            var okResponse = result as OkResult;
            okResponse.Should().NotBeNull();

            _authorManager.Verify(a => a.Update(It.IsAny<AuthorUpdateDto>(), _user.Identity.Name), Times.Once);
            _authorManager.Verify(a => a.Update(It.Is<AuthorUpdateDto>(a => a.FirstName == author.FirstName), It.IsAny<string>()), Times.Once);
            _authorManager.Verify(a => a.Update(It.Is<AuthorUpdateDto>(a => a.LastName == author.LastName), It.IsAny<string>()), Times.Once);
        }
    }
}
