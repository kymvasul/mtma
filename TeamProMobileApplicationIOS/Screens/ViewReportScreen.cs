using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TeamProMobileApplicationIOS.Model;

namespace TeamProMobileApplicationIOS
{
	public partial class ViewReportScreen : UIViewController
	{
		static ViewReportScreen reportScreen;
		UIViewController parent;
		ReportsViewTypes viewType;
		AlertDelegateDelete alertDelegateDelete;
		AlertDelegateDublicate alertDelegateDublicate;
		AlertDelegateEdit alertDelegateEdit;
		AlertDelegateSubmit alertDelegateSubmit;

		public static ViewReportScreen getController(){
			return reportScreen;
		}

		public ViewReportScreen (Report report, UIViewController parent, ReportsViewTypes viewType) : base ("ViewReportScreen", null)
		{
			this._report = report;
			this.parent = parent;
			this.viewType = viewType;
		}
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't ha'hve a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			ReportsListScreen.SegmentControl.Hidden = true;
			if (SegmentControlReport != null) {
				SegmentControlReport.Hidden = false;
			}
			base.ViewDidLoad ();
			reportScreen = this;

			if (_report != null) {
				_reportFields = new ReportMainFieldsView (_report.Date);
				_reportFields.txtTimeHours.Text = _report.Time.Hours.ToString ();
				_reportFields.txtTimeMinute.Text = _report.Time.Minutes.ToString ();
				_reportFields.txtBranch.Text = _report.FullProject;
				_reportFields.txtDescription.Text = _report.Description;
				_reportFields.ctrlStatus.Color = ColorHelper.GetColor (_report.StatusStColor);
				_reportFields.ctrlValidity.Color = ColorHelper.GetColor (_report.StatusOkColor);
				_reportFields.makeAllFieldsDisable ();
			} else {
				_reportFields = new ReportMainFieldsView (DateTime.Now);
			}

			if (_systemVersion < 7.0) {
				_reportFields.Frame = new RectangleF (10, 10, 300, 180);
			}
			if (_systemVersion >= 7.0) {
				_reportFields.Frame = new RectangleF (10, 70, 300, 180);
			}

			Add (_reportFields);

			titleLabel = new UILabel (new RectangleF (80, 15, 80, 20));
			titleLabel.Font = UIFont.FromName ("HelveticaNeue", 10);
			titleLabel.TextColor = UIColor.DarkGray;

			if (viewType == ReportsViewTypes.AddReport) {
				titleLabel.Text = "Add Report";

				addSaveButton ();
				Add (_btnSaveReport);
			} else {
				if (viewType == ReportsViewTypes.ViewReport) {
					titleLabel.Text = "View Report";

				} else {
					if (viewType == ReportsViewTypes.EditReport) {
						titleLabel.Text = "Edit Report";

					} else {
						titleLabel.Text = "Dublicate Report";

						string dateStr = String.Format(" {0} {1} , {2}", (DateTime.Now).ToString("MMMM") , (DateTime.Now).Day , (DateTime.Now).Year);
						_reportFields.txtReportDate.Text = dateStr;
					}
				}
				addSegment ();
			}

			NavigationController.NavigationItem.TitleView = titleLabel;
			NavigationController.NavigationBar.Add (titleLabel);

			if (_systemVersion < 7.0) {
				View.BackgroundColor = UIColor.FromRGB (240, 250, 252);
			}
			if (_systemVersion >= 7.0) {
				View.BackgroundColor = UIColor.FromRGB (240, 250, 252);
			}
			addLeftBarButton ();
		}

		private void addLeftBarButton(){
			UIBarButtonItem _customAddButton = new UIBarButtonItem (
				UIImage.FromFile("report/top_bar_back.png"),
				UIBarButtonItemStyle.Plain,
				(s, e) => {
				if(SegmentControlReport != null){
					SegmentControlReport.Hidden = true;
				}
				NavigationController.NavigationItem.TitleView.Hidden = true;
				ReportsListScreen.SegmentControl.Hidden = false;
				NavigationController.PopToViewController(parent, true);
			}
			);

			if (_systemVersion < 7.0) {
				_customAddButton.SetBackgroundImage (new UIImage(),UIControlState.Normal, UIBarMetrics.Default);
				_customAddButton.TintColor = UIColor.White;
			}
			if (_systemVersion >= 7.0) {
				_customAddButton.TintColor = UIColor.LightGray;
			}

			NavigationItem.LeftBarButtonItem = _customAddButton;
		}

