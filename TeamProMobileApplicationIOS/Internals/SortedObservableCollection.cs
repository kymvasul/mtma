using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamProMobileApplicationIOS
{
	public class SortedObservableCollection<T> : ObservableCollection<T> where T : IComparable<T>
	{
		public SortedObservableCollection() : base()
		{

		}

		public SortedObservableCollection(IEnumerable<T> collection) : base(collection)
		{

		}

		protected override void InsertItem (int index, T item)
		{
			for (int i = 0; i < this.Count; i++) 
			{
				switch (this [i].CompareTo (item)) {
				case 0:
					throw new InvalidOperationException ("Cannot insert duplicate items");

				case 1:
					base.InsertItem (i, item);
					return;

				case -1:
					break;
				}
			}

			base.InsertItem (index, item);
		}
	}
}

