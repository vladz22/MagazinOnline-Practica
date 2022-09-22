using MagazinOnline.Modele;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagazinOnline.DataAcces.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
         public DbSet<Categorie> Categoriile { set; get; }
         public DbSet<Produs> Produse { set; get; }
         public DbSet<ApplicationUser> ApplicationUsers { get; set; }
         public DbSet<Companie> Companii { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
