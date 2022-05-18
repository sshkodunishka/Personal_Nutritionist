using Personal_Nutritionist.DataLayer;
using Personal_Nutritionist.DataLayer.Repository;
using Personal_Nutritionist.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Personal_Nutritionist.Command
{
    public class AdminDeleteProduct : CommandBase
    {
        private readonly AdminProductViewModel _viewModel;

        public AdminDeleteProduct(AdminProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            try
            {
                Context context = new Context();

                Repository<Product> productRepository = new Repository<Product>(context);
                Repository<MealFood> mealFoodRepository = new Repository<MealFood>(context);
                User user = Account.getInstance(null).CurrentUser;



                var id = _viewModel.SelectedProduct.ProductId;
                _viewModel.Products = new ObservableCollection<Product>(
                    _viewModel.Products.Where(f => f.ProductId != _viewModel.SelectedProduct.ProductId));
                var food = productRepository.FindById(id);
                var mealFoods = mealFoodRepository.Get(x => x.ProductId == food.ProductId).ToList();
                mealFoods.ForEach(meal =>
                {
                    mealFoodRepository.Remove(meal);
                });
                productRepository.Remove(food);

            }

            catch
            {
                MessageBox.Show("Can't delete selected product");
            }
        }
    }
}
