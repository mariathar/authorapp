using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.DataLayer.Entities
{
    [Table("Users", Schema = "user")]
    public class User
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string Role { get; set; }
    }
}
