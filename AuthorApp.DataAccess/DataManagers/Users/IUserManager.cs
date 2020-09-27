using AuthorApp.DataAccessContracts.Models;
using System.Threading.Tasks;

namespace AuthorApp.DataAccess.DataManagers.Users
{
    public interface IUserManager
    {
        /// <summary>
        /// Есть ли такой пользователь
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        Task<ResultData<UserDto>> GetUser(string login, string password);
    }
}
