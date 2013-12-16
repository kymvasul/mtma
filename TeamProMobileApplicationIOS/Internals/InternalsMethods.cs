using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamProMobileApplicationIOS.Model;
using System.Diagnostics;

namespace TeamProMobileApplicationIOS.Internals
{
	public static class InternalsMethods
	{
		public static void CalculateTotalHours(DailyReports dailyReports)
		{
			if (dailyReports != null)
			{
				TimeSpan resultHours = new TimeSpan();
				foreach (var item in dailyReports)
				{
					resultHours += item.Time;
				}
				dailyReports.TotalHours = resultHours;
			}
		}

		public static void CalculateTotalHours(SortedObservableCollection<DailyReports> dailyReportsList)
		{
			foreach (DailyReports items in dailyReportsList)
			{
				CalculateTotalHours(items);
			}
		}

//		[Obsolete]
//		public static void DeleteRecordFromCollections(Int32 id)
//		{
//			foreach (DailyReports items in  InstanceDailyMonthly.MonthlyItems)
//			{
//				Report report = items.FirstOrDefault(r => r.Id == id);
//				if (report != null)
//				{
//					items.Remove(report);
//					CalculateTotalHours(items);
//					break;
//				}
//			}
//
//			foreach (DailyReports items in App.InstanceDailyMonthly.DailyItems)
//			{
//				Report report = items.FirstOrDefault(r => r.Id == id);
//				if (report != null)
//				{
//					items.Remove(report);
//					CalculateTotalHours(items);
//					break;
//				}
//			}
//		}

		public static int GetCurrentReportPeriod()
		{
			DateTime initialDate = new DateTime(1999, 12, 1);
			DateTime currentDate = DateTime.Now;
			int result = ((currentDate.Year - initialDate.Year) * 12) + currentDate.Month - initialDate.Month;
			return result;
		}

		public static int CalculateReportPeriod(DateTime reportDate)
		{
			DateTime initialDate = new DateTime(1999, 12, 1);
			int result = ((reportDate.Year - initialDate.Year) * 12) + reportDate.Month - initialDate.Month;
			return result;
		}

		public static IEnumerable<Person> FilterContacts(this IEnumerable<Person> container, String filter)
		{
			return container.Where(
				x => 
				(x.FullName != null && x.FullName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
				|| (x.EMail != null && x.EMail.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
				|| (x.PrivateEMailA31 != null && x.PrivateEMailA31.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
				|| (x.Login != null && x.Login.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
				).ToList();
		}
	}

	public static class TimePerformanceMethod
	{
		private static Stopwatch _timePerformanceResponce = new Stopwatch();
		private static Stopwatch _timePerformanceParsing = new Stopwatch();

		public static void ShowResult(String str)
		{
//			MessageBox.Show(String.Format("{0}\n Time Performance Responce = {1} seconds \n Time Performance Parsing = {2} seconds",
//			                              str, _timePerformanceResponce.Elapsed.Seconds, _timePerformanceParsing.Elapsed.Seconds));
		}

		public static String GetTimePerformanceResponce()
		{
			return _timePerformanceResponce.Elapsed.Seconds.ToString();
		}

		public static String GetTimePerformanceParsing()
		{
			return _timePerformanceParsing.Elapsed.Seconds.ToString();
		}

		public static void StartPerformanceResponce()
		{
			_timePerformanceResponce.Restart();
		}

		public static void StopPerformanceResponce()
		{
			_timePerformanceResponce.Stop();
		}

		public static void StartPerformanceParsing()
		{
			_timePerformanceParsing.Restart();
		}

		public static void StopPerformanceParsing()
		{
			_timePerformanceParsing.Stop();
		}
	}
}

