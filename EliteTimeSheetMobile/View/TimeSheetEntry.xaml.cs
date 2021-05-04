using System;
using EliteTimeSheetMobile.Persistance;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EliteTimeSheetMobile.Model;
using EliteTimeSheetMobile.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignaturePad.Forms;
using System.Net.Http;
using System.Drawing;
//using Android.Graphics;
using Color = Xamarin.Forms.Color;
using Rg.Plugins.Popup.Services;
using EliteTimeSheetMobile.Helper;

namespace EliteTimeSheetMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeSheetEntry : ContentPage, TimeSheetEntry.ISaveImageDetails
    {
        int SignatureType = 0;
        private ITimeSheetStore _timeSheetStore;
        private string date;
        private string emp_signature_name = "";
        private string supervisor_signature_name = "";
        GeneratePDF generatePDF;
        List<TimeSheet> timeSheetsEntry;
        public TimeSheetEntry()
        {
            _timeSheetStore = new SQLiteTimeSheetStore(DependencyService.Get<ISQLiteDb>());
            InitializeComponent();
            SignatureType = 0;
            lay_sign_preview.IsVisible = false;
             timeSheetsEntry = new List<TimeSheet>();
             generatePDF = new GeneratePDF();
            MainDatePicker.SetValue(DatePicker.MaximumDateProperty, DateTime.Now);

        }
        private void MainDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            date = e.NewDate.ToString("MM-dd-yyyy");
        }
        async void saveButton_Clicked(object sender, System.EventArgs e)
        {
            SaveDate();
            _ = await Navigation.PopAsync(true);
        }

        private TimeSheet SaveDate()
        {
            DateTime Intime = DateTime.Today + InTimePicker.Time;
            DateTime OutTime = DateTime.Today + OutTimePicker.Time;
            if (date == null)
            {
                date = DateTime.Now.ToString("dd-MM-yyyy");
            }

            TimeSheet timesheet = new TimeSheet()
            {
                Name = name.Text,
                Facility = facility.Text,
                SupervisiorName = supervisiorName.Text,
                Date = date,
                InTime = string.Format("{0: hh:mm tt}", Intime),
                OutTime = string.Format("{0: hh:mm tt}", OutTime),
                Lunch = lunch.Text,
                Comments = comments.Text,
                EmpSignature = emp_signature_name,
                SupSignature = supervisor_signature_name
            };

            _ = _timeSheetStore.AddTimeSheet(timesheet);

              return timesheet;
        }

           void reportButton_Clicked(object sender, System.EventArgs e)
        {
            TimeSheet timesheet = SaveDate();
            timeSheetsEntry.Clear();
            timeSheetsEntry.Add(timesheet);
            _ = generatePDF.CreatePDFAsync(timeSheetsEntry);
                timeSheetsEntry.Clear();
        }
        private async void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new SignaturePopUp(1, this));
           
            SignatureType = 1;

        }

        private async void btnPopupSupervisor_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new SignaturePopUp(2, this));
            
            SignatureType = 2;
        }
      

        private void btnImagePopupClose_Clicked(object sender, EventArgs e)
        {
            popupImageView.IsVisible = false;
        }

        private void OnEmployee_SignTapped(object sender, EventArgs e)
        {
            popupImageView.IsVisible = true;
            img_Preview_Sign.Source = img_Employee_Sign.Source;

        }
        
        private void OnSupervisior_SignTapped(object sender, EventArgs e)
        {
            popupImageView.IsVisible = true;
            img_Preview_Sign.Source = img_Supervisior_Sign.Source;

        }



        public void GetSignaturePath(int SignatureType, string SignaturePath, string SignatureName)
        {
            if (SignaturePath.Trim().Length > 0)
            {
                if (SignatureType == 1)
                {
                    lay_sign_preview.IsVisible = true;
                    img_Employee_Sign.Source = ImageSource.FromFile(SignaturePath);
                    emp_signature_name = SignatureName;
                }
                else
                {
                    lay_sign_preview.IsVisible = true;
                    img_Supervisior_Sign.Source = ImageSource.FromFile(SignaturePath);
                    supervisor_signature_name = SignatureName;
                }

                //await DisplayAlert("Signature Pad", "Raster signature saved to the photo library.", "OK");
            }
        }

        public interface ISaveImageDetails
        {
            void GetSignaturePath(int SignatureType, String SignaturePath, String SignatureName);

        }

    }
}
