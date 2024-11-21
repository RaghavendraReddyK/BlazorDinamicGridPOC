using DynamicGridGeneration.Model;
using Microsoft.EntityFrameworkCore;

namespace DynamicGridGeneration.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext _DbContext;
        public ApplicationDBContext(DbContextOptions Dbcontext) : base(Dbcontext)
        {
        }

        public DbSet<Employee> Employees {get; set;}
        public DbSet<ColumnVisibility> ColumnVisibility {get; set;}
    }
}
