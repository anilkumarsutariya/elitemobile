using EliteTimeSheetMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EliteTimeSheetMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
        async void timeSheetButton_Clicked(object sender, System.EventArgs e)
        {
            var pageService = new PageService();
            await pageService.PushAsync(new TimeSheetEntry());
        }
    }
}