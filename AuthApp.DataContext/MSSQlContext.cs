using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthApp.DataContext
{
    public class MSSQlContext : ApplicationContext, IApplicationContext
    {
        public MSSQlContext(DbContextOptions options)
         : base(options)
        {

        }
    }
}
