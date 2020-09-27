using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorApp.ApiContracts.Response
{
    public class AuthorResponse
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
    }
}
