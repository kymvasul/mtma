using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;

namespace TeamProMobileApplicationIOS
{
	public class MenuView : UIView
	{
		private UIButton _btnEdit, _btnDublicate, _btnDelete;

		private UIColor _color;

		public MenuView(UIButton btnEdit, UIButton btnDublicate, UIButton btnDelete, UIColor color) 
		{
			_color = color;
			BackgroundColor = _color.ColorWithAlpha (0.65f);;
			_btnEdit = btnEdit;
			_btnEdit.BackgroundColor = UIColor.Clear;
			_btnDublicate = btnDublicate;
			_btnDublicate.BackgroundColor = UIColor.Clear;
			_btnDelete = btnDelete;
			_btnDelete.BackgroundColor = UIColor.Clear;

			this.Add (_btnEdit);
			this.Add (_btnDublicate);
			this.Add (_btnDelete);
		}

		public MenuView (IntPtr handle) : base (handle) {
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			_btnEdit.Frame = new RectangleF (this.Bounds.Width/2 - 110, this.Bounds.Height/2 - 15, 30, 30);
			_btnDublicate.Frame = new RectangleF (this.Bounds.Width/2 - 10, this.Bounds.Height/2 - 15, 30, 30);
			_btnDelete.Frame = new RectangleF (this.Bounds.Width/2 + 90, this.Bounds.Height/2 - 15, 30, 30);
		}
	}
}
