using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
//using System.Windows.Navigation;
//using Microsoft.Expression.Interactivity.Core;
using TeamProMobileApplicationIOS.Internals;
using TeamProMobileApplicationIOS.Internals.TeamproServerEntitiesCache;
using TeamProMobileApplicationIOS.Model;

namespace TeamProMobileApplicationIOS
{
	public class EditReportViewModel : INotifyPropertyChanged
	{
		public EditReportViewModel()
		{
			_dailyMonthlyViewModel = App.InstanceDailyMonthly;
			UserContext user = UserContext.GetUserContext();
			_dManager = DataFactory.GetDataManager(user);
			Date = DateTime.Now;
			//Date = _dailyMonthlyViewModel.SelectedDay;

			PageTitle = "Create report";
			PropertyChanged += HandleChanges;
		}

		#region Public Methods

		public bool LoadData(int id, bool continueCurrentTusk)
		{
			var allReports = CacheManager.Instance.ReportsCache.GetAllEntities();
			Report report = allReports.FirstOrDefault(x => x.Id == id);
			if (report != null)
			{
				if (!continueCurrentTusk)
				{
					PageTitle = "Edit report";
					IsEditMode = true;
				}
				else
				{
					IsEditMode = false;
				}
				Id = report.Id;
				Date = report.Date;
				Description = report.Description;
				FullProject = report.FullProject;
				IsEditable = report.IsEditable;
				IssueManager = report.IssueManager;
				IssueManagerValue = report.IssueManagerValue;
				IssueNumber = report.IssueNumber;
				IssueNumberValue = report.IssueNumberValue;
				StatusOk = report.StatusOk;
				StatusSt = report.StatusSt;
				Time = report.Time;
			}
			else
			{
				//Logger.WarnFormat("Report wasn't found id='{0}'", id);
				IsEditMode = false;
				return false;
			}

			return true;
		}

		public void DeleteReport()
		{
//			MessageBoxResult result = MessageBox.Show("Do you want to delete this report ?", "Confirm", MessageBoxButton.OKCancel);
//			if (result == MessageBoxResult.OK)
//			{
//				_dailyMonthlyViewModel.DeleteReportCommand.Execute(new Report { Id = Id });
//
//				NavigationServiseCallBack();
//			}
		}

		public void GetParameters(IDictionary<string, string> queryString)
		{
			string resultParam;
			queryString.TryGetValue("paramProjectId", out resultParam);
			if (!String.IsNullOrEmpty(resultParam))
			{
				ProjectId = Convert.ToInt32(resultParam);

				queryString.TryGetValue("paramDate", out resultParam);
				if (!String.IsNullOrEmpty(resultParam))
					Date = Convert.ToDateTime(resultParam);

				queryString.TryGetValue("paramTime", out resultParam);
				if (!String.IsNullOrEmpty(resultParam))
					Time = TimeSpan.Parse(resultParam);

				queryString.TryGetValue("paramDescription", out resultParam);
				if (!String.IsNullOrEmpty(resultParam))
					Description = resultParam;

				queryString.TryGetValue("paramIssueNumber", out resultParam);
				if (!String.IsNullOrEmpty(resultParam))
					IssueNumberValue = resultParam;
				//ComboBoxIssueNumber.SelectedIndex = Convert.ToInt32(resultParam);

				queryString.TryGetValue("paramIssueManager", out resultParam);
				if (!String.IsNullOrEmpty(resultParam))
					IssueManagerValue = resultParam;
				//ComboBoxIssueManager.SelectedIndex = Convert.ToInt32(resultParam);

				queryString.TryGetValue("paramProjectPath", out resultParam);
				if (!String.IsNullOrEmpty(resultParam))
				{
					FullProject = resultParam;
				}
				queryString.Clear();
			}
		}

		#endregion Public Methods

		#region Public propertys

		public TimeSpan Time
		{
			get { return _time; }
			set { _time = value; }
		}

		public StatusStMode StatusSt
		{
			get { return _statusSt; }
			set
			{
				if (_statusSt == value)
					return;

				_statusSt = value;
				OnPropertyChanged();
			}
		}

		public StatusOkMode StatusOk
		{
			get { return _statusOk; }
			set
			{
				if (_statusOk == value)
					return;

				_statusOk = value;
				OnPropertyChanged();
			}
		}

		public String IssueNumberValue
		{
			get { return _issueNumberValue; }
			set
			{
				if (_issueNumberValue == value)
					return;

				_issueNumberValue = value;
				OnPropertyChanged();
			}
		}

		public IssueNumber IssueNumber
		{
			get { return _issueNumber; }
			set
			{
				if (_issueNumber.Equals(value))
					return;

				_issueNumber = value;
				OnPropertyChanged();
			}
		}

		public String IssueManagerValue
		{
			get { return _issueManagerValue; }
			set
			{
				if (_issueManagerValue == value)
					return;

				_issueManagerValue = value;
				OnPropertyChanged();
			}
		}

		public IssueManager IssueManager
		{
			get { return _issueManager; }
			set
			{
				if (_issueManager.Equals(value))
					return;

				_issueManager = value;
				OnPropertyChanged();
			}
		}

		public Boolean IsEditable
		{
			get { return _isEditable; }
			set { _isEditable = value; }
		}

