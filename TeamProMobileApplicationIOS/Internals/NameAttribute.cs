using System;

namespace TeamProMobileApplicationIOS.Internals
{
	class NameAttribute: Attribute
	{
		public String Name { get; private set; }
		public NameAttribute(String name)
		{
			Name = name;
		}
	}
}

