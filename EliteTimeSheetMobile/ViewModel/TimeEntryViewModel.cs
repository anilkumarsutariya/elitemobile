using System;
using System.Collections.Generic;
using System.Text;
using EliteTimeSheetMobile.View;
using EliteTimeSheetMobile.Model;
using System.Windows.Input;
using System.Threading.Tasks;
using EliteTimeSheetMobile.ViewModel;
using Xamarin.Forms;
using EliteTimeSheetMobile.Persistance;

namespace EliteTimeSheetMobile.ViewModel
{
     class TimeEntryViewModel
    {
        private readonly ITimeSheetStore _timeSheetStore;
        private readonly IPageService _pageService;
        private TimeSheetEntryViewModel timeSheetEntryViewModel;
        private SQLiteTimeSheetStore timeSheetStore;
        private PageService pageService;

        public TimeSheet TimeSheet { get; private set; }

        public ICommand SaveCommand { get; private set; }

         public TimeEntryViewModel(TimeSheetEntryViewModel viewModel, ITimeSheetStore timesheetStore, IPageService pageService)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _pageService = pageService;
            _timeSheetStore = timesheetStore;

            SaveCommand = new Command(async () => await Save());

            TimeSheet = new TimeSheet
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
  
            };
        }

        public TimeEntryViewModel(TimeSheetEntryViewModel timeSheetEntryViewModel, SQLiteTimeSheetStore timeSheetStore, PageService pageService)
        {
            this.timeSheetEntryViewModel = timeSheetEntryViewModel;
            this.timeSheetStore = timeSheetStore;
            this.pageService = pageService;
        }

        async Task Save()
        {
           
            if (TimeSheet.Id == 0)
            {
                await _timeSheetStore.AddContact(TimeSheet);
                //MessagingCenter.Send(this, Events.ContactAdded, Contact);
            }
           
            await _pageService.PopAsync();
        }
    }

}
