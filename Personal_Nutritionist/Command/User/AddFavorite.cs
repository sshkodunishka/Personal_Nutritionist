using Personal_Nutritionist.DataLayer;
using Personal_Nutritionist.DataLayer.Repository;
using Personal_Nutritionist.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Personal_Nutritionist.Command
{
    public class AddFavorite : CommandBase
    {
        private readonly UserRecipeViewModel _viewModel;

        public AddFavorite(UserRecipeViewModel viewModel)
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

                var fav = new Favorites(user.UserId, _viewModel.SelectedRecipe.RecipeId);
                favoriteRepository.Create(fav);

            }

            catch
            {
                MessageBox.Show("Can't delete selected meal");
            }
        }
    }
}
