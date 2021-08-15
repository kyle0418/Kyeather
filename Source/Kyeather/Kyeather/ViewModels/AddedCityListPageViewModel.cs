using Kyeather.Models;
using Kyeather.Views;
using MvvmHelpers;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Kyeather.ViewModels
{
    public delegate void SelectedCityDelegate(City city);
    class AddedCityListPageViewModel : BaseViewModel
    {
        public event SelectedCityDelegate SelectedCityEvent;
        INavigation Navigation { get; set; }
        public ObservableRangeCollection<City> AddedCityList { get; set; }
        public ICommand OpenAddNewCityPageCommand { get; }
        public ICommand SelectedCommand { get; }
        public ICommand LongPressCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public AddedCityListPageViewModel(INavigation navigation)
        {
            OpenAddNewCityPageCommand = new Xamarin.Forms.Command(AddNewCity);
            SelectedCommand = new Xamarin.Forms.Command<object>(Selected);
            LongPressCommand = new Xamarin.Forms.Command(LongPress);
            EditCommand = new Xamarin.Forms.Command(Edit);
            DeleteCommand = new Xamarin.Forms.Command<City>(Delete);
            Navigation = navigation;
            ShowDeleteButton = false;
            EditText = "≡";
        }

        private void AddNewCity()
        {
            if (AddedCityList.Count >= 9)
            {
                CrossToastPopUp.Current.ShowCustomToast("若要添加新城市，请先移除一个城市", "#e0e6e6e6", "#000000", Plugin.Toast.Abstractions.ToastLength.Short);
                return;
            }
            Navigation.PushAsync(new AddCityPage());
            ShowDeleteButton = false;
            EditText = "≡";
        }

        City selectedCity;
        public City SelectedCity
        {
            get => selectedCity;
            set => SetProperty(ref selectedCity, value);
        }

        private void Selected(object args)
        {
            var city = args as City;
            if (city == null)
                return;

            SelectedCityEvent(city);
            SelectedCity = null;

            Navigation.PopAsync();
        }

        bool showDeleteButton;
        public bool ShowDeleteButton
        {
            get => showDeleteButton;
            set => SetProperty(ref showDeleteButton, value);
        }

        string editText;
        public string EditText
        {
            get => editText;
            set => SetProperty(ref editText, value);
        }

        private void LongPress()
        {
            ShowDeleteButton = true;
        }

        private void Edit()
        {
            ShowDeleteButton = !ShowDeleteButton;
            EditText = ShowDeleteButton ? "✓" : "≡";
        }

        private void Delete(City city)
        {
            var selectedcity = city as City;
            if (city == null)
                return;

            if (AddedCityList.IndexOf(city) == 0) // located city
            {
                CrossToastPopUp.Current.ShowCustomToast("定位所在城市无法移除", "#e0e6e6e6", "#000000", Plugin.Toast.Abstractions.ToastLength.Short);
                return;
            }
            AddedCityList.Remove(city);

            string precityListString = Preferences.Get("key_citylist", "").Trim(',');
            Console.WriteLine("======================");
            Console.WriteLine(precityListString);
            Console.WriteLine("======================");
            List<string> precityList = precityListString.Split(',').ToList();
            precityList.RemoveAll(x => x == $"{city.Name}:{city.Id}");
            if (precityList.Count == 0)
            {
                Preferences.Set("key_citylist", "");
                Preferences.Set("isFirstTimeLoad", true);
                return;
            }
            string newCityListString = string.Join(",", precityList);
            Preferences.Set("key_citylist", newCityListString);
        }
    }
}
