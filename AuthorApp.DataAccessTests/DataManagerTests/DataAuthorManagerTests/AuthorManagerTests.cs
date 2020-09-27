using AuthApp.DataContext;
using AuthApp.DataLayer.Entities;
using AuthorApp.DataAccess.DataManagers.Authors;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable.Moq;

namespace AuthorApp.DataAccessTests.DataManagerTests.DataAuthorManagerTests
{
    [TestFixture]
    public class AuthorManagerTests
    {
        private AuthorManager _authorManager;
        private Mock<IAppContextFactory> _appContextFactory;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _appContextFactory = new Mock<IAppContextFactory>();
            _authorManager = new AuthorManager(_appContextFactory.Object);       
        }

        [Test]
        public async Task GetAuthors_AuthorExist_ShouldReturnListWithData()
        {
            // arrange 
            var authors = _fixture.Build<Author>().With(a => a.User, new User { }).CreateMany();
            var context = new Mock<IApplicationContext>();
            var mock = authors.AsQueryable().BuildMockDbSet();
            context.Setup(d => d.Set<Author>()).Returns(mock.Object);
            _appContextFactory.Setup(a => a.GetContext(null)).Returns(context.Object);

            // act 
            var result = await _authorManager.GetAuthors();

            // assert 
            result.Should().NotBeNull();
            result.ErrorMessage.Should().BeNull();
            result.IsSuccess.Should().BeTrue();
            context.Verify(a => a.Set<Author>(), Times.Once);
        }
    }
}
