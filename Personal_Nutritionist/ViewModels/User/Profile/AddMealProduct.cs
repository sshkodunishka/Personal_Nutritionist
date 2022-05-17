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
    public class AddMealProduct : ViewModelBase
    {
        public DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public MealType _selectedType;
        public MealType SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        public ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public ICommand AddMealProductCommand { get; }
        public ICommand BackToChangeMeal { get; }

        public AddMealProduct(PersonalNavigationStore personalNavigationStore, DateTime selectedDate, MealType mealType)
        {
            try
            {
                SelectedDate = selectedDate;
                SelectedType = mealType;
                Context context = new Context();
                Repository<Product> repositoryRecipe = new Repository<Product>(context);
                User user = Account.getInstance(null).CurrentUser;
                List<Product> products = repositoryRecipe
                    .GetWithInclude(x => x.UserId == user.UserId || x.User.Role.RoleName == RoleType.Admin, y => y.User, y => y.User.Role).ToList();
                Products = new ObservableCollection<Product>(products);

                AddMealProductCommand = new Command.AddMealProduct(this, personalNavigationStore);

                BackToChangeMeal = new PersonalNavigateCommand<ChangeMealViewModel>(
                  new PersonalNavigationService<ChangeMealViewModel>(personalNavigationStore,
                  () =>
                  {
                      return new ChangeMealViewModel(personalNavigationStore, selectedDate, mealType);
                  }));
            }
            catch
            {
                MessageBox.Show("Error in add meal recipe");
            }
        }
    }
}
