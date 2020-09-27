using System;
using System.Collections.Generic;
using System.Text;

namespace AuthApp.DataContext
{
    public interface IAppContextFactory
    {
        IApplicationContext GetContext(string baseString = null);
    }
}
