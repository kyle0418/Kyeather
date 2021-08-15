using Kyeather.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kyeather.ViewModels
{
    class MoreWeatherInfoViewModel
    {
        public City CurrentCity { get; set; }

        public MoreWeatherInfoViewModel(City city)
        {
            CurrentCity = city;
        }
    }
}
