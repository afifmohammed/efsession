using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using Configoo;
using Ninject;

namespace efsession
{
    public class EntityFrameworkContext : DbContext
    {
        private EntityFrameworkContext(IConnStringDiscoveryConvention<EntityFrameworkContext> convention)
            : base(Configured.Value.For<ConnectionStringSettings>(x => convention.IsConnectionString(x)).Name)
        {}

        [Inject] protected virtual IEnumerable<IConfigureModelBuilder<EntityFrameworkContext>> ModelBuilderConfigurations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelBuilderConfigurations.ForEach(c => c.Configure(modelBuilder));
        }
    }
}