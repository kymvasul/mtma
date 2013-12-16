using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using TeamProMobileApplicationIOS.Internals;
using TeamProMobileApplicationIOS.Model;

namespace TeamProMobileApplicationIOS
{
	public class CollectionItem : UICollectionViewCell
	{
		[Export ("initWithFrame:")]
		public CollectionItem (System.Drawing.RectangleF frame) : base (frame)
		{
			BackgroundView = new UIView { BackgroundColor = ColorHelper.Background};

			SelectedBackgroundView = new UIView {BackgroundColor = ColorHelper.Background};

			roundSelected = new RoundControl ();
			roundSelected.Color = UIColor.Blue;
			roundSelected.BackgroundColor = ColorHelper.Background;
			roundSelected.Frame = new RectangleF (0, 0,30,30);

			_lblSelectedDay = new UILabel (); 
			_lblSelectedDay.Center = ContentView.Center;
			_lblSelectedDay.Transform = CGAffineTransform.MakeScale (0.1f, 0.1f);

			_lblSelectedDay.TextColor = UIColor.White;
			_lblSelectedDay.BackgroundColor = UIColor.Clear;
			_lblSelectedDay.Font = UIFont.FromName ("HelveticaNeue", 140f);
			_lblSelectedDay.TextAlignment = UITextAlignment.Center;
			_lblSelectedDay.Frame = new RectangleF (0,0, 30, 30);
			roundSelected.Add (_lblSelectedDay);
			SelectedBackgroundView.Add (roundSelected);

			ContentView.BackgroundColor = UIColor.Clear;
			_lblDay = new UILabel (); 
			_lblDay.Center = ContentView.Center;
			_lblDay.Transform = CGAffineTransform.MakeScale (0.1f, 0.1f);

			_lblDay.BackgroundColor = UIColor.Clear;
			_lblDay.Font = UIFont.FromName ("HelveticaNeue", 140f);
			_lblDay.TextAlignment = UITextAlignment.Center;
			_lblDay.TextColor = ColorHelper.GetColor ("#1164b8");
			ContentView.Add(_lblDay);
			_lblDay.Frame = new RectangleF (0,0, 30, 30);

			roundStatus = new RoundControl ();
			roundStatus.Color = UIColor.Clear;
			roundStatus.BackgroundColor = UIColor.Clear;
			roundStatus.Frame = new RectangleF (0, 0,30,30);

			BackgroundView.Add (roundStatus);
			BackgroundView.Add (_lblDay);
		}

		public void UpdateCell (DateTime date, DailyReports dailyReport)
		{
			_lblDay.Text = date.Day.ToString ();
			_lblSelectedDay.Text = date.Day.ToString ();
			//roundSelected.Color = UIColor.Blue;

			if(dailyReport != null ){
				var dailyEnumerator = dailyReport.GetEnumerator ();
				while (dailyEnumerator.MoveNext()) {
					Report report = dailyEnumerator.Current;
					if (report.StatusOkColor.Equals ("Red")) {
						UpdateCellStatusRejected ();
						break;
					} 
				}
			}
		}
		private void UpdateCellStatusRejected ()
		{
			_lblDay.TextColor = UIColor.Red;
			roundSelected.Color = UIColor.Red;
		}

		public void UpdateCellCurrentDate ()
		{
			roundStatus.Layer.BorderWidth = 1f;
			roundStatus.Layer.BorderColor = UIColor.Blue.CGColor;
		}
		public void UpdateCellNotCurrentDate ()
		{
			roundStatus.Layer.BorderWidth = 1f;
			roundStatus.Layer.BorderColor = UIColor.Clear.CGColor;
		}

		public void UpdateCellEnotherMonth ()
		{
			_lblDay.TextColor = ColorHelper.GetColor ("#cfe1ee");
		}
		public void UpdateCellColorAllCells ()
		{
			_lblDay.TextColor = ColorHelper.GetColor ("#1164b8");
		}

		public void UpdateCellColor ()
		{
			_lblDay.TextColor = UIColor.White;
		}

		private UILabel _lblDay, _lblSelectedDay;
		public RoundControl roundSelected, roundStatus;
	}
}

