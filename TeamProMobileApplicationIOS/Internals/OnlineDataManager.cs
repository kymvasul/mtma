using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TeamProMobileApplicationIOS.Model;
using System.Net;
using System.Threading.Tasks;
using TeamProMobileApplicationIOS.Internals.TeamproServerEntitiesCache;
using System.Text.RegularExpressions;
using TeamProMobileApplicationIOS.Internals.TeamproServerEntityHandlers;

namespace TeamProMobileApplicationIOS.Internals
{
	class OnlineDataManager : IDataManager
	{
		private UserContext _userContext;
		public OnlineDataManager(UserContext user)
		{
			_userContext = user;
		}

		//private const String UrlUsers = "/en/asd.dll/i?W32=1&n=w32-user-data-get";
		private const String UrlReports = "/en/asd.dll/i?W32=1&n=get-personal-time-monthly-data&ReportPeriod={0}";
		private const String UrlProjects = "/en/asd.dll/i?W32=1&n=personal-project-lookup&ProjectPath={0}";
		private const String RegularProjects = @"ProjectPath=(.*)";
		private const String UrlContacts = "/en/asd.dll/i?W32=1&n=w32-user-data-get";
		private const String UrlContactsByFilter = "/en/asd.dll/i?W32=1&n=w32-user-data-get&UserLogin={0}";
		private const String UrlImage = "/en/asd.dll/i?n=w32-document&Kind=HR&Doc=1&ExtID=1&ID={0}";
		private const String RegularImage = "image00(2|3).(gif|png|jpg)\r\nContent-Transfer-Encoding: base64\r\nContent-Type: image/(gif|png|jpeg)(.*?)\r\n\r\n------=";

		public Action<ICollection<DailyReports>> GetMonthlyReportsCallback { private get; set; }
		public Action<DailyReports> GetDailyReportsCallback { private get; set; }
		public Action<ICollection<Project>> GetProjectsCallback { private get; set; }
		public Action<ICollection<Person>> GetContactsCallback { private get; set; }
		public Action<ICollection<Person>, string, Boolean?> GetContactsByFilterCallback { private get; set; }
		public Action<MemoryStream> GetImageCallback { private get; set; }

		public void BeginGetImage(Int32 id)
		{
			String url = String.Format(UrlImage, id);
			//LoadData(ReadImageCallback, url);
		}

		public void BeginGetContacts()
		{
//			ICollection<Person> contacts = CacheManager.Instance.ContactsCache.GetAllEntities();
//			if (contacts != null)
//			{
//				if (GetContactsCallback != null)
//					GetContactsCallback(contacts);
//			}
//			else
//			{
//				LoadData(ReadContactsCallback, UrlContacts);
//			}
		}

		public async void BeginGetContactsByFilter(String filter, Boolean doRequest = false)
		{
//			if (!doRequest /*&& String.Equals(App.InstanceSearchPersonListViewModel.LastFilter, filter, StringComparison.InvariantCultureIgnoreCase)*/)
//			{
//				var contactManager = new PersoneContactManager();
//				ICollection<Person> contacts = CacheManager.Instance.ContactsCache.GetAllEntities();
//				if (contacts != null)
//				{
//
//					if (!String.IsNullOrWhiteSpace(filter))
//					{
//						contacts = contacts.FilterContacts(filter).ToList();
//					}
//					foreach (Person contact in contacts)
//					{
//						if (!contact.IsImportedToLocalStore)
//							contact.IsImportedToLocalStore = await contactManager.IsPersonContainsInLocalStore(contact);
//					}
//				}
//
//				TeamProMobileIsolatedStorageManager.SaveLastFilter();
//
//				if (GetContactsByFilterCallback != null)
//					GetContactsByFilterCallback(contacts, filter, doRequest);
//			}
//			else
//			{
//				String contactsUrl = String.IsNullOrEmpty(filter) ? UrlContacts : String.Format(UrlContactsByFilter, filter);
//				LoadData(ar => ReadContactsByFilterCallback(ar, filter), contactsUrl);
//			}
		}

		public void BeginGetMonthlyReports(Int32 period, Boolean doRequest = false)
		{
			ICollection<Report> reports = CacheManager.Instance.ReportsCache.GetEntities(period);
			if (reports != null && !doRequest)
			{
				ICollection<DailyReports> dReports = GroupReports(reports);
				if (GetMonthlyReportsCallback != null)
					GetMonthlyReportsCallback(dReports);
			}
			else
			{
				String url = String.Format(UrlReports, period);
				LoadData(ReadMonthlyReportsCallback, url);
			}
		}

