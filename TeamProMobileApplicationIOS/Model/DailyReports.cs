using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TeamProMobileApplicationIOS.Model;

namespace TeamProMobileApplicationIOS.Internals
{
    public class DailyReports : ObservableCollection<Report>, IComparable<DailyReports>
    {
        public DateTime Date { get; set; }

        public TimeSpan TotalHours
        {
            get { return _totalHours; }
            set { if (value != _totalHours) { _totalHours = value; OnPropertyChanged(new PropertyChangedEventArgs("TotalHours")); } }
        }

        public DailyReports() : base() { }

        public DailyReports(DateTime date, TimeSpan totalHours, IEnumerable<Report> items)
            : base(items)
        {
            Date = date;
            TotalHours = totalHours;
        }

        public DailyReports(DateTime date, TimeSpan totalHours)
            : base()
        {
            Date = date;
            TotalHours = totalHours;
        }

        public Int32 CompareTo(DailyReports obj)
        {
            return Date.CompareTo(obj.Date);
        }

        private TimeSpan _totalHours;
    }
}