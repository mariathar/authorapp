using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace AuthApp.DataContext
{
    public interface IApplicationContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;

        DatabaseFacade Database { get; }

        int SaveChanges();
    }
}
