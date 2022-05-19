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
    public class AdminDeleteRecipe : CommandBase
    {
        private readonly AdminRecipeViewModel _viewModel;

        public AdminDeleteRecipe(AdminRecipeViewModel viewModel)
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

                Repository<Recipe> recipeRepository = new Repository<Recipe>(context);
                User user = Account.getInstance(null).CurrentUser;
                Repository<MealFood> mealFoodRepository = new Repository<MealFood>(context);
                Repository<Favorites> favRepo = new Repository<Favorites>(context);
                Repository<AdminRecommendation> adminRecRepo = new Repository<AdminRecommendation>(context);
                
                var id = _viewModel.SelectedRecipe.RecipeId;
                _viewModel.Recipes = new ObservableCollection<Recipe>(
                    _viewModel.Recipes.Where(f => f.RecipeId != _viewModel.SelectedRecipe.RecipeId));
                var food = recipeRepository.FindById(id);
                
                var mealFoods = mealFoodRepository.Get(x => x.RecipeId == food.RecipeId).ToList();
                mealFoods.ForEach(meal =>
                {
                    mealFoodRepository.Remove(meal);
                });

                var favorites = favRepo.Get(x => x.RecipeId == food.RecipeId).ToList();
                favorites.ForEach(meal =>
                {
                    favRepo.Remove(meal);
                });
                
                var adminReccomendations = adminRecRepo.Get(x => x.RecipeId == food.RecipeId).ToList();
                adminReccomendations.ForEach(meal =>
                {
                    adminRecRepo.Remove(meal);
                });
                
                recipeRepository.Remove(food);

            }

            catch
            {
                MessageBox.Show("Can't delete selected recipe");
            }
        }
    }
}
