using AuthApp.DataContext;
using AuthorApp.DataAccess.DataManagers.Authors;
using AuthorApp.DataAccess.DataManagers.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorApp.Conteiners
{
    public static class ContainerSetup
    {
        /// <summary>
        /// Регистрация служб и сейвисов в DI
        /// </summary>
        /// <param name="services">Сервисы</param>
        /// <param name="configuration">Конфигурация</param>
        public static void RegistrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddServices(configuration);
            services.ReigstrationUserPostgreSqlServices(configuration);
        }

        private static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IAuthorManager, AuthorManager>();
        }

        public static void ReigstrationUserPostgreSqlServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAppContextFactory>(ct => new MSSQLApplicationContextFactory(configuration.GetSection("Data:MSSql:ConnectionString").Value));
            services.AddDbContext<IApplicationContext, MSSQlContext>(options =>
                options
                    .UseSqlServer(configuration.GetSection("Data:MSSql:ConnectionString").Value));
        }
    }
}
