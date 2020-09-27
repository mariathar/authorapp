using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.DataLayer.Entities
{
    [Table("Authors", Schema = "author")]
    public class Author
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string LastName { get; set; }

        /// <summary>
        /// Автор записи
        /// </summary>
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        [Required]
        public DateTime DateAdded { get; set; }

        #region links

        public User User { get; set; }

        #endregion
    }
}
