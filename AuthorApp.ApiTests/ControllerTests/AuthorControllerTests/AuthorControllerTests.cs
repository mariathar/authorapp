using AuthorApp.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Reflection;

namespace AuthorApp.ApiTests.ControllerTests
{
    [TestFixture]
    public class AuthorControllerAttributeTests
    {

        [Test]
        public void AuthorController_CheckAttributes()
        {
            var testControllerType = typeof(AuthorController);
            var h = testControllerType.GetTypeInfo()
                .GetCustomAttribute<RouteAttribute>();

            testControllerType.GetTypeInfo()
                .GetCustomAttribute<Microsoft.AspNetCore.Mvc.RouteAttribute>()
                .Should()
                .NotBeNull();

            testControllerType.GetTypeInfo()
              .GetCustomAttribute<RouteAttribute>()
              .Template
              .Should()
              .Be("api/authors");

            testControllerType.GetTypeInfo()
               .GetCustomAttribute<AuthorizeAttribute>()
               .Should()
               .NotBeNull();

            testControllerType.GetTypeInfo()
             .GetCustomAttribute<AuthorizeAttribute>()
             .Roles
             .Should()
             .BeNull();
        }
    }
}
