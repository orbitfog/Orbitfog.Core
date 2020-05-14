using Microsoft.EntityFrameworkCore;

namespace Orbitfog.Core.Database.Mapper.PerformanceTestCli
{
    public class EfDbContext : DbContext
    {
        public virtual DbSet<Test1> Test1 { get; set; }

        public EfDbContext() :
            base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(SqlQuery.ConnectionString);
            }
        }
    }
}
