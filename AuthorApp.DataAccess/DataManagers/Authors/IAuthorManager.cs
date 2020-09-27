using AuthorApp.DataAccessContracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorApp.DataAccess.DataManagers.Authors
{
    public interface IAuthorManager
    {
        /// <summary>
        /// Получения списка авторов
        /// </summary>
        /// <returns></returns>
        Task<ResultData<List<AuthorDto>>> GetAuthors();

        /// <summary>
        /// Получение записи автора по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultData<AuthorDto>> GetAuthorById(Guid id);

        /// <summary>
        /// Создание записи автора
        /// </summary>
        /// <param name="author"></param>
        /// <param name="userLogin">Логин пользователя</param>
        /// <returns></returns>
        Task<ResultData<Guid>> Create(AuthorCreateDto author, string userLogin);

        /// <summary>
        /// Редактирование записи автора
        /// </summary>
        /// <param name="author"></param>
        /// <param name="userLogin">Логин пользователя</param>
        /// <returns></returns>
        Task<ResultData> Update(AuthorUpdateDto author, string userLogin);

        /// <summary>
        /// Удаление записи автора
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="userLogin">Логин пользователя</param>
        /// <returns></returns>
        Task<ResultData> Delete(Guid authorId, string userLogin);
    }
}
