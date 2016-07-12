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

        public DbSet<Signup> Logins { get; set; }
        public DbSet<Chat> chat { get; set; }
        public DbSet<Auctions> Auctions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auctions>()
              .HasOne(p => p.Signup)
              .WithMany(b => b.Auction);

            base.OnModelCreating(modelBuilder);
        }

        //Potrzebne do WebSocketa bo on nie przechodzi przez ta czesc Pipeline najwidoczniej :(
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:projektgrupowy2.database.windows.net,1433;Data Source=projektgrupowy2.database.windows.net;Initial Catalog=projektgrupowy;Persist Security Info=False;User ID=patryk;Password=PPPaaa333!!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        }
    }
}
