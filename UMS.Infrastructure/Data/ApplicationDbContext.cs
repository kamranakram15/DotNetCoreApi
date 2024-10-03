using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain.Entities;

namespace UMS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // Parameterless constructor for design-time support
        public ApplicationDbContext() : base(new DbContextOptions<ApplicationDbContext>())
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.LoginHistories)
                .WithOne(lh => lh.User)
                .HasForeignKey(lh => lh.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TransactionHistories)
                .WithOne(th => th.User)
                .HasForeignKey(th => th.UserId);

         
            modelBuilder.Entity<User>()
                .Property(u => u.Balance)
                .HasColumnType("decimal(18, 2)"); 

            modelBuilder.Entity<TransactionHistory>()
                .Property(th => th.TransactionAmount)
                .HasColumnType("decimal(18, 2)");
        }



    }
}
