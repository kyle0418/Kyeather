using Kyeather.API;
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
    public partial class AddCityPage : ContentPage
    {
        public AddCityPage()
        {
            InitializeComponent();
            this.BindingContext = new AddCityPageViewModel(this.Navigation);
        }
    }
}