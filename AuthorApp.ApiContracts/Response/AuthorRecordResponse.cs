using System;

namespace AuthorApp.ApiContracts.Response
{
    public class AuthorRecordResponse
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Автор записи
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime DateAdded { get; set; }
    }
}
