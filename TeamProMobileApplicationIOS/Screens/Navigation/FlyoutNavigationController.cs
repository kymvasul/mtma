using System;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace TeamProMobileApplicationIOS
{
	[Register("FlyoutNavigationController")]
	public class FlyoutNavigationController : UIViewController
	{
		public FlyoutNavigationController(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		public FlyoutNavigationController(UITableViewStyle navigationStyle = UITableViewStyle.Plain)
		{
			Initialize(navigationStyle);
			_navigation.View.BackgroundColor = ColorHelper.GetColor("#2478d1");
		}

		public UIColor TintColor
		{
			get { return _tintColor; }
			set
			{
				if (_tintColor == value)
					return;
			}
		}

		public Action SelectedIndexChanged { get; set; }

		public bool AlwaysShowLandscapeMenu { get; set; }

		public bool ForceMenuOpen { get; set; }

		public bool HideShadow
		{
			get { return _hideShadow; }
			set
			{
				if (value == _hideShadow)
					return;
				_hideShadow = value;
				if (_hideShadow)
				{
					if(mainView != null)
						View.InsertSubviewBelow(_shadowView, mainView);
				}
			
			else
				_shadowView.RemoveFromSuperview();

			}
		}

		public UIColor ShadowViewColor
		{
			get { return _shadowView.BackgroundColor; }
			set { _shadowView.BackgroundColor = value; }
		}

		public UIViewController CurrentViewController { get; private set; }

		UIView mainView
		{
			get
			{
				if (CurrentViewController == null)
					return null;
				return CurrentViewController.View;
			}
		}

		public RootElement NavigationRoot
		{
			get { return _navigation.Root; }
			set { EnsureInvokedOnMainThread(delegate { _navigation.Root = value; }); }
		}

		public UITableView  NavigationTableView
		{
			get { return _navigation.TableView; }
		}

		public UIViewController[] ViewControllers
		{
			get { return viewControllers; }
			set
			{
				EnsureInvokedOnMainThread(delegate
					{
						viewControllers = value;
						NavigationItemSelected(GetIndexPath(SelectedIndex));
					});
			}
		}

		public bool IsOpen
		{
			get { return mainView.Frame.X == menuWidth; }
			set
			{
				if (value)
					HideMenu();
				else
					ShowMenu();
			}
		}

		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				if (_selectedIndex == value)
					return;
				_selectedIndex = value;
				EnsureInvokedOnMainThread(delegate { NavigationItemSelected(value); });
			}
		}

		public bool DisableRotation { get; set; }

		public override bool ShouldAutomaticallyForwardRotationMethods
		{
			get { return true; }
		}

		void Initialize(UITableViewStyle navigationStyle = UITableViewStyle.Plain)
		{
			DisableStatusBarMoving = true;
			_statusImage = new UIImageView();
			_navigation = new DialogViewController(navigationStyle, null);
			_navigation.OnSelection += NavigationItemSelected;
			RectangleF navFrame = _navigation.View.Frame;
			navFrame.Width = menuWidth;
			_navigation.View.Frame = navFrame;
			View.AddSubview(_navigation.View);

			TintColor = UIColor.Black;
			var version = new System.Version(UIDevice.CurrentDevice.SystemVersion);
			isIos7 = version.Major >= 7;
			if(isIos7)
			_navigation.TableView.TableHeaderView = new UIView(new RectangleF(0, 0, 320, 22))
					{
						BackgroundColor = UIColor.Clear
					};
			_navigation.TableView.TableFooterView = new UIView(new RectangleF(0, 0, 100, 100)) {BackgroundColor = UIColor.Clear};
			_navigation.TableView.ScrollsToTop = false;
			_shadowView = new UIView();
			_shadowView.BackgroundColor = UIColor.White;
			_shadowView.Layer.ShadowOffset = new SizeF(-5, -1);
			_shadowView.Layer.ShadowColor = UIColor.Black.CGColor;
			_shadowView.Layer.ShadowOpacity = .75f;
			_closeButton = new UIButton();
			_closeButton.TouchUpInside += delegate { HideMenu(); };
			AlwaysShowLandscapeMenu = true;

			View.AddGestureRecognizer (new OpenMenuGestureRecognizer (DragContentView, shouldReceiveTouch));
		}

		public event UITouchEventArgs ShouldReceiveTouch;

		internal bool shouldReceiveTouch(UIGestureRecognizer gesture, UITouch touch)
		{
			if (ShouldReceiveTouch != null)
				return ShouldReceiveTouch(gesture, touch);
			return true;
		}

		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();
			RectangleF navFrame = View.Bounds;
			navFrame.Width = menuWidth;
			if (_navigation.View.Frame != navFrame)
				_navigation.View.Frame = navFrame;
		}

		public void DragContentView(UIPanGestureRecognizer panGesture)
		{
			if (ShouldStayOpen || mainView == null)
				return;
			RectangleF frame = mainView.Frame;
			float translation = panGesture.TranslationInView(View).X;

			if (panGesture.State == UIGestureRecognizerState.Began)
			{
				_startX = frame.X;
			}
			else if (panGesture.State == UIGestureRecognizerState.Changed)
			{
				frame.X = translation + _startX;
				if (frame.X < 0)
					frame.X = 0;
				else if (frame.X > frame.Width)
					frame.X = menuWidth;
				SetLocation(frame);
			}
			else if (panGesture.State == UIGestureRecognizerState.Ended)
			{
				float velocity = panGesture.VelocityInView(View).X;
				float newX = translation + _startX;
				Console.WriteLine(translation + _startX);
				bool show = (Math.Abs(velocity) > sidebarFlickVelocity)
								? (velocity > 0)
								: _startX < menuWidth ? (newX > (menuWidth/2)) : newX > menuWidth;
				if (show)
					ShowMenu();
				else
					HideMenu();
			}
		}

		public override void ViewWillAppear(bool animated)
		{
			RectangleF navFrame = _navigation.View.Frame;
			navFrame.Width = menuWidth;
			navFrame.Location = PointF.Empty;
			_navigation.View.Frame = navFrame;
			View.BackgroundColor = NavigationTableView.BackgroundColor;
			base.ViewWillAppear(animated);
		}

		protected void NavigationItemSelected(NSIndexPath indexPath)
		{
			int index = GetIndex(indexPath);
			NavigationItemSelected(index);
		}
		 
		protected void NavigationItemSelected(int index)
		{
			_selectedIndex = index;
			if (viewControllers == null || viewControllers.Length <= index || index < 0)
			{
				if (SelectedIndexChanged != null)
					SelectedIndexChanged();
				return;
			}
			if (ViewControllers[index] == null)
			{
				if (SelectedIndexChanged != null)
					SelectedIndexChanged();
				return;
			}
			if(!DisableStatusBarMoving)
				UIApplication.SharedApplication.SetStatusBarHidden(false,UIStatusBarAnimation.Fade);
			bool isOpen = false;

			if (mainView != null)
			{
				mainView.RemoveFromSuperview();
				isOpen = IsOpen;
			}
			CurrentViewController = ViewControllers[SelectedIndex];
			RectangleF frame = View.Bounds;
 			if (isOpen || ShouldStayOpen)
				frame.X = menuWidth;

			setViewSize();
			SetLocation(frame);

			View.AddSubview(mainView);
				AddChildViewController(CurrentViewController); 
			if (!HideShadow)
				View.InsertSubviewBelow(_shadowView, mainView);


			if (!ShouldStayOpen)
				HideMenu();
			if (SelectedIndexChanged != null)
				SelectedIndexChanged();
		}

		public void ShowMenu()
		{
			EnsureInvokedOnMainThread(delegate
				{
					_closeButton.Frame = mainView.Frame;
					_shadowView.Frame = mainView.Frame;
					var statusFrame = _statusImage.Frame;
					statusFrame.X = mainView.Frame.X;
					_statusImage.Frame = statusFrame;
					if (!ShouldStayOpen)
						View.AddSubview(_closeButton);
					UIView.BeginAnimations("slideMenu");
					UIView.SetAnimationCurve(UIViewAnimationCurve.EaseIn);
					setViewSize();
					RectangleF frame = mainView.Frame;
					frame.X = menuWidth;
					SetLocation(frame);
					setViewSize();
					frame = mainView.Frame;
					_shadowView.Frame = frame;
					_closeButton.Frame = frame;


					statusFrame.X = mainView.Frame.X;
					_statusImage.Frame = statusFrame;

					UIView.CommitAnimations();
				});
		}

		void setViewSize()
		{
			RectangleF frame = View.Bounds;
			//frame.Location = PointF.Empty;
			if (ShouldStayOpen)
				frame.Width -= menuWidth;
			if (mainView.Bounds == frame)
				return;
			mainView.Bounds = frame;
		}

		void SetLocation(RectangleF frame)
		{
			mainView.Layer.AnchorPoint = new PointF(.5f, .5f);
			frame.Y = 0;
			if (mainView.Frame.Location == frame.Location)
				return;
			frame.Size = mainView.Frame.Size;
			var center = new PointF(frame.Left + frame.Width/2,
									frame.Top + frame.Height/2);
			mainView.Center = center;
			_shadowView.Center = center;

			if (Math.Abs(frame.X - 0) > float.Epsilon)
			{
				getStatus();
				var statusFrame = _statusImage.Frame;
				statusFrame.X = mainView.Frame.X;
				_statusImage.Frame = statusFrame;
			}
		}
		public bool DisableStatusBarMoving {get;set;}
		void getStatus()
		{
			if (DisableStatusBarMoving || !isIos7 || _statusImage.Superview != null)
				return;
			this.View.AddSubview(_statusImage);
			_statusImage.Image = captureStatusBarImage();
			_statusImage.Frame = UIApplication.SharedApplication.StatusBarFrame;
			UIApplication.SharedApplication.StatusBarHidden = true;

		}
		UIImage captureStatusBarImage()
		{
			var frame = UIApplication.SharedApplication.StatusBarFrame;
			frame.Width *= 2;
			frame.Height *= 2;
			var image = CGImage.ScreenImage;
			image = image.WithImageInRect(frame);
			var newImage = new UIImage(image).Scale(UIApplication.SharedApplication.StatusBarFrame.Size, 2f);
			return newImage;
		}
		void hideStatus()
		{
			if (!isIos7)
				return;
			_statusImage.RemoveFromSuperview();
			UIApplication.SharedApplication.StatusBarHidden = false;
		}

		public void HideMenu()
		{
			EnsureInvokedOnMainThread(delegate
				{
					_navigation.FinishSearch();
					_closeButton.RemoveFromSuperview();
					_shadowView.Frame = mainView.Frame;
					var statusFrame = _statusImage.Frame;
					statusFrame.X = mainView.Frame.X;
					_statusImage.Frame = statusFrame;
					UIView.Animate(.2,	() =>
						{
							UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
							RectangleF frame = View.Bounds; 
							frame.X = 0;
							setViewSize();
							SetLocation(frame);
							_shadowView.Frame = frame;
							statusFrame.X = 0;
							_statusImage.Frame = statusFrame;
						}, hideComplete);
				});
		}

		[Export("animationEnded")]
		void hideComplete()
		{
			hideStatus();
			_shadowView.RemoveFromSuperview();
		}

		public void ResignFirstResponders(UIView view)
		{
			if (view.Subviews == null)
				return;
			foreach (UIView subview in view.Subviews)
			{
				if (subview.IsFirstResponder)
					subview.ResignFirstResponder();
				ResignFirstResponders(subview);
			}
		}

		public void ToggleMenu()
		{
			EnsureInvokedOnMainThread(delegate
				{
					if (!IsOpen && CurrentViewController != null && CurrentViewController.IsViewLoaded)
						ResignFirstResponders(CurrentViewController.View);
					if (IsOpen)
						HideMenu();
					else
						ShowMenu();
				});
		}

		private int GetIndex(NSIndexPath indexPath)
		{
			int section = 0;
			int rowCount = 0;
			while (section < indexPath.Section)
			{
				rowCount += _navigation.Root[section].Count;
				section ++;
			}
			return rowCount + indexPath.Row;
		}

		protected NSIndexPath GetIndexPath(int index)
		{
			if (_navigation.Root == null)
				return NSIndexPath.FromRowSection(0, 0);
			int currentCount = 0;
			int section = 0;
			foreach (Section element in _navigation.Root)
			{
				if (element.Count + currentCount > index)
					break;
				currentCount += element.Count;
				section ++;
			}

			int row = index - currentCount;
			return NSIndexPath.FromRowSection(row, section);
		}

		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			if (DisableRotation)
				return toInterfaceOrientation == InterfaceOrientation;

			bool theReturn = CurrentViewController == null
								? true
								: CurrentViewController.ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);
			return theReturn;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
		{
			if (CurrentViewController != null)
				return CurrentViewController.GetSupportedInterfaceOrientations();
			return UIInterfaceOrientationMask.All;
		}

		public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate(toInterfaceOrientation, duration);
		}

		public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate(fromInterfaceOrientation);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				return;
			switch (InterfaceOrientation)
			{
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					ShowMenu();
					return;
				default:
					HideMenu();
					return;
			}
			setViewSize();
		}

		public override void WillAnimateRotation(UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillAnimateRotation(toInterfaceOrientation, duration);
		}

		protected void EnsureInvokedOnMainThread(Action action)
		{
			if (IsMainThread())
			{
				action();
				return;
			}
			BeginInvokeOnMainThread(() =>
									action()
				);
		}

		private static bool IsMainThread()
		{
			return NSThread.Current.IsMainThread;
		}

		private bool ShouldStayOpen
		{
			get
			{
				if (ForceMenuOpen || (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad &&
				                      AlwaysShowLandscapeMenu &&
				                      (InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft
				 || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight)))
					return true;
				return false;
			}
		}

		public const int menuWidth = 100;
		protected UIViewController[] viewControllers;
		private const float sidebarFlickVelocity = 1000.0f;
		private UIButton _closeButton;
		private bool _firstLaunch = true;
		private DialogViewController _navigation;
		private int _selectedIndex;
		private UIView _shadowView;
		private float _startX;
		private UIColor _tintColor;
		private bool isIos7 = false;

		private UIImageView _statusImage;
		private bool _hideShadow;
	}
}