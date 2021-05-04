using System;
using System.IO;
using Android.Content;
using Java.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using EliteTimeSheetMobile;
using AndroidX.Core.Content;
using AndroidX.Core.App;
using Android.App;
using EliteTimeSheetMobile.Droid;

[assembly: Dependency(typeof(SaveAndroid))]

class SaveAndroid : ISave
{
    //Method to save document as a file in Android and view the saved document
    public async Task SaveAndView(string fileName, String contentType, MemoryStream stream)
    {
        string exception = string.Empty;
        string root = null;

        if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions((Android.App.Activity)Forms.Context, new String[] { Manifest.Permission.WriteExternalStorage }, 1);
        }

        if (Android.OS.Environment.IsExternalStorageEmulated)
        {
            root = Android.OS.Environment.ExternalStorageDirectory.ToString();
        }
        else
            root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        Java.IO.File myDir = new Java.IO.File(root + "/Syncfusion");
        myDir.Mkdir();

        Java.IO.File file = new Java.IO.File(myDir, fileName);

        bool fileExists = file.Exists();

        if (file.Exists()) file.Delete();

        try
        {
            FileOutputStream outs = new FileOutputStream(file);
            outs.Write(stream.ToArray());

            outs.Flush();
            outs.Close();
        }
        catch (Exception e)
        {
            TaskCompletionSource<bool> taskCompletionSource;
            taskCompletionSource = new TaskCompletionSource<bool>();

            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(Forms.Context);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("Alert");
            alert.SetMessage("Please give file acess permission. ");
            alert.SetButton("OK", (c, ev) =>
            {
                Xamarin.Forms.Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission));
                taskCompletionSource.SetResult(true);
            });
          
            alert.Show();
            exception = e.ToString();
       
    }
        if (file.Exists() && contentType != "application/html")
        {
            string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
            string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            Android.Net.Uri path = FileProvider.GetUriForFile(Forms.Context, Android.App.Application.Context.PackageName + ".provider", file);
            intent.SetDataAndType(path, mimeType);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
        }
    }

    public async Task<String> SaveSignature(Stream bitmap, string filename)
    {
        if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions((Android.App.Activity)Forms.Context, new String[] { Manifest.Permission.WriteExternalStorage }, 1);
        }
        var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
        var file = Path.Combine(path, filename);
        try
        {
            using (var dest = System.IO.File.OpenWrite(file))
            {
                await bitmap.CopyToAsync(dest);
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
            file = "";
        }
        return file;
    }

    public async Task<String> GetSignaturePath(string filename)
    {
        var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
        var file = Path.Combine(path, filename);
        return file;
    }

}
