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
using EliteTimeSheetMobile.Helper;

namespace EliteTimeSheetMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeSheetEntry : ContentPage
    {
        int SignatureType = 0;
        private ITimeSheetStore _timeSheetStore;
        private string date;
        GeneratePDF generatePDF;
        List<TimeSheet> timeSheetsEntry;
        public TimeSheetEntry()
        {
            _timeSheetStore = new SQLiteTimeSheetStore(DependencyService.Get<ISQLiteDb>());
             timeSheetsEntry = new List<TimeSheet>();
             generatePDF = new GeneratePDF();
             InitializeComponent();
             SignatureType = 0;
             popupImageView.IsVisible = false;
        }
        private void MainDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            date = e.NewDate.ToString("MM-dd-yyyy");
        }
        void saveButton_Clicked(object sender, System.EventArgs e)
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
                EmpSignature = "Signature",
                SupSignature = "SupSignature"
            };

            _ = _timeSheetStore.AddTimeSheet(timesheet);

                timeSheetsEntry.Add(timesheet);
        }

        async void reportButton_Clicked(object sender, System.EventArgs e)
        {
                generatePDF.CreatePDFAsync(timeSheetsEntry);
                timeSheetsEntry.Clear();
        }
        private void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            popupSignatureView.IsVisible = true;
            signatureSample.CaptionText = "Employee Signature";
            signatureSample.Clear();
            SignatureType = 1;

        }

        private void btnPopupSupervisor_Clicked(object sender, EventArgs e)
        {
            popupSignatureView.IsVisible = true;
            signatureSample.CaptionText = "Supervisor Signature";
            signatureSample.Clear();
            SignatureType = 2;
        }
        private void btnPopupClose_Clicked(object sender, EventArgs e)
        {
            popupSignatureView.IsVisible = false;

        }
        private void btnPopupSave_Clicked(object sender, EventArgs e)
        {

            popupSignatureView.IsVisible = false;
            _ = NewMethod();

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

        private async Task NewMethod()
        {
            try
            {

                String path;
                using (var bitmap = await signatureSample.GetImageStreamAsync(SignatureImageFormat.Png, Color.Black, Color.White, 1f))
                {
                    if (SignatureType == 1)
                    {
                        path = await DependencyService.Get<ISave>().SaveSignature(bitmap, "empsignature.png");
                    }
                    else
                    {
                        path = await DependencyService.Get<ISave>().SaveSignature(bitmap, "supervisorsignature.png");
                    }

                }

                if (path.Trim().Length > 0)
                {
                    if (SignatureType == 1)
                    {
                        img_Employee_Sign.Source = ImageSource.FromFile(path);
                    }
                    else
                    {
                        img_Supervisior_Sign.Source = ImageSource.FromFile(path);
                    }

                    //await DisplayAlert("Signature Pad", "Raster signature saved to the photo library.", "OK");
                }
                else
                    await DisplayAlert("Signature Pad", "There was an error saving the signature.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Signature Pad", "There was an error saving the signature.", "OK");

                ex.ToString();
            }

        }

    }
}
