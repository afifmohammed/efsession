using System.Data.Entity;

namespace efsession
{
    public interface IConfigureModelBuilder<TContext> where TContext : DbContext
    {
        void Configure(DbModelBuilder modelBuilder);
    }

    public interface IConfigureModelBuilder : IConfigureModelBuilder<EntityFrameworkContext>
    {}
}