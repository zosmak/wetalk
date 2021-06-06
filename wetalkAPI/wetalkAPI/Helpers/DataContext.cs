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

        public DbSet<User> Users { get; set; }
    }
}