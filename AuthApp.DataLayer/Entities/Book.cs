using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.DataLayer.Entities
{
    /// <summary>
    /// Книга
    /// </summary>
    [Table("Books")]
    public class Book
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя книги
        /// </summary>
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Дата добабвления записи
        /// </summary>
        [Required]
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [Required]
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        [ForeignKey("Author")]
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Пользователь, кто добавил запись
        /// </summary>
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        #region links
        public Author Author { get; set; }

        public User User { get; set; }

        #endregion
    }
}