		private void addSegment(){

			SegmentControlReport = new UISegmentedControl ();
			SegmentControlReport.ControlStyle = UISegmentedControlStyle.Bar;
			SegmentControlReport.Frame = new RectangleF (150, 0, 170, 40);
			SegmentControlReport.Layer.BorderWidth = 3.5f;
			SegmentControlReport.Layer.BorderColor = UIColor.White.CGColor;
			SegmentControlReport.Layer.BackgroundColor = UIColor.White.CGColor;
			SegmentControlReport.Layer.ShadowColor = UIColor.White.CGColor;
			SegmentControlReport.BackgroundColor = UIColor.White;

			if (_systemVersion < 7.0) {
				SegmentControlReport.TintColor = UIColor.FromRGB (255, 255, 255);
			}
			if (_systemVersion >= 7.0) {
				SegmentControlReport.TintColor = UIColor.LightGray;
			}

			UIImage imgDeleteReport = UIImage.FromFile ("report/top_bar_delete_custom.png");
			UIImage imgEditReport = UIImage.FromFile ("report/top_bar_edit_custom.png");
			UIImage imgDublicateReport = UIImage.FromFile ("report/top_bar_dublicate_custom.png");
			UIImage imgSubmitReport = UIImage.FromFile ("report/top_bar_submit_custom.png");

			SegmentControlReport.InsertSegment(imgDeleteReport, 0, false);
			SegmentControlReport.InsertSegment (imgEditReport, 1, false);
			SegmentControlReport.InsertSegment (imgDublicateReport, 2, false);
			SegmentControlReport.InsertSegment (imgSubmitReport, 3, false);

			SegmentControlReport.ValueChanged += (object sender, EventArgs e) => {
				var selectedSegm = (sender as UISegmentedControl).SelectedSegment;
				if(selectedSegm == 0){
					alertDelegateDelete = new AlertDelegateDelete();
					UIAlertView alert = new UIAlertView("Are you sure you wont to delete this report?", "", alertDelegateDelete, "No", "Yes");
					alert.Show();
				}
				if(selectedSegm == 1){
					alertDelegateEdit = new AlertDelegateEdit();
					UIAlertView alert = new UIAlertView("Are you sure you wont to edit this report?", "", alertDelegateEdit, "No", "Yes");
					alert.Show();
				}
				if(selectedSegm == 2){
					alertDelegateDublicate = new AlertDelegateDublicate();
					UIAlertView alert = new UIAlertView("Are you sure you wont to dublicate this report?", "", alertDelegateDublicate, "No", "Yes");
					alert.Show();
				}
				if(selectedSegm == 3){
					alertDelegateSubmit = new AlertDelegateSubmit();
					UIAlertView alert = new UIAlertView("Are you sure you wont to submit this report?", "", alertDelegateSubmit, "No", "Yes");
					alert.Show();
				}

			};

			NavigationController.NavigationBar.Add(SegmentControlReport);
		}

		public class AlertDelegateDelete : UIAlertViewDelegate {
			public override void Clicked (UIAlertView alertview,
				int buttonIndex)
			{
				if (buttonIndex == 0) // NO
				{
				}
				if (buttonIndex == 1) // YES
				{
				}
			}
		}

		public class AlertDelegateEdit : UIAlertViewDelegate {
			public override void Clicked (UIAlertView alertview,
				int buttonIndex)
			{
				if (buttonIndex == 0) // NO
				{
				}
				if (buttonIndex == 1) // YES
				{
					titleLabel.Text = "Edit Report";
					_reportFields.makeAllFieldsEnable ();
				}
			}
		}

		public class AlertDelegateDublicate : UIAlertViewDelegate {
			public override void Clicked (UIAlertView alertview,
				int buttonIndex)
			{
				if (buttonIndex == 0) // NO
				{
				}
				if (buttonIndex == 1) // YES
				{
					titleLabel.Text = "Dublicate Report";
					_reportFields.makeAllFieldsEnable ();
					_reportFields.makeAllFieldsEmpty ();

				}
			}
		}

		public class AlertDelegateSubmit : UIAlertViewDelegate {
			public override void Clicked (UIAlertView alertview,
				int buttonIndex)
			{
				if (buttonIndex == 0) // NO
				{
				}
				if (buttonIndex == 1) // YES
				{
				}
			}
		}

		private void addSaveButton(){
			_btnSaveReport = new UIButton (new RectangleF (10, 270, 300, 40));
			_btnSaveReport.SetTitle ("Save Report", UIControlState.Normal);
			_btnSaveReport.TintColor = UIColor.White;
			_btnSaveReport.BackgroundColor = ColorHelper.GetColor ("#0aa3cf");
			_btnSaveReport.TouchDown += SaveReport;
		}

		private void SaveReport(object sender, EventArgs e){

			Report report = new Report ();
			report.Date = DateTime.Now ;
			report.Time = new TimeSpan (Int32.Parse(_reportFields.txtTimeHours.Text) , Int32.Parse(_reportFields.txtTimeMinute.Text) , 0);
			report.FullProject = _reportFields.txtBranch.Text;
			report.Description = _reportFields.txtDescription.Text;
		}

		public UISegmentedControl SegmentControlReport;

		private static UILabel titleLabel;
		private UIButton _btnSaveReport;
		private static ReportMainFieldsView _reportFields ;
		private Report _report;
		private float _systemVersion =  AppDelegate.versionIOSFloat;
	}
}

