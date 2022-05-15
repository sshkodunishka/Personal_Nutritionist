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
    public class ChangeSelectedDate : CommandBase
    {

        private readonly UserProfileViewModel _viewModel;
        private bool isNextDay;

        public ChangeSelectedDate(UserProfileViewModel viewModel, bool _isNextDay)
        {
            isNextDay = _isNextDay;
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

                Repository<User> repository = new Repository<User>(context);
                Repository<Product> productRepository = new Repository<Product>(context);
                DateTime newSelectedDate;
                if (this.isNextDay)
                {
                    newSelectedDate = this._viewModel.SelectedDate.AddDays(1);
                }
                else
                {
                    newSelectedDate = this._viewModel.SelectedDate.AddDays(-1);
                }
                _viewModel.SelectedDate = newSelectedDate;
                _viewModel.TotalCalories = 10;
                _viewModel.CaloriesLeft = 12;
                _viewModel.BreakfastFood = "asdas";
                _viewModel.DinnerFood = "123123";
                _viewModel.LunchFood = "21312";
                User user = Account.getInstance(null).CurrentUser;

            }
            catch
            {
                MessageBox.Show("Can't click new Date");
            }
        }
    }
}
