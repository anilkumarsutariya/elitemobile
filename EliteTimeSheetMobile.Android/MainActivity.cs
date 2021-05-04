using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using EliteTimeSheetMobile.View;

namespace EliteTimeSheetMobile.Droid
{
    [Activity(Label = "EliteTimeSheetMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme",  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            MessagingCenter.Subscribe<SignaturePopUp>(this, "allowLandScapePortrait", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });

            //during page close setting back to portrait
            MessagingCenter.Subscribe<SignaturePopUp>(this, "preventLandScape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 63, 81, 181));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}