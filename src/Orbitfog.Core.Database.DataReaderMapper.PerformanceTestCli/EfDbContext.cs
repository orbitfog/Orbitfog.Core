using Microsoft.EntityFrameworkCore;

namespace Orbitfog.Core.Database.DataReaderMapper.PerformanceTestCli
{
    public class EfDbContext : DbContext
    {
        public virtual DbSet<Test1> Test1 { get; set; } = null!;

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
