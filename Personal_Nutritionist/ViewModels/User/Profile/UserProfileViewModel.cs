using Personal_Nutritionist.Command;
using Personal_Nutritionist.DataLayer;
using Personal_Nutritionist.Services;
using Personal_Nutritionist.Stores;
using System;
using System.Windows;
using System.Windows.Input;

namespace Personal_Nutritionist.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
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

        public int _totalCalories;
        public int TotalCalories
        {
            get => _totalCalories;
            set
            {
                _totalCalories = value;
                OnPropertyChanged(nameof(TotalCalories));
            }
        }

        public int _caloriesLeft;
        public int CaloriesLeft
        {
            get => _caloriesLeft;
            set
            {
                _caloriesLeft = value;
                OnPropertyChanged(nameof(CaloriesLeft));
            }
        }

        public string _breakfastFood;
        public string BreakfastFood
        {
            get => _breakfastFood;
            set
            {
                _breakfastFood = value;
                OnPropertyChanged(nameof(BreakfastFood));
            }
        }

        public string _lunchFood;
        public string LunchFood
        {
            get => _lunchFood;
            set
            {
                _lunchFood = value;
                OnPropertyChanged(nameof(LunchFood));
            }
        }

        public string _dinnerFood;
        public string DinnerFood
        {
            get => _dinnerFood;
            set
            {
                _dinnerFood = value;
                OnPropertyChanged(nameof(DinnerFood));
            }
        }


        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen == value)
                    return;
                _isOpen = value;
                OnPropertyChanged(nameof(IsOpen));
            }
        }

        public ICommand SetPrevDate { get; }
        public ICommand SetNextDate { get; }
        public ICommand ChangeBreakfast { get; }
        public ICommand ChangeLunch { get; }
        public ICommand ChangeDinner { get; }

        public UserProfileViewModel(PersonalNavigationStore personalNavigationStore)
        {
            try
            {
                SelectedDate = DateTime.Now;

                SetPrevDate = new ChangeSelectedDate(this, false);
                SetNextDate = new ChangeSelectedDate(this, true);
                


                TotalCalories = 0;
                CaloriesLeft = 0;
                BreakfastFood = "asdasd";
                LunchFood = "asdasd";
                DinnerFood = "asdasd";


                ChangeBreakfast = new PersonalNavigateCommand<ChangeMealViewModel>(
                   new PersonalNavigationService<ChangeMealViewModel>(personalNavigationStore,
                   () =>
                   {
                       return new ChangeMealViewModel(personalNavigationStore, SelectedDate, MealType.Breakfast);
                   }));

                ChangeLunch = new PersonalNavigateCommand<ChangeMealViewModel>(
                  new PersonalNavigationService<ChangeMealViewModel>(personalNavigationStore,
                  () =>
                  {
                      return new ChangeMealViewModel(personalNavigationStore, SelectedDate, MealType.Lunch);
                  }));

                ChangeDinner = new PersonalNavigateCommand<ChangeMealViewModel>(
                  new PersonalNavigationService<ChangeMealViewModel>(personalNavigationStore,
                  () =>
                  {
                      return new ChangeMealViewModel(personalNavigationStore, SelectedDate, MealType.Dinner);
                  }));
            }
            catch
            {
                MessageBox.Show("Can't load tutor ptofile");
            }
        }
    }
}
