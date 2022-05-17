using Personal_Nutritionist.Command;
using Personal_Nutritionist.DataLayer;
using Personal_Nutritionist.DataLayer.Repository;
using Personal_Nutritionist.Services;
using Personal_Nutritionist.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public float _caloriesLeft;
        public float CaloriesLeft
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

        public float _adminCountedCalories;
        public float AdminCountedCalories
        {
            get => _adminCountedCalories;
            set
            {
                _adminCountedCalories = value;
                OnPropertyChanged(nameof(AdminCountedCalories));
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

                Context context = new Context();
                Repository<MealHistory> mealHistoryRepository = new Repository<MealHistory>(context);
                User user = Account.getInstance(null).CurrentUser;

                Repository<AdminCountingCalories> adminCaloriesRepository = new Repository<AdminCountingCalories>(context);

                AdminCountedCalories = 0;
                IEnumerable<AdminCountingCalories> adminCalories = adminCaloriesRepository.
                    Get(x => x.UserId == user.UserId
                        && x.Date.Year == SelectedDate.Year
                        && x.Date.Month == SelectedDate.Month
                        && x.Date.Day == SelectedDate.Day);

                if (adminCalories.Count() > 0)
                {
                    AdminCountedCalories = adminCalories.First().Calories;
                }

                List<MealHistory> mealHistories = mealHistoryRepository.getMealHistory(user, SelectedDate);

                mealHistories.ForEach(mealHistory =>
                {
                    if (mealHistory.MealFood != null)
                    {
                        mealHistory.MealFood.ToList().ForEach(mealFood =>
                        {
                            if (mealFood.ProductId != null)
                            {
                                TotalCalories += mealFood.Product.Calories;
                                if (mealHistory.MealType == MealType.Breakfast)
                                {
                                    BreakfastFood += mealFood.Product.Name + ", ";
                                }
                                if (mealHistory.MealType == MealType.Lunch)
                                {
                                    LunchFood += mealFood.Product.Name + ", ";
                                }
                                if (mealHistory.MealType == MealType.Dinner)
                                {
                                    DinnerFood += mealFood.Product.Name + ", ";
                                }
                            }
                            else
                            {
                                TotalCalories += mealFood.Recipe.Calories;
                                if (mealHistory.MealType == MealType.Breakfast)
                                {
                                    BreakfastFood += mealFood.Recipe.Name + ", ";
                                }
                                if (mealHistory.MealType == MealType.Lunch)
                                {
                                    LunchFood += mealFood.Recipe.Name + ", ";
                                }
                                if (mealHistory.MealType == MealType.Dinner)
                                {
                                    DinnerFood += mealFood.Recipe.Name + ", ";
                                }
                            }
                        });
                    }
                });
                float koef = 4.3f;
                CaloriesLeft = user.Weight * koef - (float)TotalCalories;

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
                MessageBox.Show("Can't load user ptofile");
            }
        }

    }
}
