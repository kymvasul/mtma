using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using TeamProMobileApplicationIOS.Internals;
using TeamProMobileApplicationIOS.Model;


namespace TeamProMobileApplicationIOS
{
	public class CollectionSource: UICollectionViewSource
	{
		public CollectionSource ()		{		}

		public CollectionSource (DateTime date)
		{
			_dateInMonth = date;
			AddDatesToCalendar(date);
			CalendarView.lblMonth.Text = (date).ToString ("MMMM") + " "+ date.Year.ToString();
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var cell = (CollectionItem)collectionView.DequeueReusableCell (CalendarView.ViewCellId, indexPath);
			DateTime date = month[indexPath.Row];

			if (date.ToShortDateString ().Equals (DateTime.Now.ToShortDateString ())) {
				cell.UpdateCellCurrentDate ();
			} else {
				cell.UpdateCellNotCurrentDate ();
			}

			DailyReports dailyReport = null;
			foreach (DailyReports dailyReports in ReportsListScreen.list) {
				if (dailyReports.Date.ToShortDateString () == date.ToShortDateString ()) {
					dailyReport = dailyReports;
				}
			} 

			if (date.Month != _dateInMonth.Month || date.Year != _dateInMonth.Year) {
				cell.UpdateCellEnotherMonth ();
				} else {
					cell.UpdateCellColorAllCells ();
				}

			cell.UpdateCell (date, dailyReport);
			return cell;
		}

		public override int GetItemsCount (UICollectionView collectionView, int section) {
			return month.Count;
		}

		public override void ItemSelected (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath){
			var cell = (CollectionItem)collectionView.DequeueReusableCell (CalendarView.ViewCellId, indexPath);
			DateTime date = month[indexPath.Row];
			SortedObservableCollection<DailyReports> dayReportsList = new SortedObservableCollection<DailyReports>();
			foreach (DailyReports dailyReports in ReportsListScreen.list) {
				if (dailyReports.Date.ToShortDateString () == date.ToShortDateString ()) {
					dayReportsList.Add (dailyReports);
					CalendarView.dayTableView.Hidden = false;
				}
			} 
			if (dayReportsList.Count > 0) {
				if (_systemVersion < 7.0) {
					CalendarView.dayTableView.Frame = new RectangleF (10, 280, 300, 130);
				}
				if (_systemVersion >= 7.0) {
					CalendarView.dayTableView.Frame = new RectangleF (10, 360, 300, 130);
				}
				CalendarView.dayTableView.Source = new DayTableSource (dayReportsList);
				CalendarView.dayTableView.ReloadData ();
			} else {
				CalendarView.dayTableView.Frame = new RectangleF (10, 270, 0, 0);
				CalendarView.dayTableView.Source = null;
			}
			if (date.Month != _dateInMonth.Month || date.Year != _dateInMonth.Year) 
			{
				if (date.Month > _dateInMonth.Month)
				{
					CalendarView.swipedToNext (date);
				}
				if (date.Month < _dateInMonth.Month)
				{
					CalendarView.swipedToPrevious (date);
				}
			}
		}

		private void AddDatesToCalendar(DateTime currentDate){
			month = new List<DateTime> ();
			int currentYear = currentDate.Year;
			int currentMonth = currentDate.Month;
			int currentDay = currentDate.Day;
			int daysInCurrentMonth = DateTime.DaysInMonth (currentYear, currentMonth);

			DateTime firstDayInMonth;
			if (currentDay == 1) {
				firstDayInMonth = currentDate;
			} else {
				firstDayInMonth = new DateTime (currentYear, currentMonth, 1);
			}
			int monthShiftStart = MonthShift.GetShiftForMonthBefore (firstDayInMonth.DayOfWeek);


			int monthBefore = 0;
			int daysInMonthBefore = 0;
			if (currentMonth == 1) {
				monthBefore = 12;
				daysInMonthBefore = DateTime.DaysInMonth (currentYear - 1, monthBefore);
				firstDayAtThePreviousMonth = new DateTime (currentYear-1, monthBefore, 1);
			} else {
				monthBefore = currentMonth - 1;
				daysInMonthBefore = DateTime.DaysInMonth (currentYear, monthBefore);
				firstDayAtThePreviousMonth = new DateTime (currentYear, monthBefore, 1);
			}
			int start = daysInMonthBefore + 1 - monthShiftStart;

			int monthAfter = currentMonth + 1;

			if (monthAfter > 12) {
				firstDayAtTheNextMonth = new DateTime (currentYear + 1, 1, 1);
			} else {
				firstDayAtTheNextMonth = new DateTime (currentYear, monthAfter, 1);
			}


			DateTime lastDayInMonth= new DateTime (currentYear, currentMonth, daysInCurrentMonth);
			int monthShiftEnd = MonthShift.GetShiftForMonthAfter (lastDayInMonth.DayOfWeek);
			int day = 1;

			for (int i = 1; i <= daysInCurrentMonth + monthShiftStart + monthShiftEnd; i++) {
				if (i <= monthShiftStart) {
					if (monthBefore == 12) {
						month.Add (new DateTime (currentYear-1, monthBefore, start++));
					} else {
						month.Add (new DateTime (currentYear, monthBefore, start++));
					}
				} else {
					if(i>(monthShiftStart + daysInCurrentMonth)){
						if (monthAfter > 12) {
							month.Add (new DateTime (currentYear + 1, 1, day++));
						} else {
							month.Add (new DateTime (currentYear, monthAfter, day++));
						}
					}
					else{
						month.Add (new DateTime (currentYear, currentMonth, i-monthShiftStart));
					}
				}

			}
		}

		public List<DateTime> month;
		public static DateTime firstDayAtTheNextMonth;
		public static DateTime firstDayAtThePreviousMonth;

		private float _systemVersion = AppDelegate.versionIOSFloat;
		private DateTime _dateInMonth;
		private static NSString _CellId = new NSString ("CollectionItem");
	}
}

