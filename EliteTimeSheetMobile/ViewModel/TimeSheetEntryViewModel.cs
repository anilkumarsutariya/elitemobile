using System;
using System.Collections.Generic;
using System.Text;
using EliteTimeSheetMobile.Model;
using Xamarin.Forms;

namespace EliteTimeSheetMobile.ViewModel
{
    public class TimeSheetEntryViewModel: BaseViewModel
    {
		public int Id { get; set; }
		public TimeSheetEntryViewModel(TimeSheet timeSheet)
		{
			  Id = timeSheet.Id;
              Name = timeSheet.Name;
              Facility = timeSheet.Facility;
		}
        public TimeSheetEntryViewModel()
        {}
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetValue(ref _name, value);
            }
        }

        private string _facility;
        public string Facility
        {
            get { return _facility; }
            set
            {
                SetValue(ref _facility, value);
            }
        }
    }
}
