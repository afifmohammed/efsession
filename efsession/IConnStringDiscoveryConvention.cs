using System;
using System.Data.Entity;

namespace efsession
{
    public interface IConnStringDiscoveryConvention : IConnStringDiscoveryConvention<EntityFrameworkContext> {}

    public interface IConnStringDiscoveryConvention<TContext> where TContext : DbContext
    {
        Func<string, bool> IsConnectionString { get; }
    }
}