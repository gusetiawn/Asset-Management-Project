using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public DbSet<Item> Items { get; set; }
        public DbSet<RequestItem> RequestItems { get; set; }
        public DbSet<ReturnItem> ReturnItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleAccount> RoleAccounts { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
