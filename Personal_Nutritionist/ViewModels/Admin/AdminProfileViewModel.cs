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
    public class AdminProfileViewModel : ViewModelBase
    {
        
        public string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        public ICommand UpdateProfile { get; }
        public ICommand AddPhoto { get; }
        public ICommand Ok { get; }

        public AdminProfileViewModel(PersonalNavigationStore personalNavigationStore)
        {
            try
            {
             
                Name = "123";

            }
            catch
            {
                MessageBox.Show("Can't load tutor ptofile");
            }
        }
    }
}
