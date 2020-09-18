using DAL.Models;
using DAL.Util;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>()
                .HasOne(p => p.Project)
                .WithMany(b => b.Tests)
                .HasForeignKey(p => p.ProjectId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"server={ReadConfig.GetParam("configDb", "server")};port={ReadConfig.GetParam("configDb", "port")};UserId={ReadConfig.GetParam("configDb", "userId")};Password={ReadConfig.GetParam("configDb", "password")};database={ReadConfig.GetParam("configDb", "database")};");
        }
    }
}
