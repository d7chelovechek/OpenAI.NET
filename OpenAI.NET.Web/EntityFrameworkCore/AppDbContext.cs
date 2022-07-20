using Microsoft.EntityFrameworkCore;
using System;
using OpenAI.NET.Web.EntityFrameworkCore.Entities;
using OpenAI.NET.Web.Cryptography;
using OpenAI.NET.Models.Web;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OpenAI.NET.Web.EntityFrameworkCore
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// All users in database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// A constructor that initializes database.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            SQLitePCL.Batteries_V2.Init();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(e => e.Name);

            modelBuilder.Entity<User>().Property(e => e.Permissions).HasConversion(
                x => string.Join(',', x),
                x => x.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<User>().HasData(new User()
            {
                Name = "Ayrat",
                PasswordHash = Sha256.GetHash("p455w9"),
                Permissions = new string[] { Permission.All },
                TokenLifeTime = TimeSpan.FromDays(365)
            });
        }
    }
}