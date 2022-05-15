using Personal_Nutritionist.DataLayer;
using Personal_Nutritionist.DataLayer.Repository;
using Personal_Nutritionist.Stores;
using Personal_Nutritionist.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Personal_Nutritionist
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();

            PersonalNavigationStore personalNavigationStore = new PersonalNavigationStore();

            navigationStore.CurrentViewModel = new UserHomeViewModel(personalNavigationStore, navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };

            Context context = new Context();

            Repository<Role> repositoryRole = new Repository<Role>(context);
            Repository<User> repositoryUser = new Repository<User>(context);

            if (!repositoryRole.Get().Any())
            {
                Role user = new Role(nameof(RoleType.User));
                Role admin = new Role(nameof(RoleType.Admin));
                repositoryRole.Create(user);
                repositoryRole.Create(admin);

                Role adminRole = repositoryRole.Get((x => x.RoleName == RoleType.Admin)).First();
                string password = User.getHash("admin");
                User adminUser = new User("admin", password, "Anton", "Borisov", 30, adminRole.RoleId);

                repositoryUser.Create(adminUser);


            }
            Repository<Product> repositoryProduct = new Repository<Product>(context);
            if (!repositoryProduct.Get().Any())
            {
                User admin = repositoryUser.GetWithInclude(x=>x.Role.RoleName== RoleType.Admin,y => y.Role ).First();
                Product banana = new Product("banana", 89, admin.UserId);
                Product aple = new Product("aple", 52, admin.UserId);


                List<Product> products = new List<Product>();
                products.Add(aple);
                products.Add(banana);
                repositoryProduct.CreateRange(products);
            }

            Repository<Recipe> repositoryRecipe = new Repository<Recipe>(context);
            if (!repositoryRecipe.Get().Any())
            {
                User admin = repositoryUser.GetWithInclude(x => x.Role.RoleName == RoleType.Admin, y => y.Role).First();
                Recipe draniki = new Recipe("draniki", 320, "sone description ", admin.UserId);
                Recipe babka = new Recipe("babka", 458, "sone description ", admin.UserId);
                


                List<Recipe> recipes = new List<Recipe>();
                recipes.Add(draniki);
                recipes.Add(babka);
                repositoryRecipe.CreateRange(recipes);
            }

            User user1 = repositoryUser.Get().First();
            Account.getInstance(user1);

            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
