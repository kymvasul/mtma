using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TeamProMobileApplicationIOS.Internals;
using System.Collections.Generic;

namespace TeamProMobileApplicationIOS
{
	public partial class ReportsListScreen : UIViewController
	{
		public ReportsListScreen () : base ("ReportsListScreen", null)
		{		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View.BackgroundColor = ColorHelper.Background;

			viewController = this;
			ReportsNavigationController = NavigationController;
			App.Dispatcher = new DispatchAdapter (this);
			App.Owner = this;
			DateTime dateNow = DateTime.Today;
			App.InstanceDailyMonthly.DataLoaded += DailyMonthlyDataLoaded;

			if (!App.InstanceDailyMonthly.IsDataLoaded)
			{
				App.InstanceDailyMonthly.SelectedMonth = dateNow;
				App.InstanceDailyMonthly.LoadData (dateNow);
			}

			addSegment ();
			NavigationController.NavigationBar.Add(SegmentControl);
			if (_systemVersion < 7.0) {
				NavigationController.NavigationBar.TintColor = UIColor.White;
			}
			if (_systemVersion >= 7.0) {
				NavigationController.NavigationBar.BarTintColor = UIColor.White;
			}

			_calendarView = new CalendarView ();
			_calendarView.Frame = new RectangleF (0,0, View.Frame.Width, View.Frame.Height);

			if (App.InstanceDailyMonthly.IsDataLoaded) {
				DailyMonthlyDataLoaded ();
			}
			_tableView = new UITableView(View.Bounds);
			_tableView.Frame = new RectangleF(_tableView.Frame.X, _tableView.Frame.Y, _tableView.Frame.Width-20, View.Frame.Height-100);


			_listView = new UIView();
			_listView.Frame = new RectangleF (0,0, View.Frame.Width, View.Frame.Height);

			if (_systemVersion < 7.0) {
				_listView.BackgroundColor = ColorHelper.Background;
			}
			if (_systemVersion >= 7.0) {
				_listView.BackgroundColor = ColorHelper.Background;
			}
			_listView.Add (_tableView);

			_menu = new MenuTabBar(View);
			Add(_menu);
			Add (_listView);
			Add (_calendarView);
			addRightBarButton ();
			addLeftBarButton ();
		}

		public static void ReloadMonthList()
		{
			list = App.InstanceDailyMonthly.MonthlyItems;
		}

		private void addRightBarButton(){
			_customAddButton = new UIBarButtonItem (
				UIImage.FromFile("report/top_bar_new_report.png"),
				UIBarButtonItemStyle.Plain,
				(s, e) => {

				if(_reportScreen == null){
					_reportScreen = new ViewReportScreen(null, ReportsListScreen.viewController, ReportsViewTypes.AddReport);
				}
				NavigationController.PushViewController(_reportScreen, true);
			}
			);

			if (_systemVersion < 7.0) {
				_customAddButton.SetBackgroundImage (new UIImage(),UIControlState.Normal, UIBarMetrics.Default);
				_customAddButton.TintColor = UIColor.White;
			}
			if (_systemVersion >= 7.0) {
				_customAddButton.TintColor = UIColor.Gray;
			}
			NavigationItem.RightBarButtonItem = _customAddButton;
		}

		private void addLeftBarButton(){
			_customAddButton = new UIBarButtonItem (
				UIImage.FromFile("report/top_bar_menu.png"),
				UIBarButtonItemStyle.Plain,
				(s, e) => {
				if(MenuTabBar.IsMenuOpen == false){
					if(_systemVersion >= 7.0){
						_menu.Frame = new RectangleF(0,40, 70, View.Bounds.Height);
					}
					else{
						_menu.Frame = new RectangleF(0,0, 70, View.Bounds.Height);
					}
					_calendarView.Frame = new RectangleF(70, 0, View.Frame.Width, View.Frame.Height);
					_listView.Frame = new RectangleF(70, 0, View.Frame.Width, View.Frame.Height);
					MenuTabBar.IsMenuOpen = true;
				}
				else{
					closeMenu();				
				}
			}
			);

			if (_systemVersion < 7.0) {
				_customAddButton.SetBackgroundImage (new UIImage(),UIControlState.Normal, UIBarMetrics.Default);
				_customAddButton.TintColor = UIColor.White;
			}
			if (_systemVersion >= 7.0) {
				_customAddButton.TintColor = UIColor.Gray;
			}
			NavigationItem.LeftBarButtonItem = _customAddButton;
		}

