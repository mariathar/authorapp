using AuthorApp.ApiContracts.Response;
using AuthorApp.Controllers;
using AuthorApp.DataAccess.DataManagers.Authors;
using AuthorApp.DataAccessContracts.Models;
using AuthorApp.Mapper;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AuthorApp.ApiTests.ControllerTests
{
    [TestFixture]
    public class AuthorControllerGetAuthorTests
    {
        private AuthorController _authorController;
        private Mock<IAuthorManager> _authorManager;
        private Fixture _fixture;

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
            _authorController = new AuthorController(_authorManager.Object, mapper);
        }

        [Test]
        public void GetAuthor_CheckAttributes()
        {
            var testControllerType = typeof(AuthorController);
            testControllerType.GetTypeInfo()
                .GetMethod("GetAuthor")
                .GetCustomAttribute<RouteAttribute>()
                .Should()
                .BeNull();

            testControllerType.GetTypeInfo()
               .GetMethod("GetAuthor")
               .GetCustomAttribute<HttpGetAttribute>()
               .Should()
               .NotBeNull();

            testControllerType.GetTypeInfo()
             .GetMethod("GetAuthor")
             .GetCustomAttribute<HttpGetAttribute>()
             .Template
             .Should()
             .Be("{id}");

            testControllerType.GetTypeInfo()
              .GetMethod("GetAuthor")
              .GetCustomAttribute<AuthorizeAttribute>()
              .Should()
              .BeNull();
        }

        [Test]
        public async Task GetAuthor_AuthorsManagerError_ShouldReturnBadRequest()
        {
            // arrange 
            var erroMessage = _fixture.Create<string>();
            _authorManager.Setup(a => a.GetAuthorById(It.IsAny<Guid>())).ReturnsAsync(new ResultData<AuthorDto>(erroMessage));

            // act 
            var result = await _authorController.GetAuthor(Guid.NewGuid());

            // assert 
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be(erroMessage);
            _authorManager.Verify(a => a.GetAuthorById(It.IsAny<Guid>()), Times.Once);
        }


        [Test]
        public async Task GetAuthor_AuthorsManagerSuccess_ShouldReturnOkRequest()
        {
            // arrange 
            var author = _fixture.Create<AuthorDto>();
            _authorManager.Setup(a => a.GetAuthorById(author.Id)).ReturnsAsync(new ResultData<AuthorDto>(author));

            // act 
            var result = await _authorController.GetAuthor(author.Id);

            // assert 
            var okResponse = result as OkObjectResult;
            okResponse.Should().NotBeNull();
            var authorsResponse = okResponse.Value as AuthorRecordResponse;
            authorsResponse.FirstName.Should().Be(author.FirstName);
            authorsResponse.LastName.Should().Be(author.LastName);
            authorsResponse.User.Should().Be(author.User);
            authorsResponse.Id.Should().Be(author.Id);
            authorsResponse.DateAdded.Should().Be(author.DateAdded);

            _authorManager.Verify(a => a.GetAuthorById(author.Id), Times.Once);
        }
    }
}
