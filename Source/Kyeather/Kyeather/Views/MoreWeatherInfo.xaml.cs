using Kyeather.Models;
using Kyeather.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kyeather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoreWeatherInfo : ContentPage
    {
        public City CurrentCity { get; set; }

        public MoreWeatherInfo(City city)
        {
            InitializeComponent();
            CurrentCity = city;

            this.BindingContext = new MoreWeatherInfoViewModel(city);
        }
    }
}