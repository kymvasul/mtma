using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamProMobileApplicationIOS.Model;
using System.Threading.Tasks;

namespace TeamProMobileApplicationIOS.Internals.TeamproServerEntitiesCache
{
	class CacheManager
	{
		private TeamproCache<Report, int> _reportsCache;
		private TeamproCache<Project, string> _projectsHierarchyCache;
		private TeamproCache<Project, string> _projectsCache;
		private TeamproCache<Person, Int32> _contactsCache;

		private CacheManager()
		{
			_reportsCache = new TeamproCache<Report, int>(r => InternalsMethods.CalculateReportPeriod(r.Date));
			_projectsHierarchyCache = new TeamproCache<Project, string>(p => p.Path.Substring(0, p.Path.LastIndexOf('/')+1).ToUpper());
			_projectsCache = new TeamproCache<Project, string>(p=>p.Path.ToUpper());
			_contactsCache = new TeamproCache<Person, Int32>(p=>p.Id);
			IsContactsCachePopulated = false;
		}
		private static CacheManager _instance = new CacheManager();
		public static CacheManager Instance { get { return _instance; } }

		//
		public TeamproCache<Person, int> ContactsCache { get { return _contactsCache; } }
		public TeamproCache<Report, int> ReportsCache { get { return _reportsCache; }}
		public TeamproCache<Project, string> ProjectsHierarchyCache { get { return _projectsHierarchyCache; } }
		public TeamproCache<Project, string> ProjectsCache { get { return _projectsCache; } }

//		public void InitContactsCacheAsync()
//		{
//			if (!Instance.IsContactsCachePopulated)
//				Task.Factory.StartNew(async () =>
//				                      {
//					try
//					{
//
//						var contacts = await new PersoneContactManager().GetAllPersoneContactsWithKnownPropertiesAndExtendedPropertiesAsync();
//						Instance.ContactsCache.CacheEntities((ICollection<Person>)contacts);
//						IsContactsCachePopulated = true;
//						if (ContactsPopulated != null)
//							ContactsPopulated();
//					}
//					catch (Exception ex)
//					{
//						Logger.Error("Failed to initContact cache from Local Contacts", ex);
//						Instance.ContactsCache.CacheEntities(new Collection<Person>());                            
//						IsContactsCachePopulated = true;
//						if (ContactsPopulated != null)
//							ContactsPopulated();
//					}
//				});
//		}

		public bool IsContactsCachePopulated { get; private set; }
		public Action ContactsPopulated { private get; set; }
	}
}

