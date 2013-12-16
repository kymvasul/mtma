using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace TeamProMobileApplicationIOS
{
	public sealed class TabItemView : UIView
	{
		public TabItemView (IntPtr handle) : base(handle) {
		}

		public TabItemView (TabItemModel item)
		{
			_imageTitle = new UILabel () {
				Font = UIFont.FromName ("Helvetica Neue", 16f),
				BackgroundColor = UIColor.Clear,
				TextColor = UIColor.White,
				Text = item.ImageTitle,
				TextAlignment = UITextAlignment.Center
			};
			_image = new UIImageView (item.Image);
			_image.Frame = new RectangleF ((FlyoutNavigationController.menuWidth/2 - _imageWidth/2), (200 - (_imageHeigh + 60))/2 - 10, _imageWidth, _imageHeigh);
			_imageTitle.Frame = new RectangleF ((FlyoutNavigationController.menuWidth/2 - _imageTitleWidth/2), (_image.Frame.Height + 5), _imageTitleWidth, _imageTitleHeigt);
			BackgroundColor = ColorHelper.GetColor("#2478d1");
			AddSubview (_imageTitle);
			AddSubview (_image);
		}

		private int _imageWidth = 80;
		private int _imageHeigh = 80;
		private int _imageTitleWidth = 70;
		private int _imageTitleHeigt = 40;
		private UILabel _imageTitle;
		private UIImageView _image;
	}
}