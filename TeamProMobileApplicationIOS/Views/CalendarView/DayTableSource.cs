using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Linq;
using TeamProMobileApplicationIOS.Internals;
using TeamProMobileApplicationIOS.Model;
using TeamProMobileApplicationIOS;
using MonoTouch.ObjCRuntime;
using System.Drawing;
namespace TeamProMobileApplicationIOS
{
	public class DayTableSource : UITableViewSource
	{
		protected SortedObservableCollection<DailyReports> tableItems;
		protected string cellIdentifier = "TableCell";

		protected DayTableSource() {}

		public DayTableSource (SortedObservableCollection<DailyReports> items)
		{
			tableItems = items;
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return tableItems.Count;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return tableItems[section].Count;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var item = tableItems [indexPath.Section] [indexPath.Row];
			var _reportScreen = new ViewReportScreen (item, ReportsListScreen.viewController, ReportsViewTypes.ViewReport);
			ReportsListScreen.ReportsNavigationController.PushViewController (_reportScreen, true);
			tableView.DeselectRow (indexPath, true);
		}

		public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
		{
			Console.WriteLine("Row " + indexPath.Row.ToString() + " deselected");        
		}

		public override NSIndexPath WillSelectRow (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (ReportCell)tableView.CellAt(indexPath);
			if (!cell.MenuIsShown) {
				return indexPath;
			}
			return null;

		}
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			// declare vars
			ReportCell cell = tableView.DequeueReusableCell (cellIdentifier) as ReportCell;
			Report item = tableItems[indexPath.Section][indexPath.Row];

			// if there are no cells to reuse, create a new one
			//if (cell == null)
				cell = new ReportCell (cellIdentifier);
			cell.UpdateCell (item);

			return cell;
		}
	}
}

