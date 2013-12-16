using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;

namespace TeamProMobileApplicationIOS
{
	public class ParrentMenuView : UIView
	{
		MenuView menu;
		TriangleControl triangle;
		private UIColor _color;

		public ParrentMenuView(UIButton btnEdit, UIButton btnDublicate, UIButton btnDelete, UIColor color) 
		{
			_color = color;
			BackgroundColor = UIColor.Clear;

			menu = new MenuView (btnEdit, btnDublicate, btnDelete, _color);
			triangle = new TriangleControl (_color);

			this.Add (menu);
			this.Add (triangle);
		}

		public ParrentMenuView (IntPtr handle) : base (handle) {
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			triangle.Frame = new RectangleF (this.Bounds.Width / 2 - 2.5f, 0, 5, 5);
			menu.Frame = new RectangleF (0, 5, this.Bounds.Width, this.Bounds.Height -5);
		}
	}
}