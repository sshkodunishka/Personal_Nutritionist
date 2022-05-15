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
    public class AddMealRecipe : CommandBase
    {
        private readonly ViewModels.AddMealRecipe _viewModel;
        private readonly PersonalNavigationStore _personalNavigationStore;

        public AddMealRecipe(ViewModels.AddMealRecipe viewModel, PersonalNavigationStore personalNavigationStore)
        {
            _viewModel = viewModel;
            _personalNavigationStore = personalNavigationStore;
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
                Repository<MealHistory> mealHistoryRepository = new Repository<MealHistory>(context);
                Repository<MealFood> mealFoodRepository = new Repository<MealFood>(context);

                User currentUser = Account.getInstance(null).CurrentUser;

                IEnumerable<MealHistory> histories = mealHistoryRepository
                    .GetWithInclude(x => x.UserId == currentUser.UserId 
                    && x.Date.Year == _viewModel.SelectedDate.Year
                    && x.Date.Month == _viewModel.SelectedDate.Month
                    && x.Date.Day == _viewModel.SelectedDate.Day
                    && x.MealType == _viewModel.SelectedType,
                    y => y.User);
                MealHistory history;
                if (histories.Count() == 0)
                {
                    history = new MealHistory(_viewModel.SelectedDate, _viewModel.SelectedType, currentUser.UserId);
                    mealHistoryRepository.Create(history);
                }
                else
                {
                    history = histories.First();
                }
                MealFood food = new MealFood(_viewModel.SelectedRecipe.RecipeId, history.MealHistoryId, true);
                mealFoodRepository.Create(food);
                _personalNavigationStore.CurrentPersonalViewModel = new ChangeMealViewModel(_personalNavigationStore, _viewModel.SelectedDate, _viewModel.SelectedType);
            }
            catch
            {
                MessageBox.Show("Error while adding meal recipe");
            }
        }
    }
}
