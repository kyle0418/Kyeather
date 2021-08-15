using Kyeather.API;
using Kyeather.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Kyeather.ViewModels
{
    class AddCityPageViewModel : BaseViewModel
    {
        INavigation Navigation { get; set; }

        List<City> cityList = new List<City>();
        public List<City> CityList
        {
            get { return cityList; }
            set
            {
                cityList = value;
                OnPropertyChanged("CityList");
            }
        }

        City selectedCity;
        public City SelectedCity
        {
            get => selectedCity;
            set => SetProperty(ref selectedCity, value);
        }

        public ICommand TextChangedCommand { get; }
        public ICommand SelectedCommand { get; }

        public AddCityPageViewModel(INavigation navigation)
        {
            TextChangedCommand = new Xamarin.Forms.Command<string>(TextChanged);
            SelectedCommand = new Xamarin.Forms.Command<object>(Selected);
            Navigation = navigation;
        }

        private void TextChanged(string location)
        {
            CityList = HefengWeatherAPI.GetCityList(location);
        }

        private void Selected(object args)
        {
            var city = args as City;
            if (city == null)
                return;
            string citystr = city.Name + ":" + city.Id;

            string precityListString = Preferences.Get("key_citylist", "");

            List<string> precityList = precityListString.Split(',').ToList();
            // 该城市已存在
            if (precityList.Contains($"{city.Name}:{city.Id}"))
            {
                CrossToastPopUp.Current.ShowCustomToast("该城市已存在", "#e0e6e6e6", "#000000", Plugin.Toast.Abstractions.ToastLength.Short);

            }
            else
            {
                string newCityListString = string.Join(",", precityList);
                // 更新key_citylist，将新城市信息添加到末尾
                Preferences.Set("key_citylist", precityListString + "," + citystr);
            }

            SelectedCity = null;

            // pop to root page
            Navigation.PopToRootAsync();
        }
    }
}
