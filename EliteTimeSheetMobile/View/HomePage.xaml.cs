using EliteTimeSheetMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Tables;
using System.Reflection;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EliteTimeSheetMobile.Model;
using EliteTimeSheetMobile.Persistance;

namespace EliteTimeSheetMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private ITimeSheetStore _timeSheetStore;
        private TimeSheet _timeSheet;
        public HomePage()
        {
            _timeSheetStore = new SQLiteTimeSheetStore(DependencyService.Get<ISQLiteDb>());
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
        async void timeSheetButton_Clicked(object sender, System.EventArgs e)
        {
            var pageService = new PageService();
            await pageService.PushAsync(new TimeSheetEntry());
        }
        async void reportButton_Clicked(object sender, System.EventArgs e)
        {
            var pageService = new PageService();
            await pageService.PushAsync(new TimeSheetList());
        }
    }
}