using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace TeamProMobileApplicationIOS
{
	public sealed class TabItemModel
	{
		public TabItemModel (String imageTitle, UIImage image)
		{
			ImageTitle = imageTitle;
			Image = image;
		}

		public UIImage Image {
			get {
				return _image;
			}
			set {
				_image = value;
			}

		}

		public String ImageTitle {
			get {
				return _imageTitle;
			}
			set {
				_imageTitle = value;
			}
		}

		public UIViewController TabUIViewController {
			get {
				return _tabUIViewController;
			}
			set {
				_tabUIViewController = value;
			}

		}

		UIImage _image;
		String _imageTitle;
		UIViewController _tabUIViewController;   
	}
}

