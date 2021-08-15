using Kyeather.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Kyeather.API
{
    class HefengWeatherAPI
    {
        public static string key = "";

        public static List<City> GetCityList(string location)
        {
            string sURL = $"https://geoapi.qweather.com/v2/city/lookup?location={location}&number=20&key={key}";
            HttpWebRequest wrGetURL = (HttpWebRequest)WebRequest.Create(sURL);
            wrGetURL.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            StreamReader objReader = new StreamReader(wrGetURL.GetResponse().GetResponseStream(), Encoding.UTF8);
            string jsonstring = objReader.ReadToEnd();

            if (!jsonstring.Contains("\"code\":\"200\""))
                return null;

            Regex reg = new Regex("{\"name\":\"(.*?)\",\"id\":\"(.*?)\".*?\"adm2\":\"(.*?)\",\"adm1\":\"(.*?)\",\"country\":\"(.*?)\".*?}");
            MatchCollection cityCollection = reg.Matches(jsonstring);
            List<City> citylist = new List<City>();
            foreach (Match match in cityCollection)
            {

                string name = match.Groups[1].Value;
                string id = match.Groups[2].Value;
                string adm2 = match.Groups[3].Value;
                string adm1 = match.Groups[4].Value;
                string country = match.Groups[5].Value;
                citylist.Add(new City
                {
                    Name = match.Groups[1].Value,
                    Id = match.Groups[2].Value,
                    Adm2 = match.Groups[3].Value,
                    Adm1 = match.Groups[4].Value,
                    Country = match.Groups[5].Value,
                });
            }

            return citylist;
        }

        public static CityWeatherInfo GetCityInfoNow(string Id)
        {
            string sURL = $"https://devapi.qweather.com/v7/weather/now?location={Id}&key={key}";
            HttpWebRequest wrGetURL = (HttpWebRequest)WebRequest.Create(sURL);
            wrGetURL.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            StreamReader objReader = new StreamReader(wrGetURL.GetResponse().GetResponseStream(), Encoding.UTF8);
            string jsonstring = objReader.ReadToEnd();

            if (!jsonstring.Contains("\"code\":\"200\""))
                return null;

            Regex reg = new Regex("\"now\":{(.*?)}");
            Match match = reg.Match(jsonstring);
            string cityinfo = match.Groups[1].Value;
            CityWeatherInfo cityWeatherInfo = new CityWeatherInfo()
            {
                Temp = Convert.ToInt32(cityinfo.Split(',')[1].Split(':')[1].Trim('"')),
                FeelsLike = Convert.ToInt32(cityinfo.Split(',')[2].Split(':')[1].Trim('"')),
                Icon = Convert.ToInt32(cityinfo.Split(',')[3].Split(':')[1].Trim('"')),
                Weather = cityinfo.Split(',')[4].Split(':')[1].Trim('"'),
                Wind360 = Convert.ToInt32(cityinfo.Split(',')[5].Split(':')[1].Trim('"')),
                WindDir = cityinfo.Split(',')[6].Split(':')[1].Trim('"'),
                WindScale = Convert.ToInt32(cityinfo.Split(',')[7].Split(':')[1].Trim('"')),
                WindSpeed = Convert.ToInt32(cityinfo.Split(',')[8].Split(':')[1].Trim('"')),
                Humidity = Convert.ToInt32(cityinfo.Split(',')[9].Split(':')[1].Trim('"')),
                Precip = Convert.ToDouble(cityinfo.Split(',')[10].Split(':')[1].Trim('"')),
                Pressure = Convert.ToInt32(cityinfo.Split(',')[11].Split(':')[1].Trim('"')),
                Vis = Convert.ToInt32(cityinfo.Split(',')[12].Split(':')[1].Trim('"')),
                Cloud = Convert.ToInt32(cityinfo.Split(',')[13].Split(':')[1].Trim('"')),
                Dew = Convert.ToInt32(cityinfo.Split(',')[14].Split(':')[1].Trim('"'))
            };

            return cityWeatherInfo;
        }

        public static List<CityWeatherInfoOfDay> GetCityInfo3Days(string Id)
        {
            string sURL = $"https://devapi.qweather.com/v7/weather/3d?location={Id}&key={key}";
            HttpWebRequest wrGetURL = (HttpWebRequest)WebRequest.Create(sURL);
            wrGetURL.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            StreamReader objReader = new StreamReader(wrGetURL.GetResponse().GetResponseStream(), Encoding.UTF8);
            string jsonstring = objReader.ReadToEnd();

            if (!jsonstring.Contains("\"code\":\"200\""))
                return null;

            Regex reg = new Regex("{\"fxDate\":(.*?)}");
            MatchCollection cityCollection = reg.Matches(jsonstring);
            List<CityWeatherInfoOfDay> cityInfoOfDayList = new List<CityWeatherInfoOfDay>();
            string newmatch;
            char[] charsToTrim = { '{', '}' };

            TimeSpan rise;
            TimeSpan set;
            TimeSpan now;
            foreach (Match match in cityCollection)
            {
                newmatch = match.Value.Trim(charsToTrim);

                now = DateTime.Now.TimeOfDay;
                rise = new TimeSpan(Convert.ToInt32(newmatch.Split(',')[1].Split(':')[1].Trim('"')), Convert.ToInt32(newmatch.Split(',')[1].Split(':')[2].Trim('"')), 0);
                set = new TimeSpan(Convert.ToInt32(newmatch.Split(',')[2].Split(':')[1].Trim('"')), Convert.ToInt32(newmatch.Split(',')[2].Split(':')[2].Trim('"')), 0);
                cityInfoOfDayList.Add(new CityWeatherInfoOfDay
                {
                    Sunrise = newmatch.Split(',')[1].Split(':')[1].Trim('"') + ":" + newmatch.Split(',')[1].Split(':')[2].Trim('"'),
                    Sunset = newmatch.Split(',')[2].Split(':')[1].Trim('"') + ":" + newmatch.Split(',')[2].Split(':')[2].Trim('"'),
                    TempMax = Convert.ToInt32(newmatch.Split(',')[6].Split(':')[1].Trim('"')),
                    TempMin = Convert.ToInt32(newmatch.Split(',')[7].Split(':')[1].Trim('"')),
                    IconDay = Convert.ToInt32(newmatch.Split(',')[8].Split(':')[1].Trim('"')),
                    TextDay = newmatch.Split(',')[9].Split(':')[1].Trim('"'),
                    IconNight = Convert.ToInt32(newmatch.Split(',')[10].Split(':')[1].Trim('"')),
                    TextNight = newmatch.Split(',')[11].Split(':')[1].Trim('"'),
                    WindDirDay = newmatch.Split(',')[13].Split(':')[1].Trim('"'),
                    WindScaleDay = newmatch.Split(',')[14].Split(':')[1].Trim('"'),
                    WindDirNight = newmatch.Split(',')[17].Split(':')[1].Trim('"'),
                    WindScaleNight = newmatch.Split(',')[18].Split(':')[1].Trim('"'),
                });
            }

            return cityInfoOfDayList;
        }

        public static City GetCityByLatAndLong(string latitude, string longitude)
        {
            try
            {
                string sURL = $"https://geoapi.qweather.com/v2/city/lookup?location={longitude},{latitude}&key={key}";
                HttpWebRequest wrGetURL = (HttpWebRequest)WebRequest.Create(sURL);
                wrGetURL.Timeout = 1500;
                wrGetURL.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                StreamReader objReader = new StreamReader(wrGetURL.GetResponse().GetResponseStream(), Encoding.UTF8);
                string jsonstring = objReader.ReadToEnd();

                if (!jsonstring.Contains("\"code\":\"200\""))
                    return null;


                Regex reg = new Regex("\"name\":\"(.*?)\",\"id\":\"(.*?)\"");
                Match match = reg.Match(jsonstring);
                string name = match.Groups[1].Value;
                string id = match.Groups[2].Value;
                City city = new City()
                {
                    Name = name,
                    Id = id
                };

                return city;
            }
            catch
            {
                return null;
            }
        }
    }
}
