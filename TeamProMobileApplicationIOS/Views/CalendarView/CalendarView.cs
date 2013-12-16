using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TeamProMobileApplicationIOS
{
	public class CalendarView: UIView
	{

		public static NSString ViewCellId = new NSString ("CollectionItem");

		public CalendarView (IntPtr handle) : base(handle) {
		}
		public CalendarView ()
		{
			BackgroundColor = ColorHelper.Background;
			UICollectionViewFlowLayout flowLayout = new UICollectionViewFlowLayout (){
				ItemSize = new System.Drawing.SizeF (31, 31),
				SectionInset = new UIEdgeInsets (3,20,10,20),
				ScrollDirection = UICollectionViewScrollDirection.Vertical,
				MinimumInteritemSpacing = 5, // minimum spacing between cells
				MinimumLineSpacing = 7 // minimum spacing between rows if ScrollDirection is Vertical or between columns if Horizontal

			};

			lblMonth = new UILabel ();

			if (_systemVersion < 7.0) { 
				lblMonth.Frame = new RectangleF (0, 0, 300, 40);
			}
			if (_systemVersion >= 7.0) {
				lblMonth.Frame = new RectangleF (0, 65, 300, 40);
			}

			lblMonth.TextAlignment = UITextAlignment.Center;
			lblMonth.Font = UIFont.FromName ("HelveticaNeue", 20f);
			lblMonth.TextColor = UIColor.Black;
			lblMonth.BackgroundColor = ColorHelper.Background;
			Add (lblMonth);

			if (_systemVersion < 7.0) { 
				collectionView = new UICollectionView (new RectangleF(15,40, 290, 230), flowLayout);
			}
			if (_systemVersion >= 7.0) {
				collectionView = new UICollectionView (new RectangleF(15,100, 290, 230), flowLayout);
			}

			collectionView.RegisterClassForCell (typeof(CollectionItem), ViewCellId);
			collectionView.BackgroundColor = UIColor.FromRGB (240,250,252);
			collectionView.AddGestureRecognizer (getRightGesture());
			collectionView.AddGestureRecognizer (getLeftGesture());
			Add (collectionView);

			lblMonthTotal = new UILabel ();

			if (_systemVersion < 7.0)  {
				lblMonthTotal.Frame = new RectangleF (15, 265, 270, 15);
			}
			if (_systemVersion >= 7.0) {
				lblMonthTotal.Frame = new RectangleF (15, 335, 270, 15);
			}

			lblMonthTotal.TextAlignment = UITextAlignment.Right;
			lblMonthTotal.Font = UIFont.FromName ("HelveticaNeue", 14f);
			lblMonthTotal.TextColor = UIColor.Black;
			lblMonthTotal.Text ="Month total  ";
			lblMonthTotal.BackgroundColor = ColorHelper.Background;
			Add (lblMonthTotal);

			dayTableView = new UITableView(Bounds);

			Add (dayTableView);
		}

		[Export("SwipePrevious")]
		public virtual void SwipePrevious(UIGestureRecognizer rec)
		{
			swipedToPrevious(CollectionSource.firstDayAtThePreviousMonth);
		}

		[Export("SwipeNext")]
		public virtual void SwipeNext(UIGestureRecognizer rec)
		{
			swipedToNext (CollectionSource.firstDayAtTheNextMonth);
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			if(MenuTabBar.IsMenuOpen == true){
				ReportsListScreen.closeMenu ();
			}
		}

		public static RectangleF getFrame(){
			return calendarFrame;
		}

		private UISwipeGestureRecognizer getRightGesture(){
			var _swiperRight = new UISwipeGestureRecognizer();
			_swiperRight.Direction = UISwipeGestureRecognizerDirection.Right;
			_swiperRight.AddTarget(this, new MonoTouch.ObjCRuntime.Selector("SwipePrevious"));
			return _swiperRight;
		}

		private UISwipeGestureRecognizer getLeftGesture(){
			var _swiperLeft = new UISwipeGestureRecognizer();
			_swiperLeft.Direction = UISwipeGestureRecognizerDirection.Left;
			_swiperLeft.AddTarget(this, new MonoTouch.ObjCRuntime.Selector("SwipeNext"));
			return _swiperLeft;
		}

		public static void swipedToNext (DateTime date)
		{
			App.InstanceDailyMonthly.GoToNextMonth ();
			ReportsListScreen.ReloadMonthList ();
			collectionView.Source = new CollectionSource (date);
			dayTableView.Hidden = true;
		}

		public static void swipedToPrevious (DateTime date)
		{
			App.InstanceDailyMonthly.GoToPrevMonth ();
			ReportsListScreen.ReloadMonthList ();
			collectionView.Source = new CollectionSource(date);
			dayTableView.Hidden = true;
		}

		public static UILabel lblMonth, lblMonthTotal;
		public static UIButton btnNextMonth, btnPreviousMonth;
		public static List<DateTime> month;
		public static UICollectionView collectionView;
		public static UITableView dayTableView;

		private float _systemVersion = AppDelegate.versionIOSFloat;
		private static UIView _view;
		private static RectangleF calendarFrame;
	}
}

