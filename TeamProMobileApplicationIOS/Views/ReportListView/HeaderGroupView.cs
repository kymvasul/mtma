using System;
using MonoTouch.UIKit;
using System.Drawing;
using TeamProMobileApplicationIOS.Internals;

namespace TeamProMobileApplicationIOS
{
	public class HeaderGroupView : UIView
	{
		UILabel lblDate, lblTime;

		public HeaderGroupView (DailyReports item)
		{
			lblDate = new UILabel () {
				Font = UIFont.FromName (fontName, 17f),
				BackgroundColor = UIColor.Clear,
				TextColor = ColorHelper.GetColor("#2478d1"),
				Text = item.Date.ToString("M"),
			};
			lblTime = new UILabel () {
				Font = UIFont.FromName (fontName, 15f),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Right,
				TextColor = ColorHelper.GetColor("#888888"),
				Text = string.Format("{0:##}:{1:00}", item.TotalHours.Hours, item.TotalHours.Minutes)
			};
			lblDate.Frame = new RectangleF (20, 4, 150, 20);
			lblTime.Frame = new RectangleF (185, 4, 100, 20);
			AddSubview (lblDate);
			AddSubview (lblTime);
		}

		public HeaderGroupView (IntPtr handle) : base(handle) {
		}

		private string fontName = "HelveticaNeue";
	}
}

