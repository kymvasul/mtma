using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace TeamProMobileApplicationIOS
{
	public static class App
	{
		private static DailyMonthlyViewModel _instanceDailyMonthly;

		private static UIViewController _owner;
		public static UIViewController Owner
		{
			get { return _owner; }
			set { _owner = value; }
		}

		private static DispatchAdapter dispather;
		public static DispatchAdapter Dispatcher
		{
			get { return dispather; }
			set { dispather = value; }
		}

		public static DailyMonthlyViewModel InstanceDailyMonthly
		{
			get
			{
				if (_instanceDailyMonthly == null)
				{
					_instanceDailyMonthly = new DailyMonthlyViewModel(_owner);
				}

				return _instanceDailyMonthly;
			}

			set
			{
				_instanceDailyMonthly = value;
			}
		}
	}
}

