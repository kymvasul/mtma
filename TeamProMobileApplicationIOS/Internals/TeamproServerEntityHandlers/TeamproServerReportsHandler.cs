using System;
using System.Collections.Generic;
using System.IO;
using TeamProMobileApplicationIOS.Model;
using TeamProMobileApplicationIOS.Internals.Reports;

namespace TeamProMobileApplicationIOS.Internals
{
	class TeamproServerReportsHandler : TeamproServerHandlerBase<List<Report>>
	{
		private List<Report> _reports;
		private bool _isReportSectionFlag;
		//
		public TeamproServerReportsHandler(Stream stream)
			: base(stream)
		{
			_reports = new List<Report>();
		}
		//
		protected override void StartElementImpl(string uri, string localName, string qName, Dictionary<String, String> attributes)
		{
			if (qName == "xml")
			{
				string xmlNodeName;
				if (attributes.TryGetValue("name", out xmlNodeName) && xmlNodeName == "PersonalTimeData")
					_isReportSectionFlag = true;
			}
			else if (qName == "row")
			{
				if (_isReportSectionFlag)
				{
					InitReport(attributes);
				}
			}
		}
		/*<row WorkTimePeriodID="160" WorkTimeDate="2013-04-05T00:00:00" WorkTimeStatus="F" WorkTimeName="estimated teampro modile project" 
             * ProjectPath="/$/ELEKS/B1. SWD/Mgmt/Education (SWD)/Education/Learning/Internal/Mobile" WorkTimeStatusCode="F" WorkTimeStatusName="Submitted" 
             * WorkTimeStatusColor="65535" ID="1585366" Valid="Y" WorkTimeTime="1900-01-01T04:00:00" Status="O" ReadOnly="Y" WorkTimeActions="0" 
             * IssueManagerID="113" IssueManagerName="Education" IssueID="90982" IssueNo="1650" IssueSubject="Mobile development"/>*/
		//valid
		/*<row WorkTimeStatusCode="F" WorkTimeStatusName="Submitted" Valid="Y" Status="O" WorkTimeActions="0" />*/
		//invalid project
		/*<row WorkTimeStatusCode="A" WorkTimeStatusName="Active" Valid="P" Status="O" WorkTimeActions="0" />*/
		//invalid time
		/*<row WorkTimeStatusCode="A" WorkTimeStatusName="Active" Valid="T" Status="O" WorkTimeActions="0" />*/
		private void InitReport(Dictionary<string, string> attributes)
		{
			string ID;
			string Valid;
			string WorkTimeStatusCode;
			string WorkTimeDate;
			string WorkTimeTime;
			string ProjectPath;
			string WorkTimeName;
			string IssueManagerID;
			string IssueManagerName;
			string IssueID;
			string IssueNo;
			string IssueSubject;
			string ReadOnly;

			attributes.TryGetValue(ReportAttributes.ID, out ID);
			attributes.TryGetValue(ReportAttributes.Valid, out Valid);
			attributes.TryGetValue(ReportAttributes.WorkTimeStatusName, out WorkTimeStatusCode);
			attributes.TryGetValue(ReportAttributes.WorkTimeDate, out WorkTimeDate);
			attributes.TryGetValue(ReportAttributes.WorkTimeTime, out WorkTimeTime);
			attributes.TryGetValue(ReportAttributes.ProjectPath, out ProjectPath);
			attributes.TryGetValue(ReportAttributes.WorkTimeName, out WorkTimeName);
			attributes.TryGetValue(ReportAttributes.IssueManagerID, out IssueManagerID);
			attributes.TryGetValue(ReportAttributes.IssueManagerName, out IssueManagerName);
			attributes.TryGetValue(ReportAttributes.IssueID, out IssueID);
			attributes.TryGetValue(ReportAttributes.IssueNo, out IssueNo);
			attributes.TryGetValue(ReportAttributes.IssueSubject, out IssueSubject);
			attributes.TryGetValue(ReportAttributes.ReadOnly, out ReadOnly);
			Report report = new Report();
			report.Id = Convert.ToInt32(ID);
			report.StatusOk = StatusOkModeHelper.FromName(Valid);
			report.StatusSt = StatusStModeHelper.FromName(WorkTimeStatusCode);
			report.SetDateTime(WorkTimeDate);
			report.SetTime(WorkTimeTime);
			report.FullProject = ProjectPath;
			report.Description = WorkTimeName;           
			report.IssueNumberValue = IssueNo;
			report.IssueManagerValue = IssueManagerName;
			report.IsEditable = ReadOnly == "Y" ? false : true;

#warning No Logger
			//Logger.DebugFormat("End creating report with id '{0}'", report.Id);
			_reports.Add(report);
		}

		protected override void ValueImpl(string strValue)
		{ }

		protected override void EndElementImpl(string uri, string localName, string qName)
		{
			if (qName == "xml")
				_isReportSectionFlag = false;
		}

		public override List<Report> GetResults()
		{
			return _reports;
		}
	}
}

