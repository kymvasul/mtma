using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProMobileApplicationIOS.Internals
{
    public struct IssueManager
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public override bool Equals(object obj)
        {
            return Id.Equals(((IssueManager)obj).Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