		public void addSegment(){			

			SegmentControl = new UISegmentedControl (){
				ControlStyle = UISegmentedControlStyle.Bar,
				TintColor = UIColor.White
			};		

				SegmentControl.Frame = new RectangleF (95, 0, 120, 40);
				SegmentControl.Layer.BorderWidth = 5.5f;

				SegmentControl.InsertSegment (_titleDay, 0, false);
				SegmentControl.InsertSegment (_titleMonth, 1, false);
				SegmentControl.SelectedSegment = 0;

				var selectedAttributes = new UITextAttributes {
					Font = UIFont.FromName ("HelveticaNeue", 13), 
				};
				SegmentControl.SetTitleTextAttributes (selectedAttributes, UIControlState.Normal);

				var unselectedAttributes = new UITextAttributes {
					Font = UIFont.FromName ("HelveticaNeue", 13), 
				};

			if (_systemVersion < 7.0) 
			{
				selectedAttributes.TextColor = UIColor.FromRGB (82, 142, 215);
				unselectedAttributes.TextColor = UIColor.FromRGB (160, 160, 160);
				SegmentControl.TintColor = UIColor.White;
				SegmentControl.Layer.BorderColor = UIColor.White.CGColor;
			}

			if (_systemVersion >= 7.0) 
			{ 
				SegmentControl.Layer.BorderWidth = 2.5f;
				selectedAttributes.TextColor = UIColor.Gray;
				unselectedAttributes.TextColor = UIColor.Gray;
				SegmentControl.TintColor = UIColor.LightGray;
				SegmentControl.Layer.BorderColor = UIColor.White.CGColor;
			}

			SegmentControl.SetTitleTextAttributes (unselectedAttributes, UIControlState.Normal);

			SegmentControl.ValueChanged += (object sender, EventArgs e) => {
				var selectedSegm = (sender as UISegmentedControl).SelectedSegment;
				if(selectedSegm == 0){
					showCalendarView();
				}
				else{
					showListView();
				}
			};
		}	

		public static void closeMenu(){
			_menu.Frame = new RectangleF(0,0, 0, 0);
			_calendarView.Frame = new RectangleF(0, 0, viewController.View.Frame.Width, viewController.View.Frame.Height);
			_listView.Frame = new RectangleF(0, 0, viewController.View.Frame.Width, viewController.View.Frame.Height);
			MenuTabBar.IsMenuOpen = false;
		}
		void showCalendarView ()
		{
			_calendarView.Hidden = false;
			_listView.Hidden = true;
		}

		void showListView ()
		{
			_calendarView.Hidden = true;
			_listView.Hidden = false;
		}

		private void DailyMonthlyDataLoaded ()
		{	
			DateTime currentDate = DateTime.Now;
			list = App.InstanceDailyMonthly.MonthlyItems;	
			TableSource source = new TableSource (list);
			_tableView.Source = source;
			_tableView.ReloadData ();
			if (CalendarView.collectionView.Source == null) {
				CalendarView.collectionView.Source = new CollectionSource (currentDate);
			}
			SortedObservableCollection<DailyReports> dayReportsList = new SortedObservableCollection<DailyReports>();
			TimeSpan totalHours = new TimeSpan(0,0,0);
			foreach (DailyReports report in list) {
				if (report.Date.ToShortDateString () == currentDate.ToShortDateString ()) {
					dayReportsList.Add (report);
				}
				totalHours = totalHours + new TimeSpan(report.TotalHours.Hours, report.TotalHours.Minutes, 0);
			} 
			totalHours = totalHours + new TimeSpan(0, 30, 0);
			TimeSpan interval = TimeSpan.FromMinutes(totalHours.TotalHours - Math.Truncate(totalHours.TotalHours));

			CalendarView.lblMonthTotal.Text = null;
			CalendarView.lblMonthTotal.Text = "Month total  " + string.Format("{00:##}:{1:00}", Math.Truncate(totalHours.TotalHours) , interval.Minutes);
			if (dayReportsList.Count > 0) {

				if (_systemVersion < 7.0) 
				{
					CalendarView.dayTableView.Frame = new RectangleF (10, 280, 300, 130);
				}

				if (_systemVersion >= 7.0) 
				{ 
					CalendarView.dayTableView.Frame = new RectangleF (10, 365, 300, 130);
				}

				CalendarView.dayTableView.Source = new DayTableSource (dayReportsList);
				CalendarView.dayTableView.ReloadData ();
			}
		}

		public static UINavigationController ReportsNavigationController;
		public static UIViewController viewController ; 

		public static SortedObservableCollection<DailyReports> list;
		public static UISegmentedControl SegmentControl;
		static MenuTabBar _menu ;

		private UITableView _tableView;
		private float _systemVersion = AppDelegate.versionIOSFloat;
		private string _titleDay = "Day",
		_titleMonth = "Month";
		private static UIView _listView;
		private static  CalendarView _calendarView;
		private UIBarButtonItem _customAddButton;
		private ViewReportScreen _reportScreen;
}
}
