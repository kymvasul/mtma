using System;
using System.Collections.Generic;
using System.IO;
using TeamProMobileApplicationIOS.Internals.Projects;
using TeamProMobileApplicationIOS.Model;

namespace TeamProMobileApplicationIOS.Internals.TeamproServerEntityHandlers
{
	class TeamproServerProjectsHandler : TeamproServerHandlerBase<List<Project>>
	{
		private List<Project> _projects;
		private bool _isProjectSectionFlag;
		//
		public TeamproServerProjectsHandler(Stream stream): base(stream)
		{
			_projects = new List<Project>();
		}
		//
		protected override void StartElementImpl(string uri, string localName, string qName, Dictionary<String, String> attributes)
		{
			if(qName == "xml")
			{
				string xmlNodeName;
				if (attributes.TryGetValue("name", out xmlNodeName) && xmlNodeName == "SearchResults")
					_isProjectSectionFlag = true;
			}
			else if(qName == "row")
			{
				if (_isProjectSectionFlag)
				{
					InitReport(attributes);
				}
			}
		}
		/*<row ProjectID="26263" ProjectParent="26258" ProjectName="Other" ProjectPath="/$/Accelerated Analytics/Phoenix/Internal/Other" ProjectManagerLogin="eleks-software\s.aleksandrova" 
         * ProjectStatus="A" ProjectReportable="Y" ProjectCreated="2012-12-19T15:14:00" ProjectPathD="/$/Accelerated Analytics/Phoenix/Internal/Other/" ProjectPay="Y" ProjectHidden="N" 
         * ProjectType="?" ProjectCommType="+" ProjectLevel="5" ProjectLane="Accelerated Analytics/Phoenix/Internal/Other" ProjectWorkTypeRequired="N" ProjectIssueRequired="N" 
         * ProjectDepartment="28" ProjectEntity="414" ProjectTypeName="(unknown)" ProjectCommTypeName="Commercial" />*/
		private void InitReport(Dictionary<string, string> attributes)
		{     
			string projectId;
			string projectParent;
			string projectName;
			string projectPath;
			string projectReportable;
			string projectHidden;
			string projectWorkTypeRequired;
			string projectIssueRequired;
			string projectPathD;

			attributes.TryGetValue(ProjectArrtibutes.Id, out projectId);
			attributes.TryGetValue(ProjectArrtibutes.Parent, out projectParent);
			attributes.TryGetValue(ProjectArrtibutes.Name, out projectName);
			attributes.TryGetValue(ProjectArrtibutes.Path, out projectPath);
			attributes.TryGetValue(ProjectArrtibutes.PathD, out projectPathD);
			attributes.TryGetValue(ProjectArrtibutes.Reportable, out projectReportable);
			attributes.TryGetValue(ProjectArrtibutes.Hidden, out projectHidden);
			attributes.TryGetValue(ProjectArrtibutes.WorkTypeRequired, out projectWorkTypeRequired);
			attributes.TryGetValue(ProjectArrtibutes.IssueRequired, out projectIssueRequired);
			var project = new Project(projectName, projectId, projectPath, projectPathD, projectParent, projectReportable == "Y");
			_projects.Add(project);
		}

		protected override void ValueImpl(string strValue)
		{}

		protected override void EndElementImpl(string uri, string localName, string qName)
		{
			if (qName == "xml")
				_isProjectSectionFlag = false;
		}

		public override List<Project> GetResults()
		{
			return _projects;
		}
	}
}
