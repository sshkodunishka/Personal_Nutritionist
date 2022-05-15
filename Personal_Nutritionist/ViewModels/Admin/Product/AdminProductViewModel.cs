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
    public class AdminProductViewModel : ViewModelBase
    {

        private ObservableCollection<Product> _product;
        public ObservableCollection<Product> Products
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ICommand NavigateAdminAddProduct { get; }

        public AdminProductViewModel(PersonalNavigationStore personalNavigationStore)
        {
            try
            {
                Context context = new Context();
                Repository<Product> repositoryProduct = new Repository<Product>(context);
                Repository<User> repositoryUser = new Repository<User>(context);

                User currentUser = Account.getInstance(null).CurrentUser;

                List<Product> products = repositoryProduct.GetWithInclude(y => y.User).ToList();
                Products = new ObservableCollection<Product>(products);

                NavigateAdminAddProduct = new PersonalNavigateCommand<AdminAddProductViewModel>(
                   new PersonalNavigationService<AdminAddProductViewModel>(personalNavigationStore,
                   () =>
                   {
                       return new AdminAddProductViewModel(personalNavigationStore);
                   }));
            }
            catch
            {
                MessageBox.Show("Can't see all records");
            }
        }
    }
}
