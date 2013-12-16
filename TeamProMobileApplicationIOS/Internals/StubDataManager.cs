using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamProMobileApplicationIOS.Internals;
using TeamProMobileApplicationIOS.Model;
using System.IO;
using System.Threading.Tasks;

using System.Reflection;
using System.Resources;

namespace TeamProMobileApplicationIOS.Internals
{
	class StubDataManager : IDataManager
	{
		public Action<ICollection<DailyReports>> GetMonthlyReportsCallback { private get; set; }
		public Action<DailyReports> GetDailyReportsCallback { private get; set; }
		public Action<ICollection<Project>> GetProjectsCallback { private get; set; }
		public Action<ICollection<Person>> GetContactsCallback { private get; set; }
		public Action<ICollection<Person>, string, Boolean?> GetContactsByFilterCallback { set; private get; }
		public Action<MemoryStream> GetImageCallback { private get; set; }

		public void BeginGetMonthlyReports(Int32 period, Boolean doRequest = false)
		{
			List<DailyReports> result = new List<DailyReports>();
			for (int i = 20; i >= 0; i--)
			{
				Random rand = new Random();
				Int32 strId = i + rand.Next(i, 500);
				Report instance = new Report
				{
					Id = strId,
					Date = DateTime.Now.AddDays(-i),
					Time = TimeSpan.FromHours(2),
					StatusOk = StatusOkMode.Valid,
					StatusSt = StatusStMode.Rejected,
					IssueManager = new IssueManager() { Id = 2, Name = "Education" },
					IssueNumber = new IssueNumber() { Id = 2, Number = "2222", Subject = "Educ" },
					IsEditable = true
					,
					Description = "had read chapter (Sorting and Grouping Bound Collections) in book (Windows Phone 8 Development   Internals)" + strId
					,
					FullProject = "/$/ELEKS/B1. SWD/Mgmt/Education (SWD)/Education/Learning/Internal/Mobile" + strId
				};
				strId = i + rand.Next(i, 500);
				Report instance1 = new Report
				{
					Id = strId,
					Date = DateTime.Now.AddDays(-i),
					Time = TimeSpan.FromHours(2),
					StatusOk = StatusOkMode.Valid,
					StatusSt = StatusStMode.Rejected,
					IssueManager = new IssueManager() { Id = 3, Name = "Office Management" },
					IssueNumber = new IssueNumber() { Id = 3, Number = "3333", Subject = "Office Man" },
					IsEditable = true
					,
					Description = "had read chapter (Sorting and Grouping Bound Collections) in book (Windows Phone 8 Development   Internals)" + strId
					,
					FullProject = "/$/ELEKS/B1. SWD/Mgmt/Education (SWD)/Education/Learning/Internal/Mobile" + strId
				};
				strId = i + rand.Next(i, 500);
				Report instance2 = new Report
				{
					Id = strId,
					Date = DateTime.Now.AddDays(-i),
					Time = TimeSpan.FromHours(2),
					StatusOk = StatusOkMode.Valid,
					StatusSt = StatusStMode.Rejected,
					IssueManager = new IssueManager() { Id = 4, Name = "VRF Translation Tool" },
					IssueNumber = new IssueNumber() { Id = 4, Number = "4444", Subject = "VRF Tran" },
					IsEditable = true
					,
					Description = "had read chapter (Sorting and Grouping Bound Collections) in book (Windows Phone 8 Development   Internals)" + strId
					,
					FullProject = "/$/ELEKS/B1. SWD/Mgmt/Education (SWD)/Education/Learning/Internal/Mobile" + strId
				};

				List<Report> resultList = new List<Report>();
				resultList.Add(instance);
				resultList.Add(instance1);
				resultList.Add(instance2);

				result.Add(new DailyReports(new DateTime(2013, 3, i), TimeSpan.FromHours(2), resultList));
			}
			if (GetMonthlyReportsCallback != null)
				GetMonthlyReportsCallback(result);
		}
		public void BeginGetDailyReports(DateTime dateTime, Boolean doRequest = false)
		{
			Random rand = new Random();
			Int32 strId = 1 + rand.Next(1, 500);
			Report instance = new Report
			{
				Id = strId,
				Date = DateTime.Now,
				Time = TimeSpan.FromHours(2),
				StatusOk = StatusOkMode.Valid,
				StatusSt = StatusStMode.Rejected,
				IssueManager = new IssueManager() { Id = 2, Name = "Education" },
				IssueNumber = new IssueNumber() { Id = 2, Number = "1111", Subject = "Educ" },
				IsEditable = true
				,
				Description = "had read chapter (Sorting and Grouping Bound Collections) in book (Windows Phone 8 Development   Internals)1"
				,
				FullProject = "/$/ELEKS/B1. SWD/Mgmt/Education (SWD)/Education/Learning/Internal/Mobile" + strId
			};
			strId = 2 + rand.Next(2, 500);
			Report instance1 = new Report
			{
				Id = strId,
				Date = DateTime.Now,
				Time = TimeSpan.FromHours(2),
				StatusOk = StatusOkMode.Valid,
				StatusSt = StatusStMode.Rejected,
				IssueManager = new IssueManager() { Id = 3, Name = "Office Management" },
				IssueNumber = new IssueNumber() { Id = 3, Number = "2222", Subject = "Office Man" },
				IsEditable = true
				,
				Description = "had read chapter (Sorting and Grouping Bound Collections) in book (Windows Phone 8 Development   Internals)2"
				,
				FullProject = "/$/ELEKS/B1. SWD/Mgmt/Education (SWD)/Education/Learning/Internal/Mobile" + strId
			};
			strId = 3 + rand.Next(3, 500);
			Report instance2 = new Report
			{
				Id = strId,
				Date = DateTime.Now,
				Time = TimeSpan.FromHours(2),
				StatusOk = StatusOkMode.Valid,
				StatusSt = StatusStMode.Rejected,
				IssueManager = new IssueManager() { Id = 4, Name = "VRF Translation Tool" },
				IssueNumber = new IssueNumber() { Id = 4, Number = "3333", Subject = "VRF Tran" },
				IsEditable = false
				,
				Description = "had read chapter (Sorting and Grouping Bound Collections) in book (Windows Phone 8 Development   Internals)3"
				,
				FullProject = "/$/ELEKS/B1. SWD/Mgmt/Education (SWD)/Education/Learning/Internal/Mobile" + strId
			};

			List<Report> resultList = new List<Report>();
			resultList.Add(instance);
			resultList.Add(instance1);
			resultList.Add(instance2);
			if (GetDailyReportsCallback != null)
				GetDailyReportsCallback(new DailyReports(new DateTime(2013, 3, 1), TimeSpan.FromHours(2), resultList));
		}
		public void BeginGetProjects(String path)
		{
			List<Project> projects = new List<Project>();
			projects.Add(new Project("Root", "1", "/$", "/$/", null, false));
			projects.Add(new Project("Name2_1", "2", "/$item2", "/$item2/", "1", true));

			projects.Add(new Project("Name5_2", "5", "/$/item2/item5", "/$/item2/item5/", "2", true));
			projects.Add(new Project("Name9_5", "9", "/$/item2/item5/item9", "/$/item2/item5/item9/", "5", true));
			projects.Add(new Project("Name12_9", "12", "/$item2/item5/item9jhsbdkjghdksfbvfbdvbdfgbhhfgkljfshbvkdsfbgdsfgjdhsfvgmnbfvmxznclvhdsf vdlsfghdlisghdflis gh", "", "9", true));
			projects.Add(new Project("Name15_12", "15", "/$item2/item5/item9/item12/item15", null, "12", true));
			projects.Add(new Project("Name13_9tlgjinbgfbfdgbdfngbdfgnbldgfkjbndklfjbnkldfjnbkljfgdbkjdnfgbkjdgnfbkjgfnbfkjbnfgb;kjdfb;lkjs;lbjd;flsbfd;lgbnd;fjbndfkjbnkjgfnb;sjbn", null, "13"
			                         , "/$/itlgjinbgfbfdgbdfngbdfgnbldgfkjbndklfjbnkldfjnbkljfgdbkjdnfgbkjdgnfbkjgfnbfkjbnfgb;kjdfb;lkjs;lbjd;flsbfd;lgbnd;fjbndfkjbnkjgfnb;sjbnem9/item13", "9", true));


			projects.Add(new Project("Name3_1", "3", "/$item3", null, "1", true));
			projects.Add(new Project("Name7_3", "7", "/$item7", null, "3", true));

			projects.Add(new Project("Name4_1", "4", "/$item4", null, "1", true));
			projects.Add(new Project("Name8_4", "8", "/$item8", null, "4", true));

			projects.Add(new Project("Name6_2", "6", "/$item6", null, "2", true));
			projects.Add(new Project("Name10_6", "10", "/$item10", null, "6", true));
			projects.Add(new Project("Name11_6", "11", "/$item11", null, "6", true));
			projects.Add(new Project("Name14_10", "14", "/$item14", null, "10", true));

			//
			if (GetProjectsCallback != null)
				GetProjectsCallback(projects);
		}

