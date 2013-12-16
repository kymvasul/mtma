using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace TeamProMobileApplicationIOS
{
	public class ToolsNavigationMenuView: UIView
	{
		public ToolsNavigationMenuView (IntPtr handle) : base(handle) {
		}

		public ToolsNavigationMenuView ()
		{
			// Button Delete

			btnDeleteReport = new UIButton (new RectangleF(0, 0, 20, 20));
			btnDeleteReport.SetImage (UIImage.FromFile("report/top_bar_delete.png"), UIControlState.Normal);
			Add (btnDeleteReport);

			//Button Edit

			btnEditReport = new UIButton (new RectangleF(20,0, 20,20));
			btnEditReport.SetImage (UIImage.FromFile("report/top_bar_edit.png"), UIControlState.Normal);
			Add (btnEditReport);

			//Button Dublicate

			btnDublicate = new UIButton (new RectangleF(40,0, 20,20));
			btnDublicate.SetImage (UIImage.FromFile("report/top_bar_dublicate.png"), UIControlState.Normal);
			Add (btnDublicate);

			//Button Submit

			btnSubmit = new UIButton (new RectangleF(60, 0, 20, 20));
			btnDeleteReport.SetImage (UIImage.FromFile("report/top_bar_submit.png"), UIControlState.Normal);
			Add (btnSubmit);
		}

		UIButton btnDeleteReport;
		UIButton btnEditReport;
		UIButton btnDublicate;
		UIButton btnSubmit;
	}
}

