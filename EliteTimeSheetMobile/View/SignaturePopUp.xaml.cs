using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SignaturePad.Forms;
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
    public partial class SignaturePopUp : PopupPage
    {

        int SignatureType = 0;
        string SignatureFilename = "";
        TimeSheetEntry objTimeSheetEntry;
        public SignaturePopUp( int signaturetype, TimeSheetEntry obj)
        {
            InitializeComponent();
            SignatureType = signaturetype;
            objTimeSheetEntry = obj;
            if (SignatureType == 2)
            {
                signatureSample.CaptionText = "Supervisor Signature";
                signatureSample.Clear();
            }
            else{
                signatureSample.CaptionText = "Employee Signature";
                signatureSample.Clear();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "allowLandScapePortrait");
      }
        //during page close setting back to portrait
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "preventLandScape");
        }

        private async void btnImagePopupClose_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        [Obsolete]
        private async void btnImagePopupSave_Clicked(object sender, EventArgs e)
        {

            String path;
            using (var bitmap = await signatureSample.GetImageStreamAsync(SignatureImageFormat.Png, Color.Black, Color.White, 1f))
            {
                if (SignatureType == 1)
                {
                    SignatureFilename = "empsign" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".png";
                    //path = await DependencyService.Get<ISave>().SaveSignature(bitmap, "empsignature.png");
                    path = await DependencyService.Get<ISave>().SaveSignature(bitmap, SignatureFilename);
                }
                else
                {
                    SignatureFilename = "supervisorsig" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".png";
                    path = await DependencyService.Get<ISave>().SaveSignature(bitmap, SignatureFilename);
                    //path = await DependencyService.Get<ISave>().SaveSignature(bitmap, "supervisorsignature.png");
                }

            }
            objTimeSheetEntry.GetSignaturePath(SignatureType, path, SignatureFilename);
            await PopupNavigation.PopAsync(true);
        }
        
    }
}