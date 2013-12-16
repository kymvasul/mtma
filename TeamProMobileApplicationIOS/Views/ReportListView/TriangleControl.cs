using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.Drawing;

namespace TeamProMobileApplicationIOS
{
	[Register("TriangleControl")]
	public class TriangleControl : UIView
	{
		public TriangleControl(UIColor color) 
		{
			BackgroundColor = UIColor.Clear;
			_color = color.ColorWithAlpha (0.95f);
		}

		public TriangleControl (IntPtr handle) : base (handle) {
		}
		public override void Draw (RectangleF rect)
		{
			var ctx = UIGraphics.GetCurrentContext();

			ctx.BeginPath ();
			ctx.MoveTo   (rect.Left, rect.Bottom);  // left bottom
			ctx.AddLineToPoint   (rect.Width/2, rect.Top);  // mid top
			ctx.AddLineToPoint   (rect.Right, rect.Bottom);  // right bottom
			ctx.ClosePath ();

			_color.SetFill ();
			ctx.FillPath ();
		}

		private UIColor _color;
	}
}
