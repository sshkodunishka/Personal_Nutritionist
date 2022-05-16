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
                _viewModel.BreakfastFood = "";
                _viewModel.LunchFood = "";
                _viewModel.DinnerFood = "";
                _viewModel.TotalCalories = 0;
                _viewModel.CaloriesLeft = 0;
                Context context = new Context();
                Repository<MealHistory> mealHistoryRepository = new Repository<MealHistory>(context);
                User user = Account.getInstance(null).CurrentUser;

                List<MealHistory> mealHistories = mealHistoryRepository.getMealHistory(user, _viewModel.SelectedDate);

                mealHistories.ForEach(mealHistory =>
                {
                    if (mealHistory.MealFood == null)
                    {
                        return;
                    }
                    mealHistory.MealFood.ToList().ForEach(mealFood =>
                    {
                        if (mealFood.ProductId != null)
                        {
                            _viewModel.TotalCalories += mealFood.Product.Calories;
                            if (mealHistory.MealType == MealType.Breakfast)
                            {
                                _viewModel.BreakfastFood += ", " + mealFood.Product.Name;
                            }
                            if (mealHistory.MealType == MealType.Lunch)
                            {
                                _viewModel.LunchFood += ", " + mealFood.Product.Name;
                            }
                            if (mealHistory.MealType == MealType.Dinner)
                            {
                                _viewModel.DinnerFood += ", " + mealFood.Product.Name;
                            }
                        }
                        else
                        {
                            _viewModel.TotalCalories += mealFood.Recipe.Calories;
                            if (mealHistory.MealType == MealType.Breakfast)
                            {
                                _viewModel.BreakfastFood += ", " + mealFood.Recipe.Name;
                            }
                            if (mealHistory.MealType == MealType.Lunch)
                            {
                                _viewModel.LunchFood += ", " + mealFood.Recipe.Name;
                            }
                            if (mealHistory.MealType == MealType.Dinner)
                            {
                                _viewModel.DinnerFood += ", " + mealFood.Recipe.Name;
                            }
                        }
                    });
                });


                float koef = 120;
                _viewModel.CaloriesLeft = user.Weight * koef - (float)_viewModel.TotalCalories;
            }
            catch
            {
                MessageBox.Show("Can't click new Date");
            }
        }
    }
}
