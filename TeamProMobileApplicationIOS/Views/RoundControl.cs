using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace TeamProMobileApplicationIOS
{
	[Register("RoundControl")]
	public class RoundControl : UIView
	{
		public RoundControl() 
		{
			BackgroundColor = UIColor.White;
		}

		public RoundControl (IntPtr handle) : base (handle) {
		}

		public UIColor Color { get; set; }

		public override void Draw (RectangleF rect)
		{

			var path = UIBezierPath.FromOval(rect);
			Color.SetFill ();
			path.Fill ();
		}
	}
}

