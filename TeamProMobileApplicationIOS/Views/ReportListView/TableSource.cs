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
	public class TableSource : UITableViewSource
	{
		protected SortedObservableCollection<DailyReports> tableItems;
		protected string cellIdentifier = "TableCell";

		protected TableSource() {}

		public TableSource (SortedObservableCollection<DailyReports> items)
		{
			tableItems = items;
		}

		#region -= data binding/display methods =-

		/// <summary>
		/// Called by the TableView to determine how many sections(groups) there are.
		/// </summary>
		public override int NumberOfSections (UITableView tableView)
		{
			return tableItems.Count;
		}

		/// <summary>
		/// Called by the TableView to determine how many cells to create for that particular section.
		/// </summary>
		public override int RowsInSection (UITableView tableview, int section)
		{
			return tableItems[section].Count;
		}

		/// <summary>
		/// Called by the TableView to retrieve the header text for the particular section(group)
		/// </summary>

		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			HeaderGroupView headerView = new HeaderGroupView (tableItems [section]);
			headerView.BackgroundColor = UIColor.FromRGB (240,250,252);
			return headerView;
		}

		#endregion

		#region -= user interaction methods =-

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
//		public override bool ShouldShowMenu (UITableView tableView, NSIndexPath rowAtindexPath)
//		{
//			return true;
//		}
//
//		public override bool CanPerformAction (UITableView tableView, Selector action, NSIndexPath indexPath, NSObject sender)
//		{
//
//			return false;
//		}
//
//		public override void PerformAction (UITableView tableView, Selector action, NSIndexPath indexPath, NSObject sender)
//		{
//			Console.WriteLine ("code to perform action");
//		}
		#endregion

		/// <summary>
		/// Called by the TableView to get the actual UITableViewCell to render for the particular section and row
		/// </summary>
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			// declare vars
			var cell = new ReportCell (cellIdentifier);
//			ReportCell cell = tableView.DequeueReusableCell (cellIdentifier) as ReportCell;
			Report item = tableItems[indexPath.Section][indexPath.Row];

			// if there are no cells to reuse, create a new one
//			if (cell == null)
//				cell = new ReportCell (cellIdentifier);
			//cell.BackgroundColor = UIColor.FromRGB (240,250,252);
			cell.UpdateCell (item);

			return cell;
		}
	}
}

