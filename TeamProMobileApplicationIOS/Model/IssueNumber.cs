using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProMobileApplicationIOS.Model
{
    public struct IssueNumber
    {
        public Int32 Id { get; set; }

        public String Number { get; set; }

        public String Subject { get; set; }

        public override bool Equals(object obj)
        {
            return Id.Equals(((IssueNumber)obj).Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
