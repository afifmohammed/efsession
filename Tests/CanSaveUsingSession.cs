using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Configoo;
using NUnit.Framework;
using Ninject;
using efsession;

namespace Tests
{
    [Serializable]
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class ConfigureCustomer : IConfigureModelBuilder
    {
        public void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(x => x.Id);
            modelBuilder.Entity<Customer>().Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    [TestFixture]
    public class CanSaveUsingSession
    {
        [Test]
        public void ShouldSaveObjectToDatabase()
        {
            int age;
            using(var k = new StandardKernel())
            {
                k.Load<Configooness>();
                k.Load<EfSession>();
                Configured.Value.For("EntityFrameworkContext",
                                     new ConnectionStringSettings(name: "EntityFrameworkContext",
                                                                  connectionString: Factory.BaseConnectionString,
                                                                  providerName: "System.Data.SqlServerCe.4.0"));
                var session = k.Get<ISession>();
                session.Add(new Customer { Age = 23, Name = "Jill"});
                session.Commit();

                age = session.Query<Customer>().First().Age;
            }

            Assert.AreEqual(23, age);
        }

        private SqlCeConnectionFactory Factory
        {
            get
            {
                return new SqlCeConnectionFactory(
                providerInvariantName: "System.Data.SqlServerCe.4.0",
                databaseDirectory: "",
                baseConnectionString: @"Data Source=|DataDirectory|\test.sdf");
            }
        }

        [SetUp]
        public void Prepare()
        {
            Database.DefaultConnectionFactory = Factory;

            Database.SetInitializer(new DropCreateDatabaseAlways<EntityFrameworkContext>());
        }
    }
}
