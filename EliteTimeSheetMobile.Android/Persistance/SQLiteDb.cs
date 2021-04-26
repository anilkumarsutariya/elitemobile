using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using Xamarin.Forms;
using System.IO;
using EliteTimeSheetMobile.Droid.Persistance;
using EliteTimeSheetMobile.Persistance;

[assembly: Dependency(typeof(SQLiteDb))]

namespace EliteTimeSheetMobile.Droid.Persistance
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "EliteTimeSheet.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}