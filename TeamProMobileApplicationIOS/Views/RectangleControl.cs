using System;
using MonoTouch.UIKit;
using System.Drawing;


namespace TeamProMobileApplicationIOS
{
	public class RectangleControl : UIView
	{
		public RectangleControl ()
		{
		}

		public RectangleControl (UIColor color)
		{
			Color = color;
		}

		public RectangleControl (UIColor color, MenuTabs tab)
		{
			Color = color;
			this.tab = tab;
		}

		public RectangleControl (IntPtr handle) : base (handle) {		}

		public override void Draw (RectangleF rect)
		{
			var path = UIBezierPath.FromRect(rect);
			Color.SetFill();
			path.Fill ();
		}

		public UIColor Color { get; set; }

		public override void TouchesBegan (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			if( tab != null){
				if (tab == MenuTabs.Reports) {

				} else {
					if (tab == MenuTabs.Contacts) {

					} else {
						if (tab == MenuTabs.Settings) {
							SettingsScreen settings = new SettingsScreen ();
							ReportsListScreen.viewController.NavigationController.PushViewController (settings, false);
						} else {
							App.InstanceDailyMonthly.UnLoadData ();
							var	login = new LoginScreen ();
							ReportsListScreen.viewController.NavigationController.PushViewController (login, false);
						}
					}
				}
			}
		}

		MenuTabs tab;

	}
}