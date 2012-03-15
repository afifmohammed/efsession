using System;
using System.Data.Entity;

namespace efsession
{
    public class DefualtConnectionStringConvention<TContext> : IConnStringDiscoveryConvention<TContext> where TContext : DbContext
    {
        public Func<string, bool> IsConnectionString
        {
            get { return x => x == typeof (TContext).Name; }
        }
    }
}