using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using TeamProMobileApplicationIOS.Model;
using TeamProMobileApplicationIOS.Internals;

namespace TeamProMobileApplicationIOS
{
	public partial class LoginScreen : UIViewController
	{
		public LoginScreen () : base ("LoginScreen", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			btnLogin.TouchUpInside += (sender, e) => {
				String userName = txtUserName.Text;
				String password = txtPassword.Text;
				ValidateCredentials (userName, password);
			}; 
		}

		public override void ViewWillAppear(bool animated){
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden (true, animated);
		}

		public override void ViewWillDisappear(bool animated){
			base.ViewWillDisappear (animated);
			NavigationController.SetNavigationBarHidden (false, animated);
		}

		private async void ValidateCredentials(String login, String password)
		{
			var user = UserContext.GetUserContext();

			_dManager = DataFactory.GetDataManager(user);

			_warning = String.Empty;
			if (!String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(password))
			{
				if (await _dManager.IsValidCredentials(login, password))
				{
					Settings.Instance.Login = login;
					Settings.Instance.Password = password;
					Settings.Instance.IsLogedIn = true;

					if(_reportsListScreen == null){
						//_reportsListScreen = new MainReportsListScreen();
						_reportsListScreen = new ReportsListScreen ();
					}
					NavigationController.PushViewController(_reportsListScreen, true);
				}
				else
				{
					_warning = "Incorrect username or password";
					lblWarning.Text = _warning;
				}
			}
			else
			{
				_warning = "Please enter username and password";
				lblWarning.Text = _warning;

			}

		}

		private string _warning;
		//private MainReportsListScreen _reportsListScreen;
		private ReportsListScreen _reportsListScreen;
		private IDataManager _dManager;
	}
}

