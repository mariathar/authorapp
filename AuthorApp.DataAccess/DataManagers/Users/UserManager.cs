using AuthApp.DataContext;
using AuthApp.DataLayer.Entities;
using AuthorApp.DataAccessContracts.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuthorApp.DataAccess.DataManagers.Users
{
    public class UserManager: IUserManager
    {
        private readonly IAppContextFactory _contextFactory;

        public UserManager(IAppContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ResultData<UserDto>> GetUser(string login, string password)
        {
            using IApplicationContext context = _contextFactory.GetContext();
            var user = await context.Set<User>().FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
            if (user == null)
                return new ResultData<UserDto>("Not found");
            return new ResultData<UserDto>(new UserDto { Id = user.Id, Login = user.Login, Role = user.Role });
        }
    }
}
