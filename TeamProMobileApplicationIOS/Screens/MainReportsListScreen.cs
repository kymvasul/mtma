using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using TeamProMobileApplicationIOS.Internals;
using TeamProMobileApplicationIOS.Model;
using MonoTouch.ObjCRuntime;

namespace TeamProMobileApplicationIOS
{
	public partial class MainReportsListScreen : UIViewController
	{ 
		public MainReportsListScreen () : base (ReportListScreenTitle, null) {	}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewWillAppear(bool animated){
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden (true, animated);
		}

		public override void ViewWillDisappear(bool animated){
			base.ViewWillDisappear (animated);
			NavigationController.SetNavigationBarHidden (false, animated);
		}

		public static UINavigationController getNavigationController(){
			return navController;
		}

		public override void ViewDidLoad ()
		{
			App.Dispatcher = new DispatchAdapter (this);
			base.ViewDidLoad ();

			App.Owner = this;
			dateNow = DateTime.Today;
			navController = NavigationController;
		//	App.InstanceDailyMonthly.DataLoaded += DailyMonthlyDataLoaded;

			if (!App.InstanceDailyMonthly.IsDataLoaded)
			{
				App.InstanceDailyMonthly.SelectedMonth = dateNow;
				App.InstanceDailyMonthly.LoadData (dateNow);
			}
			View.BackgroundColor = UIColor.FromRGB (240,250,252);
			_navigation = new FlyoutNavigationController ();
			_navigation.View.Frame = UIScreen.MainScreen.Bounds;
			View.AddSubview (_navigation.View);

			// Create the menu:
			_navigation.NavigationRoot = new RootElement ("") {
				new Section {
					from page in _models
						select new TabItemView (page){Frame = new RectangleF(0,0, 100, ((View.Frame).Height/_models.Length))}
				}
			};

			// Create an array of UINavigationControllers that correspond to your menu items:
			_navigation.ViewControllers = Array.ConvertAll (_models, title => 
				new UINavigationController (new TaskPageController (_navigation, title))
			);

			_navigation.SelectedIndexChanged = delegate(){ openNextView(_models[_navigation.SelectedIndex]);};

		}
		private  void openNextView(TabItemModel tab){
		if (tab.ImageTitle == "Logout") {
				App.InstanceDailyMonthly.UnLoadData ();
			if (_login == null) {
				_login = new LoginScreen ();
				NavigationController.PushViewController (_login, true); 
			}
		} else {
			if (tab.ImageTitle == "Settings") {
					App.InstanceDailyMonthly.UnLoadData ();
				if (_settings == null) {
					_settings = new SettingsScreen ();
					NavigationController.PushViewController (_settings, true); 
				}
			} else {

				if (tab.ImageTitle == "Reports") {
						if (App.InstanceDailyMonthly.IsDataLoaded)
							App.InstanceDailyMonthly.UnLoadData ();
					if (_mainReportsListScreen == null) {
						_mainReportsListScreen = new MainReportsListScreen ();
						NavigationController.PushViewController (_mainReportsListScreen, true); 
					}
				}
			}
		}
		}

		class TaskPageController : DialogViewController
		{
			public TaskPageController (FlyoutNavigationController navigation, TabItemModel title) : base (null)
			{
				App.InstanceDailyMonthly.DataLoaded += DailyMonthlyDataLoaded;

				if (App.InstanceDailyMonthly.IsDataLoaded) {
					DailyMonthlyDataLoaded ();
				}
				tableView = new UITableView(View.Bounds);
				tableView.Frame = new RectangleF(tableView.Frame.X, tableView.Frame.Y, tableView.Frame.Width-20, View.Frame.Height-100);

				Root = new RootElement(title.ImageTitle)
				{
					new Section(){
						new UIViewElement("", tableView, false)
					}
				};

				UIBarButtonItem barButtonItem = new UIBarButtonItem( UIImage.FromFile("report/top_bar_menu.png"), UIBarButtonItemStyle.Done, delegate {
					navigation.ToggleMenu ();});
				barButtonItem.TintColor = ColorHelper.GetColor ("#888888");
				NavigationItem.LeftBarButtonItem = barButtonItem;
				addRightBarButton();
				
			}

			public override void ViewDidLoad ()
			{
				base.ViewDidLoad ();
			}

			private void addRightBarButton(){
				_customAddButton = new UIBarButtonItem (
					UIImage.FromFile("report/top_bar_new_report.png"),
					UIBarButtonItemStyle.Plain,
					(s, e) => {
					if(_reportScreen == null){
						_reportScreen = new AddNewReportScreen();
					}
					NavigationController.PushViewController(_reportScreen, true);
				}
				);
				_customAddButton.TintColor = ColorHelper.GetColor ("#888888");
				NavigationItem.RightBarButtonItem = _customAddButton;
			}

			private void DailyMonthlyDataLoaded ()
			{	
				list = App.InstanceDailyMonthly.MonthlyItems;	
				TableSource source = new TableSource (list);
				tableView.Source = source;
				tableView.ReloadData ();
			}

			SortedObservableCollection<DailyReports> list;
			UIBarButtonItem _customAddButton;
			AddNewReportScreen _reportScreen;
			UITableView tableView;
		}

		private static string ReportListScreenTitle = "MainReportsListScreen";
		private FlyoutNavigationController _navigation;
		private TabItemModel[] _models = { 
			new TabItemModel("Reports", UIImage.FromFile("menu/menu_reports_normal.png")),
			new TabItemModel("Contacts", UIImage.FromFile("menu/menu_contacts_normal.png")),
			new TabItemModel("Settings", UIImage.FromFile("menu/menu_settings_normal.png")),
			new TabItemModel("Logout", UIImage.FromFile("menu/menu_logout_normal.png")),
		};

		private DateTime dateNow;
		private SettingsScreen _settings;
		private LoginScreen _login;
		private MainReportsListScreen _mainReportsListScreen;
		private static UINavigationController navController; 

	}
}

