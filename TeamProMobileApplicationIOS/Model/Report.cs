using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
//using TeamProMobileApplicationIOS.Annotations;
using TeamProMobileApplicationIOS.Internals;
using TeamProMobileApplicationIOS.Annotations;
//using WindowsPhoneNLog;

namespace TeamProMobileApplicationIOS.Model
{
    public class Report : INotifyPropertyChanged
    {
        private readonly CultureInfo _culture = new CultureInfo("en-US");
        private DateTime _date;
        private TimeSpan _time;
        private String _description;
        private String _fullProject;

        public Int32 Id { get; set; }
        public StatusOkMode StatusOk { get; set; }
        public StatusStMode StatusSt { get; set; }
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value.Equals(_date)) 
                    return;

                _date = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan Time
        {
            get { return _time; }
            set
            {
                if (value.Equals(_time))
                    return;
                
                _time = value;
                OnPropertyChanged();
            }
        }
        public IssueNumber IssueNumber { get; set; }
        public IssueManager IssueManager { get; set; }
        public Boolean IsEditable { get; set; }
        public String FullProject
        {
            get { return _fullProject; }
            set
            {
                if (value == _fullProject) 
                    return;
                
                _fullProject = value;
                OnPropertyChanged();
                OnPropertyChanged("Project");
            }
        }

        public String Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                    return;
                
                _description = value;
                OnPropertyChanged();
            }
        }

        public String IssueNumberValue { get; set; }
        public String IssueManagerValue { get; set; }

        public String Project
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(FullProject) && FullProject.Length > 32)
                {
                    return String.Format("...{0}", FullProject.Remove(0, FullProject.Length - 27));
                }
                return FullProject;
            }
        }

        public String StatusOkColor
        {
            get { return StatusOkModeHelper.GetColor(StatusOk); }
        }

        public String StatusOkCode
        {
            get { return StatusOkModeHelper.GetCode(StatusOk); }
        }

        public String StatusOkName
        {
            get { return StatusOkModeHelper.GetName(StatusOk); }
        }

        public String StatusStColor
        {
            get { return StatusStModeHelper.GetColor(StatusSt); }
        }

        public String StatusStName
        {
            get { return StatusStModeHelper.GetName(StatusSt); }
        }
       
        public void SetDateTime(String dateTime)
        {
            try
            {
                Date = Convert.ToDateTime(dateTime, _culture);
            }
            catch (FormatException e)
            {
#warning No Logger
                //Logger.Error("Failed to convert string to date", e);
            }
        }

        public void SetTime(String timeValue)
        {
            try
            {
                Time = new TimeSpan(Convert.ToDateTime(timeValue, _culture).Ticks);
            }
            catch (FormatException e)
            {
#warning No Logger
                //Logger.Error("Failed to convert time form string", e);
            }
        }

        #region Inmplementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion  Inmplementation of INotifyPropertyChanged
    }
}