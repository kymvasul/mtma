using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace TeamProMobileApplicationIOS
{
	public class DispatchAdapter
	{
		private readonly UIViewController _owner;

		public DispatchAdapter(UIViewController owner)
		{
			_owner = owner;
		}

		public void Invoke(NSAction action)
		{
			_owner.InvokeOnMainThread (action);
		}
	}
}

