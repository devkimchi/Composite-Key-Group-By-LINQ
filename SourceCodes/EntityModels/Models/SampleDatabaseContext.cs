using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EntityModels.Models.Mapping;

namespace EntityModels.Models
{
    public partial class SampleDatabaseContext : DbContext
    {
        static SampleDatabaseContext()
        {
            Database.SetInitializer<SampleDatabaseContext>(null);
        }

        public SampleDatabaseContext()
            : base("Name=SampleDatabaseContext")
        {
        }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CarMap());
        }
    }
}
