using AuthorApp.ApiContracts.Response;
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AuthorApp.ApiTests.ControllerTests
{
    [TestFixture]
    public class AuthorControllerGetAllTests
    {
        private AuthorController _authorController;
        private Mock<IAuthorManager> _authorManager;

        [SetUp]
        public void SetUp()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppMapper());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            _authorManager = new Mock<IAuthorManager>();
            _authorController = new AuthorController(_authorManager.Object, mapper);
        }

        [Test]
        public void GetAuthors_CheckAttributes()
        {
            var testControllerType = typeof(AuthorController);
            testControllerType.GetTypeInfo()
                .GetMethod("GetAuthors")
                .GetCustomAttribute<RouteAttribute>()
                .Should()
                .BeNull();

            testControllerType.GetTypeInfo()
               .GetMethod("GetAuthors")
               .GetCustomAttribute<HttpGetAttribute>()
               .Should()
               .NotBeNull();

            testControllerType.GetTypeInfo()
             .GetMethod("GetAuthors")
             .GetCustomAttribute<HttpGetAttribute>()
             .Template
             .Should()
             .BeNull();

            testControllerType.GetTypeInfo()
               .GetMethod("GetAuthors")
              .GetCustomAttribute<AuthorizeAttribute>()
              .Should()
              .BeNull();
        }

        [Test]
        public async Task GetAuthors_AuthorsManagerError_ShouldReturnBadRequest()
        {
            // arrange 
            _authorManager.Setup(a => a.GetAuthors()).ReturnsAsync(new ResultData<List<AuthorDto>>("error message"));

            // act 
            var result = await _authorController.GetAuthors();

            // assert 
            var badRequest = result as BadRequestResult;
            badRequest.Should().NotBeNull();
            _authorManager.Verify(a => a.GetAuthors(), Times.Once);
        }


        [Test]
        public async Task GetAuthors_AuthorsManagerSuccess_ShouldReturnOkRequest()
        {
            // arrange 
            Fixture fixture = new Fixture();
            var authors = fixture.CreateMany<AuthorDto>().ToList();
            _authorManager.Setup(a => a.GetAuthors()).ReturnsAsync(new ResultData<List<AuthorDto>>(authors));

            // act 
            var result = await _authorController.GetAuthors();

            // assert 
            var okResponse = result as OkObjectResult;
            okResponse.Should().NotBeNull();
            okResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            var authorsResponse = okResponse.Value as ResponseData<AuthorResponse>;
            authorsResponse.Total.Should().Be(authors.Count);
            authorsResponse.Data.All(a => !string.IsNullOrEmpty(a.FirstName)).Should().BeTrue();
            authorsResponse.Data.All(a => !string.IsNullOrEmpty(a.LastName)).Should().BeTrue();
            authorsResponse.Data.All(a => a.Id != Guid.Empty).Should().BeTrue();

            _authorManager.Verify(a => a.GetAuthors(), Times.Once);
        }
    }
}
