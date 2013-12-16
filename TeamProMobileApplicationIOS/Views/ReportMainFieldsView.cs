using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace TeamProMobileApplicationIOS
{
	public sealed class ReportMainFieldsView: UIView
	{
		public ReportMainFieldsView (IntPtr handle) : base(handle) {
		}
		public ReportMainFieldsView(DateTime date)
		{
			UILabel lblProjectValidity, lblProjectStatus;

			ctrlValidity = new RoundControl ();
			ctrlValidity.Frame = new RectangleF (7, 13, 5, 5);
			ctrlValidity.Color = UIColor.White;

			lblProjectValidity = new UILabel (new RectangleF(15,0,150,30));
			lblProjectValidity.Text = "Project Validity";
			lblProjectValidity.Font = UIFont.FromName (fontName, 15f);
			lblProjectValidity.TextColor = fontColor;
			lblProjectValidity.BackgroundColor = ColorHelper.Background;

			ctrlStatus = new RoundControl ();
			ctrlStatus.Frame = new RectangleF (157, 15, 5, 5);
			ctrlStatus.Color = UIColor.White;

			lblProjectStatus = new UILabel (new RectangleF(165,0,150,30));
			lblProjectStatus.Text = "Project Status";
			lblProjectStatus.Font = UIFont.FromName (fontName, 15f);
			lblProjectStatus.TextColor = fontColor;
			lblProjectStatus.BackgroundColor = ColorHelper.Background;

			// Date field
			string dateStr = String.Format(" {0} {1} , {2}", date.ToString("MMMM") , date.Day , date.Year);
			txtReportDate = new UITextField ();
			txtReportDate.Text = dateStr;
			txtReportDate.Font = UIFont.FromName (fontName, 15f);
			txtReportDate.TextColor = fontColor;
			txtReportDate.BackgroundColor = UIColor.White;
		
			// Time field
			RectangleControl rectangleTime = new RectangleControl (UIColor.White);
			rectangleTime.Frame = new RectangleF (246, 40, 54, 30);

			UILabel lblPoints = new UILabel (new RectangleF(25, 3, 3, 24));
			lblPoints.Text = ":";
			rectangleTime.Add (lblPoints);

			txtTimeHours = new UITextField (new RectangleF(3, 3, 23, 24));
			txtTimeHours.Placeholder = "00";
			txtTimeHours.Font = UIFont.FromName (fontName, 20f);
			txtTimeHours.TextColor = fontColor;
			rectangleTime.Add (txtTimeHours);
			txtTimeMinute = new UITextField (new RectangleF(29, 3, 31, 24));
			txtTimeMinute.Placeholder = "00";
			txtTimeMinute.Font = UIFont.FromName (fontName, 20f);
			txtTimeMinute.TextColor = fontColor;
			rectangleTime.Add (txtTimeMinute);

			txtReportDate.Frame = new RectangleF (0,40,240,30);
			Add (rectangleTime);

			//Branch field
			RectangleControl rectangleBranch = new RectangleControl (UIColor.White);
			rectangleBranch.Frame = new RectangleF (0, 80, 300, 45);

			txtBranch = new UITextView ();
			txtBranch.TextColor = fontColor;
			txtBranch.Font = UIFont.FromName (fontName, 14f);
			txtBranch.Frame = new RectangleF (0, 0, 260, 45);

			RectangleControl line = new RectangleControl (fontColor);
			line.Frame = new RectangleF (263, 2, 1, 39);

			_btnOpenBranch = new UIButton () ;
			_btnOpenBranch.SetImage (UIImage.FromFile("report/project_select_folder.png"), UIControlState.Normal);
			_btnOpenBranch.Frame = new RectangleF (270, 10, 23,23);
			_btnOpenBranch.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			_btnOpenBranch.TouchDown += openBranch_TouchDown;

			rectangleBranch.Add (txtBranch);
			rectangleBranch.Add (_btnOpenBranch);
			rectangleBranch.Add (line);

			// Descripion textview
			txtDescription = new UITextView ();
			txtDescription.Font = UIFont.FromName (fontName, 14f);
			txtDescription.TextColor = fontColor;

			txtDescription.Frame = new RectangleF (0, 140, 300, 70);

			Add (ctrlValidity);
			Add (lblProjectValidity);
			Add (ctrlStatus);
			Add (lblProjectStatus);
			Add (rectangleBranch);
			Add (txtReportDate);
			Add (txtDescription);
		}
		public void makeAllFieldsDisable(){
			txtReportDate.Enabled = false;
			txtBranch.Editable= false;
			txtDescription.Editable = false;
			txtTimeHours.Enabled = false;
			txtTimeMinute.Enabled = false;
			_btnOpenBranch.Enabled = false;
		}

		public void makeAllFieldsEnable(){
			txtReportDate.Enabled = true;
			txtBranch.Editable= true;
			txtDescription.Editable = true;
			txtTimeHours.Enabled = true;
			txtTimeMinute.Enabled = true;
			_btnOpenBranch.Enabled = true;
		}

		public void makeAllFieldsEmpty(){
			txtReportDate.Text = null;
			txtBranch.Text= null;
			txtDescription.Text = null;
			txtTimeHours.Text = null;
			txtTimeMinute.Text = null;
		}

		void openBranch_TouchDown (object sender, EventArgs e)
		{
			new UIAlertView ("open branch", "OK?", null, "OK", null).Show ();
		}

		public RoundControl ctrlValidity;
		public RoundControl ctrlStatus;
		public UITextField txtReportDate;
		public UITextField txtTimeHours;
		public UITextField txtTimeMinute;
		public UITextView txtBranch;
		public UITextView txtDescription;
		private UIButton _btnOpenBranch;
		private String fontName = "Helvetica Neue";
		private UIColor fontColor = ColorHelper.GetColor ("#4a4a4a");
	}
}

