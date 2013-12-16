using System;

namespace TeamProMobileApplicationIOS.Internals
{
	public enum ColorStatusMode
	{
		[Name("White")]
		White,
		[Name("Red")]
		Red,
		[Name("Green")]
		Green,
		[Name("Yellow")]
		Yellow,
		[Name("Lime")]
		Lime,
		[Name("Magenta")]
		Magenta,
		[Name("#A6CAF0")]
		LightSteelBlue,
	}

	public class ColorStatusModeHelper
	{
		public static string GetName(ColorStatusMode status)
		{
			return EnumHelper<ColorStatusMode, NameAttribute>.GetAttribute(status).Name;
		} 
	}
}

