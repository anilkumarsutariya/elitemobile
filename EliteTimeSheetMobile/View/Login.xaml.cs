using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Preferences;
using EliteTimeSheetMobile.Persistance;
using EliteTimeSheetMobile.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EliteTimeSheetMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private readonly IPageService _pageService;
        public Login()
        {
    
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LogoImage.Source = ImageSource.FromResource("EliteTimeSheetMobile.Assets.logo.png");
        }

        async void loginButton_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.Properties["username"] = userName.Text;

           
            var pageService = new PageService();
            await Navigation.PushAsync(new HomePage());
            //await pageService.PushAsync(new HomePage());
        }
    }
}