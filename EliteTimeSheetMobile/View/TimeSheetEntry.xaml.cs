using System;
using EliteTimeSheetMobile.Persistance;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EliteTimeSheetMobile.Model;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Tables;
using System.Reflection;
using System.IO;
using EliteTimeSheetMobile.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignaturePad.Forms;
using System.Net.Http;
using System.Drawing;
using Android.Graphics;
using Color = Xamarin.Forms.Color;
using Rg.Plugins.Popup.Services;

namespace EliteTimeSheetMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeSheetEntry : ContentPage, TimeSheetEntry.ISaveImageDetails
    {
        int SignatureType = 0;
        private ITimeSheetStore _timeSheetStore;
        private TimeSheet _timeSheet;
        private string date;
        private string emp_signature_name = "";
        private string supervisor_signature_name = "";
        public TimeSheetEntry()
        {
            _timeSheetStore = new SQLiteTimeSheetStore(DependencyService.Get<ISQLiteDb>());
            InitializeComponent();
            SignatureType = 0;
            popupImageView.IsVisible = false;
            lay_sign_preview.IsVisible = false;
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

        private void SaveDate()
        {
            DateTime Intime = DateTime.Today + InTimePicker.Time;
            DateTime OutTime = DateTime.Today + OutTimePicker.Time;
            if (date == null)
            {
                date = DateTime.Now.ToString("MM - dd - yyyy");
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
        }

        async void reportButton_Clicked(object sender, System.EventArgs e)
        {
            SaveDate();
            var timesheets = await _timeSheetStore.GetTimeSheetAsync();
            CreatePDAsync(timesheets);
        }
        private async Task CreatePDAsync(IEnumerable<TimeSheet> timesheets)
        {
            foreach (var timeSheet in timesheets)
            {
                _timeSheet = timeSheet;
            }

            PdfDocument doc = new PdfDocument();
            //Adds a page.
            PdfPage page = doc.Pages.Add();

            //Acquires page's graphics 
            PdfGraphics graphics = page.Graphics;

            //Load the image from the disk

            //Get the images as stream
            Stream imageStream = typeof(TimeSheetEntry).GetTypeInfo().Assembly.GetManifestResourceStream("EliteTimeSheetMobile.Assets.logoelite.png");

            //Draw the image
            //Create a new PdfBitmap instance
            PdfBitmap image = new PdfBitmap(imageStream);

            //Draw the image
            page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(0, 0));

            //create a new PDF string format
            PdfStringFormat drawFormat = new PdfStringFormat();
            drawFormat.WordWrap = PdfWordWrapType.Word;
            drawFormat.Alignment = PdfTextAlignment.Justify;
            drawFormat.LineAlignment = PdfVerticalAlignment.Top;
            //Set the font.
            PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
            //Create a brush.
            PdfBrush brush = PdfBrushes.Black;
            //bounds
            Syncfusion.Drawing.RectangleF bounds = new Syncfusion.Drawing.RectangleF(new Syncfusion.Drawing.PointF(50, 60), new Syncfusion.Drawing.SizeF(page.Graphics.ClientSize.Width - 30, page.Graphics.ClientSize.Height - 20));
            //Create a new text elememt
            PdfTextElement element = new PdfTextElement("Name:", font, brush);
            //Set the string format
            element.StringFormat = drawFormat;
            //Draw the text element
            PdfLayoutResult result = element.Draw(page, bounds);

            ///////////////////////////////////////////////////////////////
            ///

            PdfTextElement name = new PdfTextElement(_timeSheet.Name, font, brush);
            //Set the string format
            name.StringFormat = drawFormat;
            //Draw the text element
            name.Draw(result.Page, new Syncfusion.Drawing.RectangleF(result.Bounds.X + 100, result.Bounds.Bottom - 15, result.Bounds.Width, result.Bounds.Height));

            ///////////////////////////////////////////////////////////////////

            PdfTextElement element1 = new PdfTextElement("Facility:", font, brush);
            //Set the string format
            element.StringFormat = drawFormat;
            //Draw the text element
            PdfLayoutResult result1 = element1.Draw(result.Page, new Syncfusion.Drawing.RectangleF(result.Bounds.X, result.Bounds.Bottom, result.Bounds.Width, result.Bounds.Height));


            PdfTextElement facility = new PdfTextElement(_timeSheet.Facility, font, brush);
            //Set the string format
            element.StringFormat = drawFormat;
            //Draw the text element
            facility.Draw(result.Page, new Syncfusion.Drawing.RectangleF(result.Bounds.X + 100, result.Bounds.Bottom, result.Bounds.Width, result.Bounds.Height));

            ////////////////////////////////////////////////////////////////////////

            // Draw the string one after another.
            // result = element.Draw(result.Page, new RectangleF(result.Bounds.X, result.Bounds.Bottom + 10, result.Bounds.Width, result.Bounds.Height));

            // Creates a PdfLightTable.
            PdfLightTable pdfLightTable = new PdfLightTable();
            //Add colums to light table
            pdfLightTable.Columns.Add(new PdfColumn("Date"));
            pdfLightTable.Columns.Add(new PdfColumn("Lunch"));
            pdfLightTable.Columns.Add(new PdfColumn("In Time"));
            pdfLightTable.Columns.Add(new PdfColumn("Out Time"));
            //Add row        
            pdfLightTable.Rows.Add(new string[] { _timeSheet.Date, _timeSheet.Lunch, _timeSheet.InTime, _timeSheet.OutTime });

            //Includes the style to display the header of the light table.
            pdfLightTable.Style.ShowHeader = true;

            //Draws PdfLightTable and returns the rendered bounds.
            result = pdfLightTable.Draw(page, new Syncfusion.Drawing.PointF(result.Bounds.Left, result.Bounds.Bottom + 20));
            //draw string with returned bounds from table

            ////  Draw the Signature Title

            PdfTextElement signatureTitle = new PdfTextElement("Employee Signature:", font, brush);
            //Set the string format
            element.StringFormat = drawFormat;

            result = signatureTitle.Draw(result.Page, result.Bounds.X, result.Bounds.Bottom + 10);

            //// Signature image

            //Get the images as stream
            //Stream imageStreamSig = typeof(TimeSheetEntry).GetTypeInfo().Assembly.GetManifestResourceStream("EliteTimeSheetMobile.Assets.signature.png");
            string path = await DependencyService.Get<ISave>().GetSignaturePath(emp_signature_name);
            Stream imageStreamSig = new FileStream(path, FileMode.Open);

            //Draw the image
            //Create a new PdfBitmap instance
            PdfBitmap imageSig = new PdfBitmap(imageStreamSig);

            //Draw the image
            //page.Graphics.DrawImage(imageSig, new Syncfusion.Drawing.PointF(result.Bounds.X + 150, result.Bounds.Y));
            page.Graphics.DrawImage(imageSig, new Syncfusion.Drawing.PointF(result.Bounds.X + 150, result.Bounds.Y), new Syncfusion.Drawing.SizeF(30, 30));
            ////////////////// Supervisior name Title
            PdfTextElement supervisiorTitle = new PdfTextElement("Supervisior Name:", font, brush);
            //Set the string format
            supervisiorTitle.StringFormat = drawFormat;

            result = supervisiorTitle.Draw(result.Page, result.Bounds.X, result.Bounds.Bottom + 30);

            ///// Supervisior name

            PdfTextElement supervisiorName = new PdfTextElement(_timeSheet.SupervisiorName, font, brush);
            //Set the string format
            supervisiorTitle.StringFormat = drawFormat;

            result = supervisiorName.Draw(result.Page, result.Bounds.X + 130, result.Bounds.Bottom - 15);

            /////////// Supervisior Signature

            PdfTextElement supervisiorSignTitle = new PdfTextElement("Supervisior Signature:", font, brush);
            //Set the string format
            element.StringFormat = drawFormat;

            result = supervisiorSignTitle.Draw(result.Page, result.Bounds.X - 130, result.Bounds.Bottom + 10);


            //////////// Supervisior Signature image
            Stream imageStreamSigSup = typeof(TimeSheetEntry).GetTypeInfo().Assembly.GetManifestResourceStream("EliteTimeSheetMobile.Assets.signature.png");

            string supersignpath = await DependencyService.Get<ISave>().GetSignaturePath(supervisor_signature_name);
            Stream imageStreamSuperVisorSig = new FileStream(supersignpath, FileMode.Open);

            //Draw the image
            //Create a new PdfBitmap instance
            PdfBitmap imageSupervisorSig = new PdfBitmap(imageStreamSuperVisorSig);

            //Draw the image
            //page.Graphics.DrawImage(imageSig, new Syncfusion.Drawing.PointF(result.Bounds.X + 150, result.Bounds.Y));
            page.Graphics.DrawImage(imageSupervisorSig, new Syncfusion.Drawing.PointF(result.Bounds.X + 150, result.Bounds.Y), new Syncfusion.Drawing.SizeF(30, 30));



            //draw string with returned bounds from table

            PdfTextElement commnets = new PdfTextElement("Commnets:", font, brush);
            //Set the string format
            commnets.StringFormat = drawFormat;
            commnets.Draw(result.Page, result.Bounds.X, result.Bounds.Bottom + 30);

            MemoryStream stream = new MemoryStream();
            //Saves the document.
            doc.Save(stream);
            doc.Close(true);

            stream.Position = 0;
            //Save the stream as a file in the device and invoke it for viewing
            DependencyService.Get<ISave>().SaveAndView("TimeSheet.pdf", "application/pdf", stream);

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
