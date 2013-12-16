using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace TeamProMobileApplicationIOS.Internals
{
	public static class EnumHelper <T1, T2>
		where T2: Attribute{
		private static ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
		private static readonly Dictionary<Type, Dictionary<T1, T2>> _index = new Dictionary<Type,Dictionary<T1,T2>>();

		private static T2 InitEnumAttribute(T1 value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString()); 
			Object[] attributes = fi.GetCustomAttributes(typeof(T2), false);
			if (attributes != null && attributes.Length > 0)
				return (T2)attributes[0];
			return null;
		}

		public static IReadOnlyDictionary<T1, T2> GetValues(){
			_cacheLock.EnterUpgradeableReadLock();
			try
			{
				Dictionary<T1, T2> values;
				_index.TryGetValue(typeof(T1), out values);
				if (values == null)
				{
					_cacheLock.EnterWriteLock();
					try
					{
						Type enumType = typeof(T1);
						// Can't use generic type constraints on value types,
						// so have to do check like this    
						if (enumType.BaseType != typeof(Enum))
							throw new ArgumentException("T must be of type System.Enum");
						Array enumValArray = Enum.GetValues(enumType);
						values = new Dictionary<T1, T2>(enumValArray.Length);
						foreach (int val in enumValArray)
						{
							T1 eVal = (T1)Enum.Parse(enumType, val.ToString());
							values.Add(eVal, InitEnumAttribute(eVal));
						}
						_index.Add(typeof(T1), values);
					}
					finally
					{
						_cacheLock.ExitWriteLock();
					}
				}
				return values;
			}
			finally
			{
				_cacheLock.ExitUpgradeableReadLock();
			}
		}

		public static T2 GetAttribute(T1 value)
		{
			IReadOnlyDictionary<T1, T2> values = GetValues();
			T2 result;
			values.TryGetValue(value, out result);
			return result;
		}

	}
}

