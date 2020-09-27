using AuthApp.DataContext;
using AuthApp.DataLayer.Entities;
using AuthorApp.DataAccessContracts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorApp.DataAccess.DataManagers.Authors
{
    public class AuthorManager: IAuthorManager
    {
        private readonly IAppContextFactory _contextFactory;

        public AuthorManager(IAppContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ResultData<List<AuthorDto>>> GetAuthors()
        {
            using IApplicationContext context = _contextFactory.GetContext();
            var authors = await context.Set<Author>()
                .Include(a => a.User)
                .Select(a => new AuthorDto
            {
                Id = a.Id,
                DateAdded = a.DateAdded,
                FirstName = a.FirstName,
                LastName = a.LastName,
                User = a.User.Login
            }).ToListAsync();

            return new ResultData<List<AuthorDto>>(authors);
        }

        public async Task<ResultData<AuthorDto>> GetAuthorById(Guid id)
        {
            using IApplicationContext context = _contextFactory.GetContext();
            var author = await context.Set<Author>()
                .Where(a => a.Id == id)
                .Include(a => a.User)
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    DateAdded = a.DateAdded,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    User = a.User.Login
                }).FirstOrDefaultAsync();

            if (author == null)
                return new ResultData<AuthorDto>("Author not found");

            return new ResultData<AuthorDto>(author);
        }

        public async Task<ResultData<Guid>> Create(AuthorCreateDto author, string userLogin)
        {
            //validation
            if(string.IsNullOrEmpty(author.FirstName) || string.IsNullOrEmpty(author.LastName))
                return new ResultData<Guid>("Author is invalid");

            //create
            using IApplicationContext context = _contextFactory.GetContext();
            var existAuthor = await context.Set<Author>().FirstOrDefaultAsync(a => a.LastName == author.LastName && a.FirstName == author.FirstName);
            if(existAuthor != null)
                return new ResultData<Guid>(existAuthor.Id);

            var user = await context.Set<User>().FirstOrDefaultAsync(u => u.Login == userLogin);
            if (user == null)
                return new ResultData<Guid>("User not found");

            var authorId = Guid.NewGuid();
            await context.Set<Author>()
               .AddAsync(new Author
               {
                   Id = authorId,
                   DateAdded = DateTime.Now,
                   FirstName = author.FirstName,
                   LastName = author.LastName,
                   UserId = user.Id
               });

            context.SaveChanges();
            return new ResultData<Guid>(authorId);
        }

        public async Task<ResultData> Update(AuthorUpdateDto author, string userLogin)
        {
            //validation
            if (string.IsNullOrEmpty(author.FirstName) || string.IsNullOrEmpty(author.LastName))
                return new ResultData("Author is invalid");

            //update
            using IApplicationContext context = _contextFactory.GetContext();
            var existAuthor = await context.Set<Author>().FirstOrDefaultAsync(a => a.LastName == author.LastName && a.FirstName == author.FirstName);
            if (existAuthor == null)
                return new ResultData("Author not exist");

            var user = await context.Set<User>().FirstOrDefaultAsync(u => u.Login == userLogin);
            if (user == null)
                return new ResultData("User not found");

            existAuthor.LastName = author.LastName;
            existAuthor.FirstName = author.FirstName;
            context.SaveChanges();
            return new ResultData();
        }

        public async Task<ResultData> Delete(Guid authorId, string userLogin)
        {
            //validation
            using IApplicationContext context = _contextFactory.GetContext();
            var existAuthor = await context.Set<Author>().FirstOrDefaultAsync(a => a.Id == authorId);
            if (existAuthor == null)
                return new ResultData("Author not exist");

            //delete
            context.Set<Book>().RemoveRange(context.Set<Book>().Where(b => b.AuthorId == authorId));
            context.Set<Author>().Remove(existAuthor);
            context.SaveChanges();
            return new ResultData();
        }
    }
}
