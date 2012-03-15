using System.Collections.Generic;
using System.Data.Entity;
using Ninject;

namespace efsession
{
    public class EntityFrameworkContext : DbContext
    {
        [Inject] protected virtual IEnumerable<IConfigureModelBuilder> ModelBuilderConfigurations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelBuilderConfigurations.ForEach(c => c.Configure(modelBuilder));
        }
    }
}