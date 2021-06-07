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
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageRead>()
                .HasOne(a => a.User)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageRead>()
                .HasOne(a => a.Message)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMember>()
               .HasOne(a => a.User)
               .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMember>()
                .HasOne(a => a.Chat)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<MessageRead>().HasKey(l => new { l.UserID, l.MessageID });
            modelBuilder.Entity<ChatMember>().HasKey(l => new { l.UserID, l.ChatID });
        }


        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageRead> MessagesReads { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMember> ChatMembers { get; set; }
    }
}