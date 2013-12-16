using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TeamProMobileApplicationIOS.Model;
using System.Threading.Tasks;

namespace TeamProMobileApplicationIOS
{
	public partial class ReportCell : UITableViewCell
	{
		MyAlertDelegate alertDelegate;

		public bool MenuIsShown { get; set; }

		public ParrentMenuView Menu { get { return _menu;}}

		public ReportCell (IntPtr p):base(p)
		{
		}
		public ReportCell (string CellId) : base (UITableViewCellStyle.Default, CellId)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Gray;
			_lblBranch = new UILabel () {
				Font = UIFont.FromName ("HelveticaNeue", 15f),
				TextColor = ColorHelper.GetColor("#4a4a4a"),
				BackgroundColor = UIColor.Clear,
				LineBreakMode = UILineBreakMode.HeadTruncation,
			};
			_lblTime = new UILabel () {
				Font = UIFont.FromName ("HelveticaNeue", 17f),
				TextColor = ColorHelper.GetColor("#888888"),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Center
			};
			_lblDescription = new UILabel () { 
				Font = UIFont.FromName("HelveticaNeue",12f),
				TextColor =  ColorHelper.GetColor("#888888"),
				BackgroundColor = UIColor.Clear,
			};

			_okImage = new RoundControl ();
			_stImage = new RoundControl ();
			_recognizer = new UILongPressGestureRecognizer (longPressHandler);

			var _btnEdit = new UIButton ();
			_btnEdit.SetImage (UIImage.FromFile ("report/long_tap_edit.png"), UIControlState.Normal);
			_btnEdit.TouchDown += edit_Touch;

			var _btnDublicate = new UIButton ();
			_btnDublicate.SetImage (UIImage.FromFile ("report/long_tap_dublicate.png"), UIControlState.Normal);
			_btnDublicate.TouchDown += dublicate_Touch;

			var _btnDelete = new UIButton ();
			_btnDelete.SetImage (UIImage.FromFile ("report/long_tap_delete.png"), UIControlState.Normal);
			_btnDelete.TouchDown += delete_Touch;

			_menu = new ParrentMenuView (_btnEdit, _btnDublicate, _btnDelete, UIColor.Blue);
			_menu.Hidden = true;

			MenuIsShown = false;

			ContentView.Add (_lblBranch);
			ContentView.Add (_lblTime);
			ContentView.Add (_okImage);
			ContentView.Add (_stImage);
			ContentView.Add (_lblDescription);
			ContentView.Add (_menu);
			ContentView.AddGestureRecognizer (_recognizer);
		}

		public void UpdateCell (Report item)
		{
			_report = item;
			_lblBranch.Text = item.FullProject;
			_lblTime.Text = string.Format("{0:##}:{1:00}", item.Time.Hours, item.Time.Minutes);
			_okImage.Color = ColorHelper.GetColor(item.StatusOkColor);
			_stImage.Color = ColorHelper.GetColor(item.StatusStColor);
			_lblDescription.Text = item.Description;
			HideMenu ();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			_lblBranch.Frame = new RectangleF (20, 2, ContentView.Bounds.Width - 79, 25);
			_lblTime.Frame = new RectangleF (ContentView.Bounds.Width - 79, 2, 100, 25);
			_okImage.Frame = new RectangleF (ContentView.Bounds.Width - 42, 30, 10, 10);
			_stImage.Frame = new RectangleF (ContentView.Bounds.Width - 26, 30, 10, 10);
			_lblDescription.Frame = new RectangleF (20,27,ContentView.Bounds.Width - 79, 15);
			_menu.Frame = new RectangleF (0, 5, ContentView.Bounds.Width, 30);
		}

		protected void longPressHandler (UILongPressGestureRecognizer gestureRecognizer)
		{
			if (!MenuIsShown) {
				if (gestureRecognizer.State == UIGestureRecognizerState.Began
				   || gestureRecognizer.State == UIGestureRecognizerState.Changed) {
					_menu.Hidden = false;
					MenuIsShown = true;
					startHiddingMenu ();
				}
			}
		}

		public void HideMenu ()
		{
			_menu.Hidden = true;
			MenuIsShown = false;
//			this.Bounds = new RectangleF (this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height-30);
		}

		public void edit_Touch (object sender, EventArgs e)
		{
			//TODO: add recognizer - menu item
			ViewReportScreen editReport = new ViewReportScreen (_report, ReportsListScreen.viewController, ReportsViewTypes.EditReport);
			ReportsListScreen.SegmentControl.Hidden = true;
			ReportsListScreen.ReportsNavigationController.PushViewController (editReport, false);
		}

		public void dublicate_Touch (object sender, EventArgs e)
		{
			ViewReportScreen dublicateReport = new ViewReportScreen (_report, ReportsListScreen.viewController, ReportsViewTypes.DublicateReport);
			ReportsListScreen.SegmentControl.Hidden = true;

			ReportsListScreen.ReportsNavigationController.PushViewController (dublicateReport, false);
		}

		public void delete_Touch (object sender, EventArgs e)
		{

			alertDelegate = new MyAlertDelegate();
			UIAlertView alert = new UIAlertView("Are you sure you wont to delete this report?", "", alertDelegate, "No", "Yes");
			alert.Show();

  
		}


		private async void startHiddingMenu()
		{
			await Task.Delay (3000);
			HideMenu ();
		}

		private UILabel _lblBranch, _lblTime, _lblDescription ;
		private RoundControl _okImage, _stImage;
		private UILongPressGestureRecognizer _recognizer;
		private Report _report;
		private ParrentMenuView _menu;
	}

	public class MyAlertDelegate : UIAlertViewDelegate {
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
}

