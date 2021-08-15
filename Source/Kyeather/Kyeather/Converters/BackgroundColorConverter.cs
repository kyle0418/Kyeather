using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kyeather.Converts
{
    public class BackgroundColorConverter : IValueConverter
    {
        public bool IsStart { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int temp)
            {
                if (temp == -100)
                {
                    return IsStart ? Application.Current.Resources["NightStartColor"] : Application.Current.Resources["NightEndColor"];
                }
                if (temp >= 30)
                {
                    return IsStart ? Application.Current.Resources["HotStartColor"] : Application.Current.Resources["HotEndColor"];
                }
                if (15 < temp && temp < 30)
                {
                    return IsStart ? Application.Current.Resources["CoolStartColor"] : Application.Current.Resources["CoolEndColor"];
                }
                if (0 < temp && temp <= 15)
                {
                    return IsStart ? Application.Current.Resources["ColdStartColor"] : Application.Current.Resources["ColdEndColor"];
                }
                return IsStart ? Application.Current.Resources["IceStartColor"] : Application.Current.Resources["IceEndColor"];
            }
            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //public object ProvideValue(IServiceProvider serviceProvider)
        //{
        //    return this;
        //}
    }
}
