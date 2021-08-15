using System;
using System.Collections.Generic;
using System.Text;

namespace Kyeather.Models
{
    public class CityWeatherInfoOfDay
    {
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public string Moonrise { get; set; }
        public string Moonset { get; set; }
        public string MoonPhase { get; set; }
        public int TempMax { get; set; }
        public int TempMin { get; set; }
        public int IconDay { get; set; }
        public string TextDay { get; set; }
        public int IconNight { get; set; }
        public string TextNight { get; set; }
        public int Wind360Day { get; set; }
        public string WindDirDay { get; set; }
        public string WindScaleDay { get; set; }
        public int WindSpeedDay { get; set; }
        public int Wind360Night { get; set; }
        public string WindDirNight { get; set; }
        public string WindScaleNight { get; set; }
        public int WindSpeedNight { get; set; }
        public int Humidity { get; set; }
        public double Precip { get; set; }
        public int Pressure { get; set; }
        public int Vis { get; set; }
        public int Cloud { get; set; }
        public int UvIndex { get; set; }
    }
}
