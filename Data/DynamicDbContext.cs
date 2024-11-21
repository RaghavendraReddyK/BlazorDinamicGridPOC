//using Microsoft.EntityFrameworkCore;

//namespace DynamicGridGeneration.Data
//{
//    public class DynamicDbContext : DbContext
//    {
//        public DynamicDbContext(DbContextOptions<DynamicDbContext> options)
//            : base(options)
//        {
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//                optionsBuilder.UseSqlServer("DefaultConnectionString");
//            }
//        }
//    }
//}
