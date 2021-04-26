using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        async void loginButton_Clicked(object sender, System.EventArgs e)
        {
            var pageService = new PageService();
            await pageService.PushAsync(new TimeSheetEntry());
        }
    }
}