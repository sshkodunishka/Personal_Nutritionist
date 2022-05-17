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
    public class DeleteFavoriteCommand : CommandBase
    {

        private readonly UserRecipeViewModel _viewModel;

        public DeleteFavoriteCommand(UserRecipeViewModel viewModel)
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
                Repository<Favorites> favoriteRepository = new Repository<Favorites>(context);
                User user = Account.getInstance(null).CurrentUser;
                var id = _viewModel.SelectedRecipe.FavoriteId;
                List<RecipeView> newRecipeViews = _viewModel.Recipes.Select((recieVIew) =>
                {
                    if (recieVIew.Recipe.RecipeId == _viewModel.SelectedRecipe.Recipe.RecipeId)
                    {
                        _viewModel.SelectedRecipe.isFavorite = false;
                        _viewModel.SelectedRecipe.isNotFavorite = true;
                        _viewModel.SelectedRecipe.FavoriteId = null;
                        return _viewModel.SelectedRecipe;
                    };
                    return recieVIew;
                }).ToList();
                _viewModel.Recipes = new ObservableCollection<RecipeView>(newRecipeViews);

                Favorites fav = favoriteRepository.FindById((int)id);
                favoriteRepository.Remove(fav);
            }
            catch
            {
                MessageBox.Show("Can't delete selected meal");
            }
        }
    }
}
