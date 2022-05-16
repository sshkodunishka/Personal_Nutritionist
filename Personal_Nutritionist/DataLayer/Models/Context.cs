using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Nutritionist.DataLayer
{
    public class Context : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Server=адрес_сервера/localhost;Database=имя_базы_данных;User Id=логин;Password=пароль;
            //optionsBuilder.UseSqlServer(@"Server=KRISTINAS\SQLEXPRESS;Database=food;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=food;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<RecipeProduct>()
                .HasOne(bc => bc.Recipe)
                .WithMany(b => b.RecipeProducts)
                .HasForeignKey(bc => bc.ProductId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipeProduct>()
                .HasOne(bc => bc.Product)
                .WithMany(c => c.RecipeProducts)
                .HasForeignKey(bc => bc.ProductId).OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Favorites>()
                .HasOne(bc => bc.Recipe)
                .WithMany(b => b.Favorites)
                .HasForeignKey(bc => bc.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorites>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.Favorites)
                .HasForeignKey(bc => bc.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MealFood>()
                .HasOne(bc => bc.Recipe)
                .WithMany(b => b.MealFoods).IsRequired(false)
                .HasForeignKey(bc => bc.RecipeId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<MealFood>()
                .HasOne(bc => bc.Product)
                .WithMany(c => c.MealFoods).IsRequired(false)
                .HasForeignKey(bc => bc.ProductId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);
        }
    }
}
