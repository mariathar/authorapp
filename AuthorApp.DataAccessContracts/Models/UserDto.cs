using System;

namespace AuthorApp.DataAccessContracts.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string Role { get; set; }
    }
}
