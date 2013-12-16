using System;
using TeamProMobileApplicationIOS.Internals;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace TeamProMobileApplicationIOS
{
	public class ColorHelper
	{
		private static readonly Dictionary<string,UIColor> _reportColors = 
			new Dictionary<string, UIColor>()
		{
			{"White",
				UIColor.White},
			{"Red",
				UIColor.Red},
			{"Green",
				UIColor.Green},
			{"Yellow",
				UIColor.Yellow},
			{"Lime",
				UIColor.FromRGB(191,255,0)},
			{"Magenta",
				UIColor.Magenta},
			{"#A6CAF0",
				UIColor.FromRGB(166,202,240)},
			{"#4a4a4a",
				UIColor.FromRGB(74,74,74)},
			{"#888888", 
				UIColor.FromRGB(136,136,136)},
			{"#f9565a",
				UIColor.FromRGB(249, 86,90)},
			{"#818181",
				UIColor.FromRGB(129,129,129)},
			{"#0aa3cf",
				UIColor.FromRGB(10,163,207)},
			{"#528ed7", 
				UIColor.FromRGB(82,142,215)},
			{"#a0a0a0",
				UIColor.FromRGB(160,160,160)},
			{"#2478d1",
				UIColor.FromRGB(36,120,209)},
			{"#cfe1ee",
				UIColor.FromRGB(207,225,238)},
			{"#1164b8",
				UIColor.FromRGB(17,100,184)},
			{"#8cda66",
				UIColor.FromRGB(140,218,102)},
			{"#ffffff",
				UIColor.FromRGB(255,255,255)},
			{"#a6b7d2",
				UIColor.FromRGB(166, 183, 210)},
		};

		public static UIColor Background = UIColor.FromRGB (240,250,252);

		public static UIColor GetColor(string name)
		{
			return _reportColors [name];
		} 
	}
}

