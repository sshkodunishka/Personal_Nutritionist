using Personal_Nutritionist.Command;
using Personal_Nutritionist.Services;
using Personal_Nutritionist.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Personal_Nutritionist.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private int _age;
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(_age));
            }
        }

       

        private float _weight;
        public float Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen == value)
                    return;
                _isOpen = value;
                OnPropertyChanged(nameof(IsOpen));
            }
        }

        private bool _isSended;
        public bool IsSended
        {
            get { return _isSended; }
            set
            {
                if (_isSended == value)
                    return;
                _isSended = value;
                OnPropertyChanged(nameof(IsSended));
            }
        }

        public ICommand ToProfile { get; }
        public ICommand LoginCommand { get; }
        public ICommand Ok { get; }

        public RegistrationViewModel(NavigationStore navigationStore, PersonalNavigationStore personalNavigationStore)
        {
            try
            {
                ToProfile = new RegistrationCommand(this, navigationStore, personalNavigationStore);

                LoginCommand = new NavigateCommand<LoginViewModel>(
                    new NavigationService<LoginViewModel>(navigationStore,
                    () => new LoginViewModel(navigationStore, personalNavigationStore)));
                
                //Ok = new OkPopUpCommand(this);
            }
            catch
            {
                MessageBox.Show("Can't register user");
            }
        }

    }
}
