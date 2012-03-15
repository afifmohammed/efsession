using System;
using System.Collections.Generic;
using System.Data.Entity;
using Configoo;
using Ninject;

namespace efsession
{
    public class DefualtConnectionStringConvention<TContext> : IConnStringDiscoveryConvention<TContext> where TContext : DbContext
    {
        public Func<string, bool> IsConnectionString
        {
            get { return x => x == typeof (TContext).Name; }
        }
    }

    public interface IConnStringDiscoveryConvention : IConnStringDiscoveryConvention<EntityFrameworkContext> {}

    public interface IConnStringDiscoveryConvention<TContext> where TContext : DbContext
    {
        Func<string, bool> IsConnectionString { get; }
    }

    public class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext(IConnStringDiscoveryConvention<EntityFrameworkContext> convention)
            : base(Configured.Value.For<string>(x => convention.IsConnectionString(x)))
        {}

        [Inject] protected virtual IEnumerable<IConfigureModelBuilder<EntityFrameworkContext>> ModelBuilderConfigurations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelBuilderConfigurations.ForEach(c => c.Configure(modelBuilder));
        }
    }
}