		public void BeginGetDailyReports(DateTime dateTime, Boolean doRequest = false)
		{
			Int32 period = InternalsMethods.CalculateReportPeriod(dateTime);
			ICollection<Report> reports = CacheManager.Instance.ReportsCache.GetEntities(period);
			if (reports != null && !doRequest)
			{
				DailyReports dReports = GetDailyReports(reports, dateTime);
				if (GetDailyReportsCallback != null)
					GetDailyReportsCallback(dReports);
			}
			else
			{
				String url = String.Format(UrlReports, period);
				LoadData(ar => ReadDailyReportsCallback(ar, dateTime), url);
			}
		}
		public void BeginGetProjects(String path)
		{
			ICollection<Project> projects = CacheManager.Instance.ProjectsHierarchyCache.GetEntities(path.ToUpper());
			if (projects == null)
				projects = CacheManager.Instance.ProjectsCache.GetEntities(path.ToUpper());
			if (projects != null)
			{
				if (GetProjectsCallback != null)
					GetProjectsCallback(projects);
			}
			else
			{
				String url = String.Format(UrlProjects, path);
				LoadData(ReadProjectsCallback, url);
			}
		}

		public List<IssueManager> GetIssueManager()
		{
			List<IssueManager> result = new List<IssueManager>();

			//Not implemented

			return result;
		}
		public List<IssueNumber> GetIssueNumber(Int32? issueManagerId)
		{
			List<IssueNumber> result = new List<IssueNumber>();

			//Not implemented

			return result;
		}

		public Boolean DeleteReport(Int32 id)
		{
			//Not implemented
			return true;
		}
		public Report CreateReport(Report report)
		{
			//Not implemented
			Random rand = new Random();
			Report newReport = new Report()
			{
				Date = report.Date,
				Time = report.Time,
				FullProject = report.FullProject,
				IssueNumber = report.IssueNumber,
				Description = report.Description,
				IssueManager = report.IssueManager,
				StatusOk = StatusOkMode.Valid,
				StatusSt = StatusStMode.Active,
				IsEditable = true,
				Id = rand.Next(0, 10000)//todo when creating report API will be available, investigate Id population
			};
			ICollection<Report> allEntities = CacheManager.Instance.ReportsCache.GetAllEntities();
			allEntities.Add(newReport);
			CacheManager.Instance.ReportsCache.CacheEntities(allEntities);
			return newReport;
		}
		public Report ContinueCurrentTask(Report report)
		{
			//todo: Not implemented
			return report;
		}
		public Report EditReport(Report report)
		{
			//todo: Not implemented
			return report;
		}

		public List<PersonGroup> GetPersonList()
		{
			List<PersonGroup> result = new List<PersonGroup>();
			//todo: 
			return result;
		}

		public Report SubmitReport(Report report)
		{
			//todo: Not Implemented
			return report;
		}

		public async Task<Boolean> IsValidCredentials(String login, String password)
		{
			try
			{

				String strTargetUri = Settings.Instance.HostName + UrlReports;
				Uri targetUri = new Uri(strTargetUri);
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetUri);		
				request.UseDefaultCredentials = false;

				String domain = "eleks-software";
				if (!String.IsNullOrEmpty(login))
				{
					String[] loginParts = login.Split(@"\"[0]);

					if (loginParts.Length > 1)
					{
						domain = loginParts[0];
						login = loginParts[1];
					}
				}

				request.Credentials = new NetworkCredential(login, password, domain);

				HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

				if (response.StatusCode == HttpStatusCode.OK)
				{
					return true;
				}

				return false;
			}
			catch (Exception exception)
			{
#warning No Logger
				//Logger.Error("An error occured when trying to authenticate user.", exception);
				return false;
			}
		}

//		public async Task<Stream> LoadImageStreamAsync(int id)
//		{
//			//Logger.DebugFormat("Start retrieving image data for person id='{0}'", id);
//			String url = String.Format(UrlImage, id);
//			String strTargetUri = String.Concat(TeamproMobileSettings.Instance.HostName, url);
//			//Logger.DebugFormat("Finish building request url='{0}' for person id='{1}'", strTargetUri, id);
//			Uri targetUri = new Uri(strTargetUri);
//			var request = (HttpWebRequest)WebRequest.Create(targetUri);
//			request.UseDefaultCredentials = false;
//			request.Credentials = new NetworkCredential(_userContext.UserName, _userContext.Password, _userContext.Domain);
//			var response = (HttpWebResponse)await request.GetResponseAsync();
//			//Logger.DebugFormat("End retrieving image data for person id='{0}' with statusCode = '{1}' statusDesctiption = '{2}'", id, response.StatusCode, response.StatusDescription);
//			return GetImageFromRepsonse(response);
//		}

		private void LoadData(AsyncCallback callback, string url)
		{
			try
			{
				String strTargetUri = Settings.Instance.HostName + url;
				Uri targetUri = new Uri(strTargetUri);
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetUri);
				request.UseDefaultCredentials = false;
				request.Credentials = new NetworkCredential(_userContext.UserName, _userContext.Password,
				                                            _userContext.Domain);
				#if DEBUG
				TimePerformanceMethod.StartPerformanceResponce();
				#endif
				request.BeginGetResponse(callback, request); //recursion, ReadWebRequestCallback may call LoadDataAsync again
			}
			catch (Exception ex)
			{
#warning No Logger
				//Logger.Error(String.Format("Error occurred connecting to '{0}'", url), ex);
			}
		}

//		private void ReadImageCallback(IAsyncResult callbackResult)
//		{
//			try
//			{
//				MemoryStream imageStream = null;
//				HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
//				if (myRequest.HaveResponse)
//				{
//					HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);
//					#if DEBUG
//					TimePerformanceMethod.StopPerformanceResponce();
//#warning No Logger
//					//Logger.Debug(String.Format("Time responce for GetImageCallback {0}", TimePerformanceMethod.GetTimePerformanceResponce()));
//					TimePerformanceMethod.StartPerformanceParsing();
//					#endif
//					imageStream = GetImageFromRepsonse(myResponse);
//					#if DEBUG
//					TimePerformanceMethod.StopPerformanceParsing();
//#warning No Logger
//					//Logger.Debug(String.Format("Time Parsing Image {0}", TimePerformanceMethod.GetTimePerformanceParsing()));
//					#endif
//					if (GetImageCallback != null)
//						GetImageCallback(imageStream);
//				}
//
//			}
//			catch (Exception e)
//			{
//#warning No Logger
//				//Logger.Error(String.Format("Unable process response = '{0}'", callbackResult), e);
//			}
//		}

//		private MemoryStream GetImageFromRepsonse(HttpWebResponse myResponse)
//		{
//			MemoryStream imageStream = null;
//
//			try
//			{
//				using (var sr = new StreamReader(myResponse.GetResponseStream()))
//				{
//					string response = sr.ReadToEnd();
//					Regex regex = new Regex(RegularImage, RegexOptions.Singleline);
//					var result = regex.Match(response);
//
//#warning No Logger
//					//Logger.DebugFormat(String.Format("Result comparing response with regular expression {0}", result.Success));
//					if (result.Success)
//					{
//						try
//						{
//							String resultString = result.Groups[result.Groups.Count - 1].Value.Trim();
//							// Logger.DebugFormat(String.Format("Result parsing response {0}", resultString));
//							imageStream = new MemoryStream(Convert.FromBase64String(resultString));
//						}
//						catch (Exception ex)
//						{
//#warning No Logger
//							//Logger.Error("Error when converting byte array in FromBase64String", ex);
//						}
//					}
//					else
//					{
//						var rm = new ResourceManager("TeamProMobile.Resources.AppResources", typeof(AppResources).Assembly);
//						imageStream = new MemoryStream(Convert.FromBase64String(rm.GetString("ImageSource")));
//					}
//				}
//			}
//			catch (Exception exception)
//			{
//#warning No Logger
//				//Logger.Error("An error occured when retrieving image.", exception);
//			}
//			finally
//			{
//				myResponse.Close();
//			}
//
//			return imageStream;
//		}

//		private void ReadContactsCallback(IAsyncResult callbackResult)
//		{
//			try
//			{
//				var myRequest = (HttpWebRequest)callbackResult.AsyncState;
//				if (myRequest.HaveResponse)
//				{
//					var myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);
//					#if DEBUG
//					TimePerformanceMethod.StopPerformanceResponce();
//
//#warning No Logger
//					//Logger.Debug(String.Format("Time responce for GetContactsCallback {0}", TimePerformanceMethod.GetTimePerformanceResponce()));
//					TimePerformanceMethod.StartPerformanceParsing();
//					#endif
//					List<Person> contacts;
//					using (var handler = new TeamproServerContactsHandler(myResponse.GetResponseStream()))
//					{
//						handler.Read();
//						contacts = handler.GetResults();
//					}
//					myResponse.Close();
//					#if DEBUG
//					TimePerformanceMethod.StopPerformanceParsing();
//#warning No Logger
//					//Logger.Debug(String.Format("Time Parsing Contacts {0}", TimePerformanceMethod.GetTimePerformanceParsing()));
//					#endif
//
//					if (contacts.Count > 0)
//					{
//						CacheManager.Instance.ContactsCache.CacheEntities(contacts);
//					}
//
//					if (contacts.Count > 0)
//					{
//						CacheManager.Instance.ContactsCache.CacheEntities(contacts);
//					}
//					if (GetContactsCallback != null)
//						GetContactsCallback(contacts);
//				}
//			}
//			catch (WebException e)
//			{
//				using (WebResponse response = e.Response)
//				{
//					HttpWebResponse httpResponse = (HttpWebResponse)response;
//					using (Stream data = response.GetResponseStream())
//					{
//						using (var reader = new StreamReader(data))
//						{
//							string text = reader.ReadToEnd();
//#warning No Logger
//							//Logger.Error(String.Format("Error code: {0}\nResponse Message: {1}\n", httpResponse.StatusCode, text), e);
//						}
//					}
//				}
//				if (GetContactsCallback != null)
//					GetContactsCallback(null);
//			}
//			catch (Exception e)
//			{
//				if (GetContactsCallback != null)
//					GetContactsCallback(null);
//#warning No Logger
//				//Logger.Error("Unable process response", e);
//			}
//		}
//
//		private void ReadContactsByFilterCallback(IAsyncResult callbackResult, String filter)
//		{
//			try
//			{
//				HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
//				if (myRequest.HaveResponse)
//				{
//					List<Person> contacts;
//
//					using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult))
//					{
//						#if DEBUG
//						TimePerformanceMethod.StopPerformanceResponce();
//#warning No Logger
//						//Logger.Debug(String.Format("Time responce for ReadContactsByFilterCallback {0}", TimePerformanceMethod.GetTimePerformanceResponce()));
//						TimePerformanceMethod.StartPerformanceParsing();
//						#endif
//						using (TeamproServerContactsHandler handler = new TeamproServerContactsHandler(myResponse.GetResponseStream()))
//						{
//							handler.Read();
//							contacts = handler.GetResults();
//						}
//					}
//					#if DEBUG
//					TimePerformanceMethod.StopPerformanceParsing();
//#warning No Logger
//					//Logger.Debug(String.Format("Time Parsing Contacts {0}", TimePerformanceMethod.GetTimePerformanceParsing()));
//					#endif
//
//					CacheManager.Instance.ContactsCache.ClearCacheEntities(person => !person.IsImportedToLocalStore);
//					CacheManager.Instance.ContactsCache.MerageCacheEntities(contacts, person => !person.IsImportedToLocalStore);
//
//					TeamProMobileIsolatedStorageManager.SaveLastFilter();
//
//					if (GetContactsByFilterCallback != null)
//						GetContactsByFilterCallback(contacts, filter, true);
//				}
//			}
//			catch (WebException e)
//			{
//				using (WebResponse response = e.Response)
//				{
//					HttpWebResponse httpResponse = (HttpWebResponse)response;
//					using (Stream data = response.GetResponseStream())
//					{
//						using (var reader = new StreamReader(data))
//						{
//							string text = reader.ReadToEnd();
//#warning No Logger
//							//Logger.Error(String.Format("Error code: {0}\nResponse Message: {1}\n", httpResponse.StatusCode, text), e);
//						}
//					}
//				}
//				if (GetContactsByFilterCallback != null)
//					GetContactsByFilterCallback(null, null, null);
//			}
//			catch (Exception e)
//			{
//				if (GetContactsByFilterCallback != null)
//					GetContactsByFilterCallback(null, null, null);
//
//#warning No Logger
//				//Logger.Error("Unable process response", e);
//			}
//		}

		private void ReadMonthlyReportsCallback(IAsyncResult callbackResult)
		{
			try
			{
				HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
				if (myRequest.HaveResponse)
				{
					HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);
					#if DEBUG
					TimePerformanceMethod.StopPerformanceResponce();
#warning No Logger
					//Logger.Debug(String.Format("Time responce for GetMonthlyReportsCallback {0}", TimePerformanceMethod.GetTimePerformanceResponce()));
					TimePerformanceMethod.StartPerformanceParsing();
					#endif
					List<Report> reports;
					using (TeamproServerReportsHandler handler = new TeamproServerReportsHandler(myResponse.GetResponseStream()))
					{
						handler.Read();
						reports = handler.GetResults();
					}
					myResponse.Close();
					#if DEBUG
					TimePerformanceMethod.StopPerformanceParsing();
#warning No Logger
					//Logger.Debug(String.Format("Time Parsing MonthlyReports {0}", TimePerformanceMethod.GetTimePerformanceParsing()));
					#endif
					CacheManager.Instance.ReportsCache.CacheEntities(reports);
					ICollection<DailyReports> dReports = GroupReports(reports);
					if (GetMonthlyReportsCallback != null)
						GetMonthlyReportsCallback(dReports);
				}
			}
			catch (WebException e)
			{
				using (WebResponse response = e.Response)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)response;
					using (Stream data = response.GetResponseStream())
					{
						using (var reader = new StreamReader(data))
						{
							string text = reader.ReadToEnd();
#warning No Logger
							//Logger.Error(String.Format("Error code: {0}\nResponse Message: {1}\n", httpResponse.StatusCode, text), e);
						}
					}
				}
				if (GetMonthlyReportsCallback != null)
					GetMonthlyReportsCallback(null);
			}
			catch (Exception e)
			{
				if (GetMonthlyReportsCallback != null)
					GetMonthlyReportsCallback(null);

#warning No Logger
				//Logger.Error("Unable process response", e);
			}
			//makes an attempt to repeat request to the server
			/*else
            {
                //ICredentials credentials= myRequest.Credentials.GetCredential();
                LoadDataAsync(new UserContext(credentials.));//recursion
            }*/
		}
		private void ReadDailyReportsCallback(IAsyncResult callbackResult, DateTime dateTime)
		{
			try
			{
				var myRequest = (HttpWebRequest)callbackResult.AsyncState;
				if (myRequest.HaveResponse)
				{
					var myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);
					#if DEBUG
					TimePerformanceMethod.StopPerformanceResponce();
#warning No Logger
					//Logger.Debug(String.Format("Time responce for GetDailyReportsCallback {0}", TimePerformanceMethod.GetTimePerformanceResponce()));
					TimePerformanceMethod.StartPerformanceParsing();
					#endif
					List<Report> reports;
					using (var handler = new TeamproServerReportsHandler(myResponse.GetResponseStream()))
					{
						handler.Read();
						reports = handler.GetResults();
					}
					myResponse.Close();
					#if DEBUG
					TimePerformanceMethod.StopPerformanceParsing();
#warning No Logger
					//Logger.Debug(String.Format("Time Parsing DailyReports {0}", TimePerformanceMethod.GetTimePerformanceParsing()));
					#endif
					CacheManager.Instance.ReportsCache.CacheEntities(reports);
					DailyReports dReports = GetDailyReports(reports, dateTime);
					if (GetDailyReportsCallback != null)
					{
						GetDailyReportsCallback(dReports);
					}
				}
			}
			catch (WebException e)
			{
				using (WebResponse response = e.Response)
				{
					var httpResponse = (HttpWebResponse)response;
					using (Stream data = response.GetResponseStream())
					{
						using (var reader = new StreamReader(data))
						{
							string text = reader.ReadToEnd();
#warning No Logger
							//Logger.Error(String.Format("Error code: {0}\nResponse Message: {1}\n", httpResponse.StatusCode, text), e);
						}
					}
				}
				if (GetDailyReportsCallback != null)
					GetDailyReportsCallback(null);
			}
			catch (Exception e)
			{
				if (GetDailyReportsCallback != null)
					GetDailyReportsCallback(null);

#warning No Logger
				//Logger.Error("Unable process response", e);
			}
			//makes an attempt to repeat request to the server
			/*else
            {
                //ICredentials credentials= myRequest.Credentials.GetCredential();
                LoadDataAsync(new UserContext(credentials.));//recursion
            }*/
		}
		private void ReadProjectsCallback(IAsyncResult callbackResult)
		{
			try
			{
				Regex regex = new Regex(RegularProjects);
				HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
				if (myRequest.HaveResponse)
				{
					HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);
					#if DEBUG
					TimePerformanceMethod.StopPerformanceResponce();
#warning No Logger
					//Logger.Debug(String.Format("Time responce for GetProjectsCallback {0}", TimePerformanceMethod.GetTimePerformanceResponce()));
					TimePerformanceMethod.StartPerformanceParsing();
					#endif
					List<Project> projects;
					using (TeamproServerProjectsHandler handler = new TeamproServerProjectsHandler(myResponse.GetResponseStream()))
					{
						handler.Read();
						projects = handler.GetResults();
					}

#warning No Logger
					//Logger.DebugFormat(String.Format("List of project {0}", projects.Count));
					myResponse.Close();
					#if DEBUG
					TimePerformanceMethod.StopPerformanceParsing();
#warning No Logger
					//Logger.Debug(String.Format("Time Parsing Projects {0}", TimePerformanceMethod.GetTimePerformanceParsing()));
					#endif
					CacheManager.Instance.ProjectsCache.CacheEntities(projects);
					CacheManager.Instance.ProjectsHierarchyCache.CacheEntities(projects);

					var result = regex.Match(myRequest.RequestUri.ToString());
#warning No Logger
					//Logger.DebugFormat(String.Format("Query string {0}", myRequest.RequestUri.Query));

					var resultPath = result.Groups[result.Groups.Count - 1].ToString().ToUpper();
#warning No Logger
					//Logger.DebugFormat(String.Format("Result path{0}", resultPath));

					if (GetProjectsCallback != null)
					{
						if ((resultPath.Length - 1) == resultPath.LastIndexOf('/'))
						{
							GetProjectsCallback(CacheManager.Instance.ProjectsHierarchyCache.GetEntities(resultPath));
						}
						else
						{
							GetProjectsCallback(CacheManager.Instance.ProjectsCache.GetEntities(resultPath));
						}
					}
				}
			}
			catch (WebException e)
			{
				using (WebResponse response = e.Response)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)response;
					using (Stream data = response.GetResponseStream())
					{
						using (var reader = new StreamReader(data))
						{
							string text = reader.ReadToEnd();
#warning No Logger
							//Logger.Error(String.Format("Error code: {0}\nResponse Message: {1}\n", httpResponse.StatusCode, text), e);
						}
					}
				}
				if (GetProjectsCallback != null)
					GetProjectsCallback(null);
			}
			catch (Exception e)
			{
				if (GetProjectsCallback != null)
					GetProjectsCallback(null);

#warning No Logger
				//Logger.Error("Unable process response", e);
			}
		}

		private ICollection<DailyReports> GroupReports(IEnumerable<Report> reports)
		{
			var dReportsIndex = new Dictionary<DateTime, DailyReports>();
			foreach (Report report in reports)
			{
				DailyReports dReports;
				dReportsIndex.TryGetValue(report.Date, out dReports);
				if (dReports == null)
				{
					dReports = new DailyReports {Date = report.Date};
					dReportsIndex.Add(report.Date, dReports);
				}
				dReports.Add(report);
			}

			var resultTotalHours = new TimeSpan(0, 0, 0);
			foreach (var items in dReportsIndex.Values)
			{
				foreach (var item in items)
				{
					resultTotalHours += item.Time;
				}
				items.TotalHours = resultTotalHours;
				resultTotalHours = TimeSpan.Zero;
			}
			return dReportsIndex.Values;
		}

		private DailyReports GetDailyReports(IEnumerable<Report> reports, DateTime dateTime)
		{
			var dReports = new DailyReports { Date = dateTime };
			foreach (Report report in reports)
			{
				if (report.Date.Day == dReports.Date.Day &&report.Date.Month == dReports.Date.Month &&report.Date.Year == dReports.Date.Year )
					dReports.Add(report);
			}
			return dReports;
		}
	}
}

