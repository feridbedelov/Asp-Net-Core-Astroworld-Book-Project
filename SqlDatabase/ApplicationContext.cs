using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlDatabase
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {
            
        }


        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<WishList> WishLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            foreach (var foreignKey in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<WishList>()
                .HasKey(bc => new { bc.BookId, bc.UserId });
            builder.Entity<WishList>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.WishLists)
                .HasForeignKey(bc => bc.BookId);
            builder.Entity<WishList>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.WishLists)
                .HasForeignKey(bc => bc.UserId);




        }




    }
}
