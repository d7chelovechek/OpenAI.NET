using Microsoft.EntityFrameworkCore;
using OpenAI.NET.Web.Services;
using OpenAI.NET.Web.EntityFrameworkCore.Models;
using System;

namespace OpenAI.NET.Web.EntityFrameworkCore
{
    public class ApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            SQLitePCL.Batteries_V2.Init();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(e => e.Permissions).HasConversion(
                x => string.Join(',', x),
                x => x.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Ayrat",
                PasswordHash = Sha256.GetHash("p455w9"),
                Permissions = new string[] { Permission.All },
                TokenLifeTime = TimeSpan.FromDays(365)
            });
        }
    }
}