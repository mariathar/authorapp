using System;

namespace AuthorApp.DataAccessContracts.Models
{
    public class AuthorCreateDto
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

    }
}
