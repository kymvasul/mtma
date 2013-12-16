using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TeamProMobileApplicationIOS.Internals;
using TeamProMobileApplicationIOS.Model;
using System.Runtime.CompilerServices;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TeamProMobileApplicationIOS
{
	public class DailyMonthlyViewModel : INotifyPropertyChanged
	{
		UIViewController owner;

		public delegate void OnDataLoaded ();
		public event OnDataLoaded DataLoaded;

		#region Private Fields

		//private UserContext _user;
		private readonly IDataManager _dManager;
		private DateTime _selectedMonth;
		private DateTime _selectedDay;

		//		private Visibility _monthlyListVisibility;
		//		private Visibility _dailyListVisibility;
		private Boolean _isEmptyMonthlyResult;
		private Boolean _isEmptyDailyResult;

		private Boolean _isProgressOn;
		private Boolean _isDataLoad;
		private Int32 _callsCount;

		#endregion Private Fields

		#region Public Properties

		public SortedObservableCollection<DailyReports> DailyItems { get; set; }

		public SortedObservableCollection<DailyReports> MonthlyItems { get; set; }

		public Boolean IsDataLoaded
		{
			get { return _isDataLoad; }
			private set
			{
				_isDataLoad = value;
				IsProgressOn = !_isDataLoad;
			}
		}

		//		public Visibility MonthlyListVisibility
		//		{
		//			get
		//			{
		//				return _monthlyListVisibility;
		//			}
		//			set
		//			{
		//				if (_monthlyListVisibility != value)
		//				{
		//					_monthlyListVisibility = value;
		//					OnPropertyChanged();
		//				}
		//			}
		//		}
		//
		//		public Visibility DailyListVisibility
		//		{
		//			get
		//			{
		//				return _dailyListVisibility;
		//			}
		//			set
		//			{
		//				if (_dailyListVisibility != value)
		//				{
		//					_dailyListVisibility = value;
		//					OnPropertyChanged();
		//				}
		//			}
		//		}

		public Boolean IsEmptyMonthlyResult
		{
			get
			{
				return _isEmptyMonthlyResult;
			}
			set
			{
				if (_isEmptyMonthlyResult != value)
				{
					_isEmptyMonthlyResult = value;
					OnPropertyChanged();
					//MonthlyListVisibility = _isEmptyMonthlyResult ? Visibility.Collapsed : Visibility.Visible;
				}
			}
		}

		public Boolean IsEmptyDailyResult
		{
			get
			{
				return _isEmptyDailyResult;
			}
			set
			{
				if (_isEmptyDailyResult != value)
				{
					_isEmptyDailyResult = value;
					OnPropertyChanged();
					//DailyListVisibility = _isEmptyDailyResult ? Visibility.Collapsed : Visibility.Visible;
				}
			}
		}

		public DateTime SelectedMonth
		{
			get { return _selectedMonth; }
			set
			{
				if (_selectedMonth != value)
				{
					_selectedMonth = value;
					OnPropertyChanged();
				}
			}
		}

		public DateTime SelectedDay
		{
			get { return _selectedDay; }
			set
			{
				if (_selectedDay != value)
				{
					_selectedDay = value;
					OnPropertyChanged();
				}
			}
		}

		public Boolean IsProgressOn
		{
			get
			{
				return _isProgressOn;
			}
			set
			{
				if (_callsCount > 0)
					return;

				if (_isProgressOn != value)
				{
					_isProgressOn = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion

		public DailyMonthlyViewModel(UIViewController controller) //Activity activity)
		{
			this.owner = controller;
			_selectedMonth = DateTime.Now;
			_selectedDay = DateTime.Now;

			App.Dispatcher = new DispatchAdapter (owner);

			DailyItems = new SortedObservableCollection<DailyReports>();
			MonthlyItems = new SortedObservableCollection<DailyReports>();

			UserContext user = UserContext.GetUserContext();
			_dManager = DataFactory.GetDataManager(user, DataFactory.DataManagerTypes.Online);

			_dManager.GetMonthlyReportsCallback = reports => 

				//Deployment.Current.Dispatcher.BeginInvoke
				//App.Dispatcher.Invoke(() =>
				controller.InvokeOnMainThread(() =>
				                              {
					_callsCount--;
					GetMonthlyReportsCallback(reports);
				});

			_dManager.GetDailyReportsCallback = reports => 

				//Deployment.Current.Dispatcher.BeginInvoke
				//App.Dispatcher.Invoke(() =>
				controller.InvokeOnMainThread(() =>
				                              {
					_callsCount--;
					GetDailyReportsCallback(reports);
				});

		}

		public void LoadData(DateTime date, Boolean doReguest = true)
		{
			#warning NO LOGGER

			IsProgressOn = true;

			_dManager.BeginGetDailyReports(SelectedDay, doReguest);
			_callsCount++;
			_dManager.BeginGetMonthlyReports(InternalsMethods.CalculateReportPeriod(SelectedMonth), doReguest);
			_callsCount++;

			#warning NO LOGGER
		}

		public void AddReportToMonthly(Report newReport)
		{
			var dailyReports = MonthlyItems.FirstOrDefault(r => r.Date.Day == newReport.Date.Day && r.Date.Month == newReport.Date.Month &&r.Date.Year == newReport.Date.Year);
			if (dailyReports != null)
			{
				dailyReports.Add(newReport);
				InternalsMethods.CalculateTotalHours(dailyReports);
			}
			else
			{
				if (newReport.Date.Month != SelectedMonth.Month)
				{
					var newDailyReport = new DailyReports(newReport.Date.Date, newReport.Time) {newReport};
					if (MonthlyItems.Count == 0)
						GetMonthlyReportsCallback(new[] {newDailyReport});
					else
					{
						MonthlyItems.Add(newDailyReport);
						InternalsMethods.CalculateTotalHours(newDailyReport);
					}
				}
			}
		}

		public void AddReportToDaily(Report newReport)
		{
			DailyReports dailyReports = DailyItems.FirstOrDefault(r => r.Date.Day == newReport.Date.Day && r.Date.Month == newReport.Date.Month && r.Date.Year == newReport.Date.Year);
			if (dailyReports != null)
			{
				dailyReports.Add(newReport);
				InternalsMethods.CalculateTotalHours(dailyReports);
			}
			else
			{
				if (newReport.Date.Day != SelectedDay.Day)
				{
					var newDailyReport = new DailyReports(newReport.Date.Date, newReport.Time) {newReport};
					if (DailyItems.Count == 0)
						GetDailyReportsCallback(newDailyReport);
					else
					{
						DailyItems.Add(newDailyReport);
						InternalsMethods.CalculateTotalHours(newDailyReport);
					}
				}
			}
		}

		public void UpdateDailyReport(Report newReport)
		{
			Report firstOrDefault = DailyItems.SelectMany(r => r.Select(rep => rep)).FirstOrDefault(r => r.Date.Day == newReport.Date.Day && r.Date.Month == newReport.Date.Month && r.Date.Year == newReport.Date.Year);
			if (firstOrDefault != null)
			{
				firstOrDefault.Description = newReport.Description;
				firstOrDefault.FullProject = newReport.FullProject;
				firstOrDefault.IsEditable = newReport.IsEditable;
				firstOrDefault.Date = newReport.Date;
				firstOrDefault.Time = newReport.Time;
				firstOrDefault.IssueManager = newReport.IssueManager;
				firstOrDefault.IssueManagerValue = newReport.IssueManagerValue;
				firstOrDefault.IssueNumber = newReport.IssueNumber;
				firstOrDefault.IssueNumberValue = newReport.IssueNumberValue;
				firstOrDefault.StatusOk = newReport.StatusOk;
				firstOrDefault.StatusSt = newReport.StatusSt;
				InternalsMethods.CalculateTotalHours(DailyItems);
			}
			else
			{
				AddReportToDaily(newReport);
			}
		}

		public void UnLoadData()
		{
			IsDataLoaded = false;
		}

		public void GoToPrevMonth()
		{
			GoToPreviousPeriodHandler ();
		}

		public void GoToNextMonth()
		{
			GoToNextPeriodHandler ();
		}

		#region private methods
		private void GetDailyReportsCallback(DailyReports dailyReports)
		{
			if (dailyReports == null)
			{
				#warning NO LOGGER
				IsDataLoaded = true;
				return;
			}

			DailyItems.Clear();
			IsEmptyDailyResult = dailyReports.Count == 0;
			if (!IsEmptyDailyResult)
			{
				DailyItems.Add(dailyReports);
				InternalsMethods.CalculateTotalHours(dailyReports);
			}

			IsDataLoaded = true;
			#if DEBUG
			TimePerformanceMethod.ShowResult("Get Daily Report");
			#endif
		}

		private void GetMonthlyReportsCallback(ICollection<DailyReports> reports)
		{
			if (reports == null)
			{
				IsDataLoaded = true;
				return;
			}

			MonthlyItems.Clear();

			IOrderedEnumerable<DailyReports> resultReports = reports.OrderBy(m => m.Date);
			foreach (var report in resultReports)
			{
				MonthlyItems.Add(report);
			}

			IsEmptyMonthlyResult = MonthlyItems.Count == 0;
			IsDataLoaded = true;

			// report all subscribers that data is loaded
			if (DataLoaded != null)
				DataLoaded ();

			#if DEBUG
			//TimePerformanceMethod.ShowResult("GetMothlyReport");
			#endif
		}

		private void DeleteReportByIdFrom(int id, SortedObservableCollection<DailyReports> items)
		{
			foreach (DailyReports item in items)
			{
				Report report = item.FirstOrDefault(r => r.Id == id);
				if (report != null)
				{
					item.Remove(report);
					InternalsMethods.CalculateTotalHours(item);
					break;
				}
			}
		}

		//		private void DeleteReport(Int32 id)
		//		{
		//			if (!_dManager.DeleteReport(id))
		//				return;
		//
		//			DeleteReportByIdFrom(id, MonthlyItems);
		//
		//			DeleteReportByIdFrom(id, DailyItems);
		//		}

		private void GoToPreviousPeriodHandler()
		{
			//Note: will trigger GoToMonthHandler
			SelectedMonth = _selectedMonth.AddMonths(-1);
			GoToMonthHandler (SelectedMonth);
		}

		private void GoToNextPeriodHandler()
		{
			//Note: will trigger GoToMonthHandler
			SelectedMonth = _selectedMonth.AddMonths(+1);
			GoToMonthHandler (SelectedMonth);
		}

		private void GoToNextDayHandler()
		{
			//Note: will trigger GoToDayHandler
			SelectedDay = _selectedDay.AddDays(1);
		}

		private void GoToPreviousDayHandler()
		{
			//Note: will trigger GoToDayHandler
			SelectedDay = _selectedDay.AddDays(-1);
		}

		private void GoToDayHandler(Object dateTime)
		{
			IsProgressOn = true;
			SelectedDay = (DateTime)dateTime;
			_dManager.BeginGetDailyReports(SelectedDay);
			_callsCount++;
		}

		private void GoToMonthHandler(Object dateTime)
		{
			IsProgressOn = true;
			SelectedMonth = (DateTime)dateTime;
			_dManager.BeginGetMonthlyReports(InternalsMethods.CalculateReportPeriod(SelectedMonth));
			_callsCount++;
		}

		private void GoToEditReportPage(Int32? reportId, Boolean continueCurrentTaskActive)
		{
			//			if (NavigationService != null)
			//				NavigationService.Navigate(new Uri(String.Format("/View/EditReport.xaml?paramSelectedReportId={0}&continueCurrentTaskActive={1}", reportId, continueCurrentTaskActive), UriKind.Relative));
		}

		private void ContinueCurrentTask(Object rep)
		{
			Report report = rep as Report;
			if (report != null)
			{
				GoToEditReportPage(report.Id, true);
			}
		}

		private void EditReportHandler(Object reportId)
		{
			GoToEditReportPage((Int32?)reportId, false);
		}

		private void DeleteReportWithConfirmHandler(Object report)
		{
			Report item = report as Report;
			if (item != null)
			{
				//				MessageBoxResult result = MessageBox.Show("Do you want to delete this report ?", "Confirm", MessageBoxButton.OKCancel);
				//				if (result == MessageBoxResult.OK)
				//				{
				//					DeleteReport(item.Id);
				//				}
			}
		}

		//		private void DeleteReportHandler(Object report)
		//		{
		//			Report item = report as Report;
		//			if (item != null)
		//			{
		//				DeleteReport(item.Id);
		//			}
		//		}

		private void ShowReportDetailsHandler(Object sender)
		{
			//			var listSelector = sender as LongListSelector;
			//			if (listSelector != null && listSelector.SelectedItem != null)
			//			{
			//				var report = listSelector.SelectedItem as Report;
			//				if (report != null)
			//				{
			//					NavigationService.Navigate(
			//						new Uri(String.Format("/View/ReportDetails.xaml?SelectedId={0}", report.Id), UriKind.Relative));
			//				}
			//			}
		}

		private void RefreshPageHandler(Object sender)
		{
			LoadData(DateTime.Now);
		}
		#endregion

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		//[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion Implementation of INotifyPropertyChanged
	}
}

