
using System.Windows;
using System.Windows.Input;
using Personal_Nutritionist.Command;
using Personal_Nutritionist.Services;
using Personal_Nutritionist.Stores;

namespace Personal_Nutritionist.ViewModels
{
    public class UserHomeViewModel : ViewModelBase
    {
        private readonly PersonalNavigationStore _personalNavigationStore;
        public ViewModelBase CurrentPersonalViewModel => _personalNavigationStore.CurrentPersonalViewModel;

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentPersonalViewModel));
        }

        public ICommand ProfileCommand { get; }
        public ICommand RecipeCommand { get; }
        public ICommand ProductCommand { get; }
        public ICommand FavoriteCommand { get; }
        public ICommand LogOutCommand { get; }

        public UserHomeViewModel(PersonalNavigationStore personalNavigationStore, NavigationStore navigationStore)
        {
            try
            {
                _personalNavigationStore = personalNavigationStore;
                _personalNavigationStore.CurrentPersonalViewModelChanged += OnCurrentViewModelChanged;

                ProfileCommand = new PersonalNavigateCommand<UserProfileViewModel>(
                    new PersonalNavigationService<UserProfileViewModel>(personalNavigationStore,
                    () => new UserProfileViewModel(personalNavigationStore)));

                RecipeCommand = new PersonalNavigateCommand<UserRecipeViewModel>(
                    new PersonalNavigationService<UserRecipeViewModel>(personalNavigationStore,
                    () => new UserRecipeViewModel(personalNavigationStore)));

                ProductCommand = new PersonalNavigateCommand<UserProductViewModel>(
                    new PersonalNavigationService<UserProductViewModel>(personalNavigationStore,
                    () => new UserProductViewModel(personalNavigationStore)));

                FavoriteCommand = new PersonalNavigateCommand<UserFavoriteViewModel>(
                    new PersonalNavigationService<UserFavoriteViewModel>(personalNavigationStore,
                    () => new UserFavoriteViewModel(personalNavigationStore)));

                //LogOutCommand = new LogOutCommand(this, personalNavigationStore, navigationStore);
            }
            catch
            {
                MessageBox.Show("Can't load student home page");
            }
        }
    }
}

