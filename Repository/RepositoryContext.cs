using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository
{
    public class RepositoryContext: DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CompanyConfiguration());
            builder.ApplyConfiguration(new EmployeeConfiguration());
        }
        public DbSet<Company>? Companies { get; set; }
        public DbSet<Employee>? Employees { get; set; }

    }
}