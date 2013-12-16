using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TeamProMobileApplicationIOS
{
	public class MenuTabBar: UIView
	{
		public MenuTabBar (IntPtr handle) : base(handle) {
		}

		public MenuTabBar (UIView mainView)
		{
			BackgroundColor = UIColor.Blue;

			// reports

			reportsRectangle = new RectangleControl (UIColor.Blue, MenuTabs.Reports);
			reportsRectangle.Frame = new RectangleF (0,15, _width, mainView.Bounds.Height/4);

			UIImageView imgReports = new UIImageView (new RectangleF (5,5, 60,60));
			imgReports.Image = UIImage.FromFile ("menu/menu_reports_normal.png");

			UILabel lblReports = new UILabel (new RectangleF (0,65, 70, 20));
			lblReports.Text = "Reports";
			lblReports.TextAlignment = UITextAlignment.Center;
			lblReports.Font = UIFont.FromName ("HelveticaNeue", 11f);
			lblReports.TextColor = UIColor.Black;
			lblReports.BackgroundColor = UIColor.Blue;

			reportsRectangle.Add (imgReports);
			reportsRectangle.Add (lblReports);

			//contacts

			contactsRectangle = new RectangleControl (UIColor.Blue, MenuTabs.Contacts);
			contactsRectangle.Frame = new RectangleF (0, mainView.Bounds.Height/4 - 5, _width, mainView.Bounds.Height/4);

			UIImageView imgContacts = new UIImageView (new RectangleF (5,10, 60,60));
			imgContacts.Image = UIImage.FromFile ("menu/menu_contacts_normal.png");

			UILabel lblContacts = new UILabel (new RectangleF (0,70, 70, 20));
			lblContacts.Text = "Contacts";
			lblContacts.TextAlignment = UITextAlignment.Center;
			lblContacts.Font = UIFont.FromName ("HelveticaNeue", 11f);
			lblContacts.TextColor = UIColor.Black;
			lblContacts.BackgroundColor = UIColor.Blue;

			contactsRectangle.Add (imgContacts);
			contactsRectangle.Add (lblContacts);

			//settings
			settingsRectangle = new RectangleControl (UIColor.Blue, MenuTabs.Settings);
			settingsRectangle.Frame = new RectangleF (0, mainView.Bounds.Height/2 - 20, _width, mainView.Bounds.Height/4);

			UIImageView imgSettings = new UIImageView (new RectangleF (5,10, 60,60));
			imgSettings.Image = UIImage.FromFile ("menu/menu_settings_normal.png");

			UILabel lblSettings = new UILabel (new RectangleF (0,70, 70, 20));
			lblSettings.Text = "Settings";
			lblSettings.TextAlignment = UITextAlignment.Center;
			lblSettings.Font = UIFont.FromName ("HelveticaNeue", 11f);
			lblSettings.TextColor = UIColor.Black;
			lblSettings.BackgroundColor = UIColor.Blue;

			settingsRectangle.Add (imgSettings);
			settingsRectangle.Add (lblSettings);

			//logout
			logoutRectangle = new RectangleControl (UIColor.Blue, MenuTabs.Logout);
			logoutRectangle.Frame = new RectangleF (0, 3*mainView.Bounds.Height/4 - 35, _width, mainView.Bounds.Height/4);

			UIImageView imgLogout = new UIImageView (new RectangleF (5,10, 60,60));
			imgLogout.Image = UIImage.FromFile ("menu/menu_settings_normal.png");

			UILabel lblLogout = new UILabel (new RectangleF (0,70, 70, 20));
			lblLogout.Text = "Logout";
			lblLogout.TextAlignment = UITextAlignment.Center;
			lblLogout.Font = UIFont.FromName ("HelveticaNeue", 11f);
			lblLogout.TextColor = UIColor.Black;
			lblLogout.BackgroundColor = UIColor.Blue;

			logoutRectangle.Add (imgLogout);
			logoutRectangle.Add (lblLogout);


			Add (reportsRectangle);
			Add (contactsRectangle);
			Add (settingsRectangle);
			Add (logoutRectangle);
		}

		public static bool IsMenuOpen = false;
		private float _width = 70;
		private RectangleControl 	reportsRectangle,
		contactsRectangle,
		settingsRectangle,
		logoutRectangle;
	}
}