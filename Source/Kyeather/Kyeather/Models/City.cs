using System;
using System.Collections.Generic;
using System.Text;

namespace Kyeather.Models
{
    public class City
    {

        public string Name { get; set; }
        public string Id { get; set; }
        public string Adm2 { get; set; }
        public string Adm1 { get; set; }
        public string Country { get; set; }
        public CityWeatherInfo WeatherInfo { get; set; }
        public List<CityWeatherInfoOfDay> CityInfoOfDayList { get; set; }
        public bool IsLocated { get; set; } = false;
        // 夜间设为-100
        public int BackgroundTemp { get; set; }

    }
}
