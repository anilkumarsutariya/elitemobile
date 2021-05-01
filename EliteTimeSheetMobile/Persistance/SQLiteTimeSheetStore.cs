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
            return await _connection.Table<TimeSheet>().ToListAsync() ;
        }
        public async Task AddTimeSheet(TimeSheet timeSheet)
        {
            int count = await _connection.InsertAsync(timeSheet);
            Console.WriteLine("COUNT",count);
        }
    }
}