		public Task<Boolean> IsValidCredentials(String login, String password)
		{
			// Not implement
			return new Task<bool>(() => { return false; });
		}

		public List<IssueManager> GetIssueManager()
		{
			List<IssueManager> instance = new List<IssueManager>();
			instance.Add(new IssueManager() { Id = -1, Name = "" });
			instance.Add(new IssueManager() { Id = 1, Name = "Computer Service" });
			instance.Add(new IssueManager() { Id = 2, Name = "Education" });
			instance.Add(new IssueManager() { Id = 3, Name = "Office Management" });
			instance.Add(new IssueManager() { Id = 4, Name = "VRF Translation Tool" });
			return instance;
		}
		public List<IssueNumber> GetIssueNumber(Int32? idIssueManager)
		{
			List<IssueNumber> instance = new List<IssueNumber>();

			switch (idIssueManager)
			{
				case -1: instance.Add(new IssueNumber() { Id = -1, Number = "" });
				break;
				case 1: instance.Add(new IssueNumber() { Id = 1, Number = "1111", Subject = "Computer Ser" });
				break;
				case 2: instance.Add(new IssueNumber() { Id = 2, Number = "2222", Subject = "Edu" });
				break;
				case 3: instance.Add(new IssueNumber() { Id = 3, Number = "3333", Subject = "Office Man" });
				break;
				case 4: instance.Add(new IssueNumber() { Id = 4, Number = "4444", Subject = "VRF Translation Tool" });
				break;
			}
			return instance;
		}

