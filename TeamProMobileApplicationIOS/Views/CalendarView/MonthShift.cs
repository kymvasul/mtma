using System;
using System.Collections.Generic;

namespace TeamProMobileApplicationIOS
{
	public class MonthShift
	{
		private static Dictionary<System.DayOfWeek, int> shiftMonthBefore = new Dictionary<System.DayOfWeek, int>(){
				{System.DayOfWeek.Monday, 0},
				{System.DayOfWeek.Tuesday, 1},
				{System.DayOfWeek.Wednesday, 2},
				{System.DayOfWeek.Thursday, 3},
				{System.DayOfWeek.Friday, 4},
				{System.DayOfWeek.Saturday, 5},
				{System.DayOfWeek.Sunday, 6},
			};

		private static Dictionary<System.DayOfWeek, int> shiftMonthAfter = new Dictionary<System.DayOfWeek, int>(){
			{System.DayOfWeek.Sunday, 0},
			{System.DayOfWeek.Saturday, 1},
			{System.DayOfWeek.Friday, 2},
			{System.DayOfWeek.Thursday, 3},
			{System.DayOfWeek.Wednesday, 4},
			{System.DayOfWeek.Tuesday, 5},
			{System.DayOfWeek.Monday, 6},
		};

		public static int GetShiftForMonthBefore(System.DayOfWeek dayOfWeek)
		{
			return shiftMonthBefore[dayOfWeek];
		} 

		public static int GetShiftForMonthAfter(System.DayOfWeek dayOfWeek)
		{
			return shiftMonthAfter[dayOfWeek];
		} 
	}
}

