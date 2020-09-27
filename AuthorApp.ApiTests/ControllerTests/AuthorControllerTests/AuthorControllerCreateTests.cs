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
    public class AuthorControllerCreateTests
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
        public void CreateAuthor_CheckAttributes()
        {
            var testControllerType = typeof(AuthorController);
            testControllerType.GetTypeInfo()
                .GetMethod("Create")
                .GetCustomAttribute<RouteAttribute>()
                .Should()
                .BeNull();

            testControllerType.GetTypeInfo()
             .GetMethod("Create")
             .GetCustomAttribute<HttpPostAttribute>()
             .Template
             .Should()
             .BeNull();

            testControllerType.GetTypeInfo()
              .GetMethod("Create")
              .GetCustomAttribute<AuthorizeAttribute>()
              .Should()
              .NotBeNull();

            testControllerType.GetTypeInfo()
             .GetMethod("Create")
             .GetCustomAttribute<AuthorizeAttribute>()
             .Roles
             .Should()
             .Be("admin");
        }

        [Test]
        public async Task CreateAuthor_AuthorsManagerError_ShouldReturnBadRequest()
        {
            // arrange 
            var erroMessage = _fixture.Create<string>();
            _authorManager.Setup(a => a.Create(It.IsAny<AuthorCreateDto>(), It.IsAny<string>())).ReturnsAsync(new ResultData<Guid>(erroMessage));

            // act 
            var result = await _authorController.Create(new AuthorCreateRequest());

            // assert 
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be(erroMessage);
            _authorManager.Verify(a => a.Create(It.IsAny<AuthorCreateDto>(), It.IsAny<string>()), Times.Once);
        }


        [Test]
        public async Task CreateAuthor_AuthorsManagerSuccess_ShouldReturnOkRequest()
        {
            // arrange 
            var author = _fixture.Create<AuthorCreateRequest>();
            var id = _fixture.Create<Guid>();
            _authorManager.Setup(a => a.Create(It.IsAny<AuthorCreateDto>(), It.IsAny<string>())).ReturnsAsync(new ResultData<Guid>(id));

            // act 
            var result = await _authorController.Create(author);

            // assert 
            var okResponse = result as OkObjectResult;
            okResponse.Should().NotBeNull();
            var authorsResponse = okResponse.Value as Guid?;
            authorsResponse.Should().Be(id);

            _authorManager.Verify(a => a.Create(It.IsAny<AuthorCreateDto>(), _user.Identity.Name), Times.Once);
            _authorManager.Verify(a => a.Create(It.Is<AuthorCreateDto>(a => a.FirstName == author.FirstName), It.IsAny<string>()), Times.Once);
            _authorManager.Verify(a => a.Create(It.Is<AuthorCreateDto>(a => a.LastName == author.LastName), It.IsAny<string>()), Times.Once);
        }
    }
}
