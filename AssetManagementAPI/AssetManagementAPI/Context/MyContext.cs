using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<RequestItem> RequestItems { get; set; }
        public DbSet<ReturnItem> ReturnItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleAccount> RoleAccounts { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.Account)
                .WithOne(b => b.User)
                .HasForeignKey<Account>(b => b.Id);

            modelBuilder.Entity<Department>()
                .HasMany(a => a.Users)
                .WithOne(b => b.Department);

            modelBuilder.Entity<Gender>()
                .HasMany(a => a.Users)
                .WithOne(b => b.Gender);

            modelBuilder.Entity<RoleAccount>()
                .HasKey(ar => new { ar.AccountId, ar.RoleId });
            modelBuilder.Entity<RoleAccount>()
                .HasOne(ar => ar.Account)
                .WithMany(a => a.RoleAccounts)
                .HasForeignKey(ar => ar.AccountId);
            modelBuilder.Entity<RoleAccount>()
                .HasOne(ar => ar.Role)
                .WithMany(r => r.RoleAccounts)
                .HasForeignKey(ar => ar.RoleId);

            //modelBuilder.Entity<RequestItem>()
                //.HasKey(ri => new { ri.AccountId, ri.ItemId });
            modelBuilder.Entity<RequestItem>()
                .HasOne(a => a.Account)
                .WithMany(ri => ri.RequestItems)
                .HasForeignKey(a => a.AccountId);
            modelBuilder.Entity<RequestItem>()
                .HasOne(i => i.Item)
                .WithMany(r => r.RequestItems)
                .HasForeignKey(i => i.ItemId);

            modelBuilder.Entity<Status>()
                .HasMany(a => a.RequestItems)
                .WithOne(b => b.Status);

            modelBuilder.Entity<Category>()
                .HasMany(a => a.Items)
                .WithOne(b => b.Category);

            modelBuilder.Entity<ReturnItem>()
                .HasOne(a => a.RequestItem)
                .WithOne(b => b.ReturnItem)
                .HasForeignKey<ReturnItem>(b => b.RequestItemId);

        }
    }

}
