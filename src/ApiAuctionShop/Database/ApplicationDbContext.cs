using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Projekt.Controllers;
using ApiAuctionShop.Models;

namespace ApiAuctionShop.Database
{
    public class ApplicationDbContext : IdentityDbContext<Signup>
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Signup> Logins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
