﻿using Personal_Nutritionist.DataLayer;
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
    public class AdminAddRecipe : CommandBase
    {
        private readonly AddRecipeViewModel _viewModel;
        private readonly PersonalNavigationStore _personalNavigationStore;

        public AdminAddRecipe(AddRecipeViewModel viewModel, PersonalNavigationStore personalNavigationStore)
        {
            _viewModel = viewModel;
            _personalNavigationStore = personalNavigationStore;
        }

        public override bool CanExecute(object parameter)
        {
            try
            {
                if (_viewModel.Name != null && _viewModel.Calories != 0 && _viewModel.Description != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                Context context = new Context();
                Repository<User> repository = new Repository<User>(context);
                Repository<Recipe> courseRepository = new Repository<Recipe>(context);

                User user = Account.getInstance(null).CurrentUser;

                Recipe course = new Recipe(_viewModel.Name,
                    _viewModel.Calories, _viewModel.Description, user.UserId);
                courseRepository.Create(course);

                _personalNavigationStore.CurrentPersonalViewModel = new AdminRecipeViewModel(_personalNavigationStore);
            }
            catch
            {
                MessageBox.Show("Can't add course");
            }
        }

    }
}
