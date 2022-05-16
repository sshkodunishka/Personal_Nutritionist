 using Personal_Nutritionist.Command;
using Personal_Nutritionist.DataLayer;
using Personal_Nutritionist.DataLayer.Repository;
using Personal_Nutritionist.Services;
using Personal_Nutritionist.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Personal_Nutritionist.ViewModels
{
    public class UserRecipeViewModel : ViewModelBase
    {
        private ObservableCollection<Recipe> _recipes;
        public ObservableCollection<Recipe> Recipes
        {
            get => _recipes;
            set
            {
                _recipes = value;
                OnPropertyChanged(nameof(Recipes));
            }
        }

        public Recipe _selectedRecipe;
        public Recipe SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }

        public ICommand OpenRecipe { get; }
        public ICommand NavigateUserAddRecipe { get; }

        public UserRecipeViewModel(PersonalNavigationStore personalNavigationStore)
        {
            try
            {
                Context context = new Context();
                Repository<Recipe> repositoryRecipe = new Repository<Recipe>(context);
                Repository<User> repositoryUser = new Repository<User>(context);

                User currentUser = Account.getInstance(null).CurrentUser;

                List<Recipe> recipes = repositoryRecipe.GetWithInclude(x => x.User.Role.RoleName == RoleType.Admin
                || x.UserId == currentUser.UserId, y => y.User, z => z.User.Role).ToList();
                Recipes = new ObservableCollection<Recipe>(recipes);

                OpenRecipe = new PersonalNavigateCommand<UserRecipeInfoViewModel>(
                    new PersonalNavigationService<UserRecipeInfoViewModel>(personalNavigationStore,
                    () =>
                    {
                        if (SelectedRecipe == null)
                            return null;
                        return new UserRecipeInfoViewModel(personalNavigationStore, SelectedRecipe);
                    }));

                NavigateUserAddRecipe = new PersonalNavigateCommand<UserAddRecipeViewModel>(
                   new PersonalNavigationService<UserAddRecipeViewModel>(personalNavigationStore,
                   () =>
                   {
                       return new UserAddRecipeViewModel(personalNavigationStore);
                   }));
            }
            catch
            {
                MessageBox.Show("Can't see all records");
            }
        }
    }
}
