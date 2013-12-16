using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TeamProMobileApplicationIOS.Internals
{
    public class PersonGroup : SortedObservableCollection<Person>
    {
        public String GroupingKey { set; get; }

        public PersonGroup(String groupingKey, IEnumerable<Person> items)
            : base(items)
        {
            GroupingKey = groupingKey;
        }

        public PersonGroup()
        { }
    }
}