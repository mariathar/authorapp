using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AuthApp.DataContext
{
    public class MSSQLApplicationContextFactory : IAppContextFactory
    {
        private string mBaseString;

        public MSSQLApplicationContextFactory(string baseString = null)
        {
            mBaseString = baseString;
        }

        public NetworkCredential Credential { get; set; }

        public IApplicationContext GetContext(string baseString = null)
        {
            var basestr = baseString ?? mBaseString;
            if (string.IsNullOrEmpty(basestr))
                return null;

            var optionsBuilder = new DbContextOptionsBuilder<MSSQlContext>();
            optionsBuilder.UseSqlServer(basestr);
            return (IApplicationContext)new MSSQlContext(optionsBuilder.Options);
        }
    }
}
