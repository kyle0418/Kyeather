using Kyeather.API;
using Kyeather.Models;
using Kyeather.Services;
using Kyeather.ViewModels;
using Kyeather.Views;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Kyeather
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //HefengWeatherAPI.key = "5a3fd7fbff7f40ecbad585a6d131fa16";
            HefengWeatherAPI.key = "7f3f3edee25b49a597a245ad44e6fe49";
        }

        bool isFirstTimeLoad;
        string original_key_citylist = null;
        City locatedcity = null;

        int count = 0;

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                return;
            }

            if (count == 0)
            {
                Preferences.Set("isFirstTimeLoad", true);
                count = 1;
            }

            isFirstTimeLoad = Preferences.Get("isFirstTimeLoad", true);

            List<City> storedcityList = new List<City>();

            //打开程序后获取定位的地理位置信息，添加所在地区信息
            if (isFirstTimeLoad)
            {
                if (count == 1)
                    original_key_citylist = "";
                (this.BindingContext as WeatherViewModel).AddedCityList.Clear();

                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    string latitude = location.Latitude.ToString();
                    string longitude = location.Longitude.ToString();

                    locatedcity = HefengWeatherAPI.GetCityByLatAndLong(latitude, longitude);

                    CityWeatherInfo weatherInfoNow = HefengWeatherAPI.GetCityInfoNow(locatedcity.Id);

                    if (weatherInfoNow == null)
                    {
                        CrossToastPopUp.Current.ShowCustomToast("服务器开小差啦~", "#e0e6e6e6", "#000000", Plugin.Toast.Abstractions.ToastLength.Long);
                        return;
                    }

                    List<CityWeatherInfoOfDay> weatherInfoList = HefengWeatherAPI.GetCityInfo3Days(locatedcity.Id);

                    TimeSpan sunrise = TimeSpan.Parse(weatherInfoList[0].Sunrise);
                    TimeSpan sunset = TimeSpan.Parse(weatherInfoList[0].Sunset);

                    int backgroundTemp = (DateTime.Now.TimeOfDay > sunrise)
                                    && (DateTime.Now.TimeOfDay < sunset)
                                    ? weatherInfoNow.Temp : -100;

                    locatedcity = new City()
                    {
                        Name = locatedcity.Name,
                        Id = locatedcity.Id,
                        WeatherInfo = weatherInfoNow,
                        CityInfoOfDayList = weatherInfoList,
                        IsLocated = true,
                        BackgroundTemp = backgroundTemp
                    };

                    (this.BindingContext as WeatherViewModel).AddedCityList.Add(locatedcity);

                }
                else
                {
                    CrossToastPopUp.Current.ShowCustomToast("获取地理位置信息失败!", "#e0e6e6e6", "#000000", Plugin.Toast.Abstractions.ToastLength.Long);
                }
            }

            // 城市名:城市ID,城市名:城市ID,城市名:城市ID,...
            var myValue = Preferences.Get("key_citylist", "");

            //如果为空或未发生改变，退出该方法
            if (myValue == "")
            {
                //isFirstTimeLoad = false;
                Preferences.Set("isFirstTimeLoad", false);
                return;
            }
            // 未改变，退出执行
            if (myValue == original_key_citylist)
                return;

            //读取key_citylist，获取城市列表
            original_key_citylist = myValue;
            string[] citystringArray = myValue.Trim(',').Split(',').ToArray();

            foreach (string citystr in citystringArray)
            {
                CityWeatherInfo weatherInfoNow = HefengWeatherAPI.GetCityInfoNow(citystr.Split(':')[1]);
                if (weatherInfoNow == null)
                {
                    CrossToastPopUp.Current.ShowCustomToast("服务器开小差啦~", "#e0e6e6e6", "#000000", Plugin.Toast.Abstractions.ToastLength.Long);
                    return;
                }
                List<CityWeatherInfoOfDay> weatherInfoList = HefengWeatherAPI.GetCityInfo3Days(citystr.Split(':')[1]);

                TimeSpan sunrise = TimeSpan.Parse(weatherInfoList[0].Sunrise);
                TimeSpan sunset = TimeSpan.Parse(weatherInfoList[0].Sunset);

                int backgroundTemp = (DateTime.Now.TimeOfDay > sunrise)
                                && (DateTime.Now.TimeOfDay < sunset)
                                ? weatherInfoNow.Temp : -100;

                City newcity = new City()
                {
                    Name = citystr.Split(':')[0],
                    Id = citystr.Split(':')[1],
                    WeatherInfo = weatherInfoNow,
                    CityInfoOfDayList = weatherInfoList,
                    BackgroundTemp = backgroundTemp
                };
                storedcityList.Add(newcity);
            }
            (this.BindingContext as WeatherViewModel).AddedCityList.Clear();
            storedcityList.Insert(0, locatedcity);
            //(this.BindingContext as WeatherViewModel).AddedCityList.Add(locatedcity);
            (this.BindingContext as WeatherViewModel).AddedCityList.AddRange(storedcityList);
            // 不是第一次加载，即添加新城市后才跳转到最后页面
            if (!isFirstTimeLoad)
            {
                (this.BindingContext as WeatherViewModel).CurrentCity = storedcityList.Last();
            }
            //isFirstTimeLoad = false;
            Preferences.Set("isFirstTimeLoad", false);
        }

        protected override bool OnBackButtonPressed()
        {
            DependencyService.Get<IHomeButtonSimulator>().SimulateHomeButton();
            return true;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new MoreWeatherInfo((this.BindingContext as WeatherViewModel).CurrentCity));
        }

        private void TapGestureRecognizer_Tapped1(object sender, EventArgs e)
        {
            Preferences.Set("key_citylist", "");
        }

        private void City_TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            AddedCityListPage addedCityListPage = new AddedCityListPage((this.BindingContext as WeatherViewModel).AddedCityList);
            (addedCityListPage.BindingContext as AddedCityListPageViewModel).SelectedCityEvent += MainPage_SelectedCityEvent;
            this.Navigation.PushAsync(addedCityListPage);
        }

        private void MainPage_SelectedCityEvent(City city)
        {
            (this.BindingContext as WeatherViewModel).CurrentCity = city;
        }
    }
}
