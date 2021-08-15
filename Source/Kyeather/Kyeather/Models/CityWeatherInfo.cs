using System;
using System.Collections.Generic;
using System.Text;

namespace Kyeather.Models
{
    public class CityWeatherInfo
    {
        public string UpdateTime { get; set; }
        public int Temp { get; set; }
        public int FeelsLike { get; set; }
        public int Icon { get; set; }
        public string Weather { get; set; }
        public int Wind360 { get; set; }
        public string WindDir { get; set; }
        public int WindScale { get; set; }
        public int WindSpeed { get; set; }
        public int Humidity { get; set; }
        public double Precip { get; set; }
        public int Pressure { get; set; }
        public int Vis { get; set; }
        public int Cloud { get; set; }
        public int Dew { get; set; }
    }
}
