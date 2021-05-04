using EliteTimeSheetMobile.Model;
using EliteTimeSheetMobile.Persistance;
using EliteTimeSheetMobile.ViewModel;
using EliteTimeSheetMobile.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EliteTimeSheetMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeSheetList : ContentPage
    {
        private ITimeSheetStore _timeSheetStore;
        private TimeSheet _timeSheet;
        GeneratePDF generatePDF;
        List<TimeSheet> timeSheets;

        public TimeSheetList()
        {
            generatePDF = new GeneratePDF();
            timeSheets = new List<TimeSheet>();
            _timeSheetStore = new SQLiteTimeSheetStore(DependencyService.Get<ISQLiteDb>());
             InitializeComponent();
        }
        async protected override void OnAppearing()
        {
            var timesheets = await _timeSheetStore.GetTimeSheetAsync();
            timesheetListView.ItemsSource = timesheets.Reverse();
            if (timeSheets.Count > 0)
            {
                report_gen_icon.IsEnabled = true;
            }
            else
            {
                report_gen_icon.IsEnabled = false;
            }
            base.OnAppearing();
        }
        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            if (e.Value == true)
            {
               var selectedTimeSheet = checkbox.BindingContext as TimeSheet;
                timeSheets.Add(selectedTimeSheet);
            }
            else
            {
                var selectedTimeSheet = checkbox.BindingContext as TimeSheet;
                timeSheets.Remove(selectedTimeSheet);
            }
            if (timeSheets.Count > 0)
            {
                report_gen_icon.IsEnabled = true;
            }
            else
            {
                report_gen_icon.IsEnabled = false;
            }
        }
        async void NewTimeSheet_Clicked(object sender, System.EventArgs e)
        {
            var pageService = new PageService();
            await pageService.PushAsync(new TimeSheetEntry());

        }
         void GenerateTimesheet_Clicked(object sender, System.EventArgs e)
        {
            _ = generatePDF.CreatePDFAsync(timeSheets);
             timeSheets.Clear();
        }
      }
}