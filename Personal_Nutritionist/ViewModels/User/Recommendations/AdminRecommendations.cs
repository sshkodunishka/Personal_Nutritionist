﻿using Personal_Nutritionist.DataLayer;
using Personal_Nutritionist.DataLayer.Repository;
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
    public class AdminRecommendationsViewModel : ViewModelBase
    {
        private ObservableCollection<AdminRecommendation> _adminRecommendations;
        public ObservableCollection<AdminRecommendation> AdminRecommendations
        {
            get => _adminRecommendations;
            set
            {
                _adminRecommendations = value;
                OnPropertyChanged(nameof(AdminRecommendations));
            }
        }

        public AdminRecommendation _adminRecommendation;
        public AdminRecommendation AdminRecommendation
        {
            get => _adminRecommendation;
            set
            {
                _adminRecommendation = value;
                OnPropertyChanged(nameof(AdminRecommendation));
            }
        }


        public AdminRecommendationsViewModel(PersonalNavigationStore personalNavigationStore)
        {

            try
            {
                Context context = new Context();
                Repository<AdminRecommendation> repositoryRecommendations= new Repository<AdminRecommendation>(context);
                Repository<User> repositoryUser = new Repository<User>(context);

                User currentUser = Account.getInstance(null).CurrentUser;

                List<AdminRecommendation> recommendations = repositoryRecommendations.GetWithInclude(x => x.UserId == currentUser.UserId, y => y.User, z => z.Recipe).ToList();
                AdminRecommendations = new ObservableCollection<AdminRecommendation>(recommendations);
            }
            catch
            {
                MessageBox.Show("Can't see all recommandarions");
            }


        }
    }
}
