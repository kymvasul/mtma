using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TeamProMobileApplicationIOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		public static float versionIOSFloat = Convert.ToSingle ((UIDevice.CurrentDevice.SystemVersion).Substring (0, 3));

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			var rootNavigationController = new UINavigationController ();
			LoginScreen loginScreen = new LoginScreen ();

			rootNavigationController.PushViewController (loginScreen, false);
			window.RootViewController = rootNavigationController;			

			window.MakeKeyAndVisible ();
			return true;
		}
	}
}

