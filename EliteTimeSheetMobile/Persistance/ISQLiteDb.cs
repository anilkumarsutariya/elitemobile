using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EliteTimeSheetMobile.Persistance
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
