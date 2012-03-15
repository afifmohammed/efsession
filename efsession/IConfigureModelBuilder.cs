using System.Data.Entity;

namespace efsession
{
    public interface IConfigureModelBuilder
    {
        void Configure(DbModelBuilder modelBuilder);
    }
}