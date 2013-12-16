using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TeamProMobileApplicationIOS.Internals;
using TeamProMobileApplicationIOS.Model;

namespace TeamProMobileApplicationIOS
{
	internal interface IDataManager
	{
		Action<ICollection<DailyReports>> GetMonthlyReportsCallback { set; }
		Action<DailyReports> GetDailyReportsCallback { set; }
		Action<ICollection<Project>> GetProjectsCallback { set; }
		Action<ICollection<Person>> GetContactsCallback { set; }
		Action<ICollection<Person>, string, Boolean?> GetContactsByFilterCallback { set; }
		Action<MemoryStream> GetImageCallback { set; }

		void BeginGetMonthlyReports(Int32 period, Boolean doRequest = false);
		void BeginGetDailyReports(DateTime dateTime, Boolean doRequest = false);
		void BeginGetProjects(String path);
		void BeginGetContacts();
		void BeginGetContactsByFilter(String filter, Boolean doRequest = false);
		void BeginGetImage(Int32 id);

		Task<Boolean> IsValidCredentials(String login, String password);
		//List<IssueManager> GetIssueManager();
		//List<IssueNumber> GetIssueNumber(Int32? issueManagerId);
		//Boolean DeleteReport(Int32 id);
		Report CreateReport(Report report);
		//Report ContinueCurrentTask(Report report);
		//Report EditReport(Report report);
		//List<PersonGroup> GetPersonList();
		//Report SubmitReport(Report report);
		//Task<Stream> LoadImageStreamAsync(int id);
	}

	internal static class DataFactory
	{
		public enum DataManagerTypes
		{
			Online,
			Stub
		}

		public static IDataManager GetDataManager(UserContext user) { return GetDataManager(user, DataManagerTypes.Online); }
		public static IDataManager GetDataManager(UserContext user, DataManagerTypes managerType)
		{
			if (managerType == DataManagerTypes.Online)
				return new OnlineDataManager(user);
			return new StubDataManager();
		}
	}
}