		public Boolean DeleteReport(Int32 id)
		{
			return true;
		}
		public Report CreateReport(Report report)
		{
			Random rand = new Random();

#warning NO LOGGER
			//Logger.Debug("Start creating new report");

			Report newReport = new Report()
			{
				Date = report.Date,
				Time = report.Time,
				FullProject = report.FullProject,
				IssueNumberValue = report.IssueNumberValue,
				Description = report.Description,
				IssueManagerValue = report.IssueManagerValue,
				IsEditable = true,
				StatusOk = StatusOkMode.Valid,
				StatusSt = StatusStMode.Active,
				Id = rand.Next(0, 10000)
			};

#warning NO LOGGER
			//Logger.DebugFormat("End creating report with id '{0}'", report.Id);
			return newReport;
		}
		public Report ContinueCurrentTask(Report report)
		{
			Random rand = new Random();

			Report newReport = new Report()
			{
				Date = report.Date,
				FullProject = report.FullProject,
				IssueNumber = report.IssueNumber,
				IssueManager = report.IssueManager,
				IsEditable = true,
				StatusOk = StatusOkMode.Valid,
				StatusSt = StatusStMode.Active,
				Id = rand.Next(0, 10000)
			};

			return newReport;
		}
		public Report EditReport(Report report)
		{
			var newReport = new Report()
			{
				Date = report.Date,
				FullProject = report.FullProject,
				IssueNumber = report.IssueNumber,
				IssueManager = report.IssueManager,
				IssueNumberValue = report.IssueNumberValue,
				IssueManagerValue = report.IssueManagerValue,
				Description = report.Description,
				IsEditable = true,
				StatusOk = StatusOkMode.Valid,
				StatusSt = StatusStMode.Active,
				Id = report.Id
			};
			return newReport;
		}
		public Report SubmitReport(Report report)
		{
			report.StatusSt = StatusStMode.Rejected;
			report.StatusOk = StatusOkMode.Valid;
			return report;
		}

//		public async Task<Stream> LoadImageStreamAsync(int id)
//		{
//			var rm = new ResourceManager("TeamProMobile.Resources.AppResources", typeof(AppResources).Assembly);
//			return await Task.Run(() => (Stream)new MemoryStream(Convert.FromBase64String(rm.GetString("ImageSource"))));
//		}

