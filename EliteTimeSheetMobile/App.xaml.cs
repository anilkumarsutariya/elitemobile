using EliteTimeSheetMobile.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EliteTimeSheetMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Login())
            {
                BarBackgroundColor = Color.FromHex("#3F51B5"),
                BarTextColor = Color.White
             
            };
            //MainPage = new TimeSheetEntry();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
