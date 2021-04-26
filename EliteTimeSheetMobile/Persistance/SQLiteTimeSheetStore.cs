using EliteTimeSheetMobile.Model;
using EliteTimeSheetMobile.ViewModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EliteTimeSheetMobile.Persistance
{
    class SQLiteTimeSheetStore : ITimeSheetStore
    {

        private SQLiteAsyncConnection _connection;

        public SQLiteTimeSheetStore(ISQLiteDb db)
        {
            _connection = db.GetConnection();
            _connection.CreateTableAsync<TimeSheet>();
        }

        public async Task<IEnumerable<TimeSheet>> GetTimeSheetAsync()

        {

            //var timesheets = await _connection.Table<TimeSheet>().ToListAsync();
            //  Console.WriteLine("time sheet", timesheets);
            //return timesheets;

            return await _connection.Table<TimeSheet>().ToListAsync(); ;

        }

        public async Task AddContact(TimeSheet contact)
        {
            int count = await _connection.InsertAsync(contact);
            Console.WriteLine("COUNT",count);
        }

    }
}
