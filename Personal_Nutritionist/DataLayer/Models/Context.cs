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

        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Server=адрес_сервера/localhost;Database=имя_базы_данных;User Id=логин;Password=пароль;
            optionsBuilder.UseSqlServer(@"Server=KRISTINAS\SQLEXPRESS;Database=food;Trusted_Connection=True;");
           // optionsBuilder.UseSqlServer(@"Server=localhost;Database=food;Trusted_Connection=True;");
        }
    }
}