		public List<PersonGroup> GetPersonList()
		{
			var resultList = new List<PersonGroup>();

//			var img = new BitmapImage();
//			var rm = new ResourceManager("TeamProMobile.Resources.AppResources", typeof(AppResources).Assembly);
//			using (var im = new MemoryStream(Convert.FromBase64String(rm.GetString("ImageSource"))))
//			{
//				img.SetSource(im);
//			}
//
//			Int32 id = 0;
//			for (Int32 i = 0; i < 10; i++)
//			{
//				String groupId = i.ToString();
//				var personGroup = new PersonGroup() { GroupingKey = "A" + groupId };
//				for (Int32 y = 0; y < 4; y++)
//				{
//					var enumerator = groupId + y.ToString();
//					personGroup.Add(new Person()
//					                {
//						Id = id++,
//						FullName = "Full name " + enumerator,
//						NameA12 = "Firs name " + enumerator,
//						SurnameA47 = "Firs name " + enumerator,
//						PhoneA2 = "+0960000" + enumerator,
//						CarModelNameA81 = "BMW " + enumerator,
//						Login = "user_" + enumerator,
//						EMail = enumerator + "@eleks.com",
//						//Picture = img,
//						Position = "developer ",
//						ProjectPath = "project_" + enumerator,
//						SexA91 = "Male",
//						CellphoneA28 = "998877"
//					});
//				}
//				resultList.Add(personGroup);
//			}

			return resultList;
		}

		public void BeginGetContacts()
		{
//			var resultList = new List<Person>();
//
//			var img = new BitmapImage();
//			var rm = new ResourceManager("TeamProMobile.Resources.AppResources", typeof(AppResources).Assembly);
//			using (var im = new MemoryStream(Convert.FromBase64String(rm.GetString("ImageSource"))))
//			{
//				img.SetSource(im);
//			}
//
//			Int32 id = 0;
//			for (Int32 i = 0; i < 10; i++)
//			{
//				String groupId = i.ToString();
//				var personGroup = new PersonGroup() { GroupingKey = "A" + groupId };
//				for (Int32 y = 0; y < 4; y++)
//				{
//					var enumerator = groupId + y.ToString();
//					personGroup.Add(new Person()
//					                {
//						Id = id++,
//						FullName = "Full name " + enumerator,
//						NameA12 = "Firs name " + enumerator,
//						SurnameA47 = "Firs name " + enumerator,
//						PhoneA2 = "+0960000" + enumerator,
//						CarModelNameA81 = "BMW " + enumerator,
//						Login = "user_" + enumerator,
//						EMail = enumerator + "@eleks.com",
//						//Picture = img,
//						Position = "developer ",
//						ProjectPath = "project_" + enumerator,
//						SexA91 = "Male",
//						CellphoneA28 = "998877"
//					});
//				}
//				//resultList.Add(personGroup);
//			}
			//
			// if (GetContactsCallback != null)
			//GetContactsCallback(resultList);
		}

		public void BeginGetContactsByFilter(string filter, Boolean doRequest = false)
		{
			//throw new NotImplementedException();
		}

		public void BeginGetImage(Int32 id)
		{
//			var rm = new ResourceManager("TeamProMobile.Resources.AppResources", typeof(AppResources).Assembly);
//			MemoryStream image = new MemoryStream(Convert.FromBase64String(rm.GetString("ImageSource")));
//
//			if (GetImageCallback != null)
//				GetImageCallback(image);
		}
	}
}

