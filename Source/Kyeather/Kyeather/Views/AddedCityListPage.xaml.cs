using Kyeather.Models;
using Kyeather.ViewModels;
using MvvmHelpers;
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
    public partial class AddedCityListPage : ContentPage
    {
        public AddedCityListPage(ObservableRangeCollection<City> addedCityList)
        {
            InitializeComponent();

            AddedCityListPageViewModel viewmodel = new AddedCityListPageViewModel(this.Navigation);
            viewmodel.AddedCityList = addedCityList;

            this.BindingContext = viewmodel;
        }


        protected override bool OnBackButtonPressed()
        {
            if ((BindingContext as AddedCityListPageViewModel).ShowDeleteButton)
            {
                (BindingContext as AddedCityListPageViewModel).ShowDeleteButton = false;
                (BindingContext as AddedCityListPageViewModel).EditText = "≡";
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
    }
}