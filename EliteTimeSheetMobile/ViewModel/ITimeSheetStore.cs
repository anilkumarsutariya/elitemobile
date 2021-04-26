using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EliteTimeSheetMobile.Model;

namespace EliteTimeSheetMobile.ViewModel
{
     interface ITimeSheetStore
    {

        Task<IEnumerable<TimeSheet>> GetTimeSheetAsync();
        Task AddContact(TimeSheet contact);

    }
}
