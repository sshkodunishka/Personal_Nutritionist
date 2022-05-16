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
    public class AdminUsersViewModel : ViewModelBase
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public ICommand OpenUser { get; }

        public AdminUsersViewModel(PersonalNavigationStore personalNavigationStore)
        {
            try
            {
                Context context = new Context();
                Repository<User> repositoryUser = new Repository<User>(context);

                List<User> users = repositoryUser.GetWithInclude(x => x.Role.RoleName == RoleType.User, y => y.Role).ToList();

                Users = new ObservableCollection<User>(users);

                OpenUser = new PersonalNavigateCommand<AdminUserInfoViewModel>(
                    new PersonalNavigationService<AdminUserInfoViewModel>(personalNavigationStore,
                    () =>
                    {
                        if (SelectedUser == null)
                            return null;
                        return new AdminUserInfoViewModel(personalNavigationStore, SelectedUser);
                    }));

            }
            catch
            {
                MessageBox.Show("Can't see all users");
            }
        }
    }
}
