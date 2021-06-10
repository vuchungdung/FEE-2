using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FEE.Models
{
    public class FEEDbContext : DbContext
    {
        public FEEDbContext() : base("FEEDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<WebInfo> WebInfos { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<DepartmentInMenu> DepartmentInMenus { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<CommandInFunction> CommandInFunctions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Partner> Partners { get; set; }

        public static FEEDbContext Create()
        {
            return new FEEDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}