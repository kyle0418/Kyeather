using Kyeather.API;
using Kyeather.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kyeather.ViewModels
{
    class WeatherViewModel : BaseViewModel
    {
        public ObservableRangeCollection<City> AddedCityList { get; set; }

        City currentCity;
        public City CurrentCity
        {
            get => currentCity;
            set => SetProperty(ref currentCity, value);
        }

        public AsyncCommand RefreshCommand { get; }

        public WeatherViewModel()
        {
            AddedCityList = new ObservableRangeCollection<City>();
            RefreshCommand = new AsyncCommand(Refresh);
        }

        async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(500);

            ObservableRangeCollection<City> addedCityListCopy = new ObservableRangeCollection<City>();

            foreach (City city in AddedCityList)
            {
                addedCityListCopy.Add(city);
            }


            for (int i = 0; i < addedCityListCopy.Count; i++)
            {
                string id = addedCityListCopy[i].Id;
                CityWeatherInfo weatherInfo = HefengWeatherAPI.GetCityInfoNow(id);
                if (weatherInfo == null)
                {
                    CrossToastPopUp.Current.ShowCustomToast("服务器开小差啦~", "#e0e6e6e6", "#000000", Plugin.Toast.Abstractions.ToastLength.Long);
                    return;
                }
                addedCityListCopy[i].WeatherInfo.Icon = weatherInfo.Icon;
                addedCityListCopy[i].WeatherInfo.Temp = weatherInfo.Temp;
            }

            AddedCityList.Clear();

            foreach (City city in addedCityListCopy)
            {
                AddedCityList.Add(city);
            }

            IsBusy = false;
        }

    }
}