		public String FullProject
		{
			get { return _fullProject; }
			set
			{
				if (_fullProject == value)
					return;

				_fullProject = value;
				OnPropertyChanged();
				OnPropertyChanged("Project");
			}
		}

		public String Description
		{
			get { return _description; }
			set
			{
				if (_description == value)
					return;

				_description = value;
				OnPropertyChanged();
			}
		}

		public DateTime Date
		{
			get { return _date; }
			set
			{
				if (_date == value)
					return;

				_date = value;
				OnPropertyChanged();
			}
		}

		public Int32 Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public String Project
		{
			get
			{
				if (!String.IsNullOrWhiteSpace(FullProject) && FullProject.Length > 32)
				{
					return String.Format("...{0}", FullProject.Remove(0, FullProject.Length - 27));
				}
				return FullProject;
			}
		}

		public string PageTitle
		{
			get { return _pageTitle; }
			set
			{
				if (_pageTitle == value)
					return;

				_pageTitle = value;
				OnPropertyChanged();
			}
		}

		//public NavigationService NavigationService { get; set; }

		public Boolean IsEditMode { get; set; }

		public Boolean IsReportPropertiesChanged { get; set; }

		public Int32 ProjectId { get; set; }

		#endregion Public propertys

		#region Commands

		public ICommand SaveReportCommand {
			//get { return new ActionCommand(SaveHandler); }
			get{ return null;}
		}

		public ICommand SelectProjectCommand {
			//get { return new ActionCommand(InputProject); } 
			get{ return null;}
		}

		#endregion Commands

		#region Private Methods

		private void NavigationServiseCallBack()
		{
//			if (NavigationService != null)
//			{
//				if (NavigationService.BackStack.Count() > 2)
//				{
//					NavigationService.RemoveBackEntry();
//				}
//				NavigationService.GoBack();
//			}
		}

		private void SaveHandler()
		{
			SaveReport();
			NavigationServiseCallBack();
		}

		private void SaveReport()
		{
			if (IsEditMode)
			{
				EditReport();
			}
			else
			{
				var r = new Report
				{
					Date = Date,
					Time = Time,
					Id = Id,
					Description = Description,
					FullProject = FullProject,
					IsEditable = IsEditable,
					IssueManager = IssueManager,
					IssueManagerValue = IssueManagerValue,
					IssueNumber = IssueNumber,
					IssueNumberValue = IssueNumberValue,
					StatusOk = StatusOk,
					StatusSt = StatusSt
				};

				CreateReport(r);
			}
			IsReportPropertiesChanged = false;
		}

		private void EditReport()
		{
			var allReports = CacheManager.Instance.ReportsCache.GetAllEntities();
			Report tmpReport = allReports.FirstOrDefault(x => x.Id == Id);

			if (tmpReport != null)
			{
				tmpReport.Id = Id;
				tmpReport.Date = Date;
				tmpReport.Description = Description;
				tmpReport.FullProject = FullProject;
				tmpReport.IsEditable = IsEditable;
				tmpReport.IssueManager = IssueManager;
				tmpReport.IssueManagerValue = IssueManagerValue;
				tmpReport.IssueNumber = IssueNumber;
				tmpReport.IssueNumberValue = IssueNumberValue;
				tmpReport.StatusOk = StatusOk;
				tmpReport.StatusSt = StatusSt;
				tmpReport.Time = Time;

				CacheManager.Instance.ReportsCache.CacheEntities(allReports);
				_dailyMonthlyViewModel.LoadData(_dailyMonthlyViewModel.SelectedMonth, false);
			}
			else
			{
			//Logger.Warn("Report wasn't found");
			}

		}

		private void CreateReport(Report report)
		{
			Report newReport = _dManager.CreateReport(report);
			if (newReport == null)
				return;

			//_dailyMonthlyViewModel.AddReportToMonthly(newReport);
			//_dailyMonthlyViewModel.AddReportToDaily(newReport);
			_dailyMonthlyViewModel.LoadData(_dailyMonthlyViewModel.SelectedMonth, false);
		}

		private void HandleChanges(object sender, PropertyChangedEventArgs e)
		{
			IsReportPropertiesChanged = true;
		}

		private void InputProject(object sender)
		{
//			NavigationService.Navigate(
//				new Uri(
//				String.Format(
//				"/View/SelectProject.xaml?paramDate={0}&paramTime={1}&paramDescription={2}&paramIssueNumber={3}&paramIssueManager={4}&paramSelectedReportId={5}&paramProjectId={6}&paramProjectPath={7}",
//				Date,
//				Time,
//				Uri.EscapeDataString(Description ?? String.Empty),
//				IssueNumberValue,
//				IssueManagerValue,
//				!IsEditMode ? null : Id.ToString(),
//				ProjectId,
//				Uri.EscapeDataString(FullProject ?? String.Empty)),
//				UriKind.Relative));

		}

		#endregion Private Methods

		private int _id;
		private DateTime _date;
		private string _description;
		private string _fullProject;
		private bool _isEditable;
		private IssueManager _issueManager;
		private string _issueManagerValue;
		private IssueNumber _issueNumber;
		private string _issueNumberValue;
		private StatusOkMode _statusOk;
		private StatusStMode _statusSt;
		private TimeSpan _time;
		private DailyMonthlyViewModel _dailyMonthlyViewModel;
		private String _pageTitle;
		private IDataManager _dManager;

		#region Implementation if INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion Implementation if INotifyPropertyChanged
	}
}
