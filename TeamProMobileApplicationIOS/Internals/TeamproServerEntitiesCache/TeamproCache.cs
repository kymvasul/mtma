using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamProMobileApplicationIOS.Internals.TeamproServerEntitiesCache
{
	internal class CacheValue<T>
	{
		public ICollection<T> Values { get; private set; }
		public DateTime LastAccessDate { get; set; }

		public CacheValue(ICollection<T> values) : this(values, DateTime.Now) { }
		public CacheValue(ICollection<T> values, DateTime lastAccessDate)
		{
			Values = values;
			LastAccessDate = lastAccessDate;
		}
	}

	class TeamproCache<T1, T2>
	{
		private readonly Func<T1, T2> _keyRetriever;
		public TeamproCache(Func<T1, T2> keyRetriever)
		{
			_keyRetriever = keyRetriever;
		}

		private readonly Dictionary<T2, CacheValue<T1>> _entitiesIndex = new Dictionary<T2, CacheValue<T1>>();
		//private DateTime _minAccessDate;
		public ICollection<T1> GetEntities(T2 id)
		{
			CacheValue<T1> result;
			_entitiesIndex.TryGetValue(id, out result);
			//updates  lastAccess date property in the collection's key.
			if (result != null)
			{
				result.LastAccessDate = DateTime.Now;
				return result.Values;
			}
			return null;
		}

		public T1 GetEntityById(T2 id)
		{
			ICollection<T1> collection = GetEntities(id);
			if (collection == null)
				return default(T1);

			return collection.FirstOrDefault();
		}

		public ICollection<T1> GetAllEntities()
		{
			ICollection<T1> result = new List<T1>();

			foreach (var cacheValue in _entitiesIndex.Values)
			{
				foreach (T1 value in cacheValue.Values)
				{
					result.Add(value);
				}
			}
			if (result.Count > 0)
			{
				return result;
			}
			return null;
		}

		public void CacheEntities(ICollection<T1> entities)
		{
			//organise internal dictionary of the objects to cache
			Dictionary<T2, ICollection<T1>> objectsToCache = new Dictionary<T2, ICollection<T1>>();
			foreach (T1 entity in entities)
			{
				T2 id = _keyRetriever(entity);
				ICollection<T1> cacheCollection;
				objectsToCache.TryGetValue(id, out cacheCollection);
				if (cacheCollection == null)
				{
					cacheCollection = new List<T1>();
					objectsToCache.Add(id, cacheCollection);
				}
				cacheCollection.Add(entity);
			}
			foreach (KeyValuePair<T2, ICollection<T1>> objectToCache in objectsToCache)
			{
				_entitiesIndex[objectToCache.Key] = new CacheValue<T1>(objectToCache.Value, DateTime.Now);
			}
		}

		public void MerageCacheEntities(ICollection<T1> entities, Func<T1, Boolean> condition)
		{
			//organise internal dictionary of the objects to cache
			Dictionary<T2, ICollection<T1>> objectsToCache = new Dictionary<T2, ICollection<T1>>();
			foreach (T1 entity in entities)
			{
				if (condition(entity))
				{
					T2 id = _keyRetriever(entity);
					ICollection<T1> cacheCollection;
					objectsToCache.TryGetValue(id, out cacheCollection);
					if (cacheCollection == null)
					{
						cacheCollection = new List<T1>();
						objectsToCache.Add(id, cacheCollection);
					}
					cacheCollection.Add(entity);
				}
			}
			foreach (KeyValuePair<T2, ICollection<T1>> objectToCache in objectsToCache)
			{
				_entitiesIndex[objectToCache.Key] = new CacheValue<T1>(objectToCache.Value, DateTime.Now);
			}
		}

		public void Treat()
		{
			/*if(_entitiesIndex.Count == 0)
                return;*/
			//for(CacheKey key)
		}
		/*private void SetMinAccessDate()
        {
            if (_entitiesIndex.Count == 0)
                return;
            _minAccessDate = _entitiesIndex.Keys.Min(k => k.LastAccessDate);
        }*/

		public void ClearCacheEntities()
		{
			_entitiesIndex.Clear();
		}

		public void ClearCacheEntities(Func<T1, Boolean> condition)
		{
			var keys = 
				(from keyValuePair in _entitiesIndex 
				 from value in keyValuePair.Value.Values 
				 where condition(value) 
				 select keyValuePair.Key).ToList();

			foreach (T2 key in keys)
			{
				_entitiesIndex.Remove(key);
			}
		}
	}
}

