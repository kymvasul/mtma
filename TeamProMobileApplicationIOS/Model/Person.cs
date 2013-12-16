using System;
//using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace TeamProMobileApplicationIOS.Internals
{
	public class Person : IComparable<Person>
    {
        public Int32 Id { get; set; }
        public String GroupingKey { get { return NameA12[0].ToString(); } }
        public String Login { get; set; }
        public String FullName { get; set; }
        public String Department { get; set; }
        public String Level1 { get; set; }
        public String Kind1 { get; set; }
        
//		[XmlIgnore]
//        public BitmapImage Picture { get; set; }
        
		public String EMail { get; set; }
        public String Position { get; set; }
        public String ProjectPath { get; set; }
        public String Superior { get; set; }
        public String Division { get; set; }
        public String PhoneA2 { get; set; }
        public String CompNameA3 { get; set; }
        public String NameA12 { get; set; }
        public String DayA21 { get; set; }
        public String CellphoneA28 { get; set; }
        public String PrivateEMailA31 { get; set; }
        public String SurnameA47 { get; set; }
        public String PatronymicA48 { get; set; }
        public String MonthA57 { get; set; }
        public String SkypeMessengerA75 { get; set; }
        public String OfficeA80 { get; set; }
        public String SexA91 { get; set; }
        public String Supervisor { get; set; }
        public String ApprovalBy { get; set; }
        public String Level2 { get; set; }
        public String Kind2 { get; set; }
        public String LeaveStatus { get; set; }
        public String RoomA1 { get; set; }
        public String HomePhoneA30 { get; set; }
        public String YahooMessengerA32 { get; set; }
        public String IcqA34 { get; set; }
        public String MsnMessengerA33 { get; set; }
        public String CarModelNameA81 { get; set; }
        public String RegistrationNumberA82 { get; set; }

        /// <summary>
        /// Determine that contact was stored in local contact store of device
        /// </summary>
        public Boolean IsImportedToLocalStore { get; set; }

        public int CompareTo(Person other)
        {
            return FullName.CompareTo(other.FullName);
        }
    }
}
