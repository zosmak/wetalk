using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WetalkAPI.Entities;

namespace WetalkAPI.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("WetalkDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.Permission)
                .WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserPermission>().HasData(
                new UserPermission
                {
                    ID = 1,
                    Description = "Admin",
                },
                new UserPermission
                {
                    ID = 2,
                    Description = "User",
                }
            );
        }


        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
    }
}