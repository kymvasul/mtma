using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TeamProMobileApplicationIOS.Model;
using TeamProMobileApplicationIOS.Internals;

namespace TeamProMobileApplicationIOS
{
	public partial class AddNewReportScreen : UIViewController
	{
		public AddNewReportScreen () : base ("AddNewReportScreen", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			reportFields = new ReportMainFieldsView(DateTime.Now);
			reportFields.Frame = new RectangleF (10, 70, 300, 180);
			reportFields.ctrlStatus.Color = UIColor.White;
			reportFields.ctrlValidity.Color = UIColor.White;

			btnSaveReport = new UIButton (new RectangleF (10, 270, 300, 40));
			btnSaveReport.SetTitle ("Save Report", UIControlState.Normal);
			btnSaveReport.TintColor = UIColor.White;
			btnSaveReport.BackgroundColor = ColorHelper.GetColor ("#0aa3cf");
			btnSaveReport.TouchDown += SaveReport;
				
			View.Add (reportFields);
			View.Add (btnSaveReport);
			View.BackgroundColor = UIColor.FromRGB (240,250,252);
		}

		void SaveReport(object sender, EventArgs e){
			
			Report report = new Report ();
			report.Date = DateTime.Now ;
			//report.Time 
			report.FullProject = reportFields.txtBranch.Text;
			report.Description = reportFields.txtDescription.Text;

		}

		UIButton btnSaveReport;
		ReportMainFieldsView reportFields ;
	}
}

