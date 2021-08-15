using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Kyeather.Droid.Services;
using Kyeather.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(HomeButtonSimulator))]
namespace Kyeather.Droid.Services
{
    class HomeButtonSimulator : IHomeButtonSimulator
    {
        public void SimulateHomeButton()
        {
            var activity = (Activity)Forms.Context;
            activity.MoveTaskToBack(true);
        }
    }
}