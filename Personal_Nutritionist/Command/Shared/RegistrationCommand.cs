using Personal_Nutritionist.DataLayer;
using Personal_Nutritionist.DataLayer.Repository;
using Personal_Nutritionist.Stores;
using Personal_Nutritionist.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Personal_Nutritionist.Command
{
    public class RegistrationCommand : CommandBase
    {
        private readonly RegistrationViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly PersonalNavigationStore _personalNavigationStore;

        public RegistrationCommand(RegistrationViewModel viewModel, NavigationStore navigationStore, PersonalNavigationStore personalNavigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _personalNavigationStore = personalNavigationStore;
        }

        public override bool CanExecute(object parameter)
        {
            if (_viewModel.Login != null && _viewModel.Password != null &&
                _viewModel.Name != null && _viewModel.Surname != null &&
                _viewModel.Age != 0 && _viewModel.Weight != 0 && _viewModel.Height != 0)
                return true;
            else
                return false;
        }

        public override void Execute(object parameter)
        {
            try
            {
                Account.Dispose();

                Context context = new Context();
                Repository<User> repository = new Repository<User>(context);
                Repository<Role> repositoryRole = new Repository<Role>(context);
                User user = new User();

                if (repository.Get(x => x.Login == _viewModel.Login).Any())
                {
                    _viewModel.Error = "This login is already taken";
                }
                else
                {
                    Role role = repositoryRole.Get(x => x.RoleName == RoleType.User).First();
                    _viewModel.Password = User.getHash(_viewModel.Password);
                    user = new User(_viewModel.Login, _viewModel.Password, _viewModel.Name, _viewModel.Surname, _viewModel.Weight, _viewModel.Age,  role.RoleId, _viewModel.Height, _viewModel.SexType);
                    repository.Create(user);

                    Account account = Account.getInstance(user);

                    _navigationStore.CurrentViewModel = new UserHomeViewModel(_personalNavigationStore, _navigationStore);
                    _personalNavigationStore.CurrentPersonalViewModel = new UserProfileViewModel(_personalNavigationStore);
                }

            }
            catch
            {
                MessageBox.Show("Can't register user");
            }
        }
    }
}
