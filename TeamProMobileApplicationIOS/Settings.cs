using System;
using Microsoft;
using MonoTouch;
//using OpenTK;
using MonoTouch.Foundation;

namespace TeamProMobileApplicationIOS
{
	class Settings
	{

			public static Settings Instance
		{
			get { return _instance; }
		}

		public String HostName
		{
			get
			{
				var hostName = GetValue(_hostNameKey, "");
				return string.IsNullOrEmpty(hostName) ? _DefaultHostName : hostName;
			}
			set { SetValue(_hostNameKey, value); }
		}

		public Boolean IsLogedIn {
			get {
				var isLogedIn = GetValue (_loginInKey, false);
				return isLogedIn;
			}
			set { SetValue (_loginInKey, value); }

		}

		public String Login
		{
			get
			{
				var login = (String)GetValue(_loginKey, "");
				return String.IsNullOrEmpty(login) ? String.Empty : login;
			}
			set { SetValue(_loginKey, value); }
		}

		public String Password
		{
			get
			{
				var password = (String)GetValue(_passwordKey, "");
				return String.IsNullOrEmpty(password) ? _DefaultHostName : password;
			}
			set { SetValue(_passwordKey, value); }
		}

			
		public string GetValue (string key, string defVal = "")
		{

			string strValue = _preferences.StringForKey (key);
			return strValue;
		}

		public bool GetValue (string key, bool defVal = false)
		{
			bool boolValue = _preferences.BoolForKey(key);
			return boolValue;
		}

		#region Setters

		public bool SetValue (string key, string stringValue)
		{
			bool isValueChanged = false;

			if (_preferences.DataForKey(key)==null) 
			{
				isValueChanged = true;
			}
			NSUserDefaults.StandardUserDefaults.SetString(stringValue, key);
			NSUserDefaults.StandardUserDefaults.Init();
			return isValueChanged;
		}

		public bool SetValue (string key, int intValue)
		{
			bool isValueChanged = false;

			_editor = _preferences;

			if (_preferences.DataForKey(key) == null) 
			{
				isValueChanged = true;
			}
			NSUserDefaults.StandardUserDefaults.SetInt (intValue, key);
			NSUserDefaults.StandardUserDefaults.Init();

			return isValueChanged;
		}

		public bool SetValue (string key, float floatValue)
		{
			bool isValueChanged = false;

			if (_preferences.DataForKey(key) == null) 
			{
				isValueChanged = true;
			}
			NSUserDefaults.StandardUserDefaults.SetFloat (floatValue, key);
			NSUserDefaults.StandardUserDefaults.Init();

			return isValueChanged;
		}

		public bool SetValue (string key, bool boolValue)
		{
			bool isValueChanged = false;
			if (_preferences.DataForKey(key) == null) 
			{
				isValueChanged = true;
			}

			_preferences.SetBool (boolValue, key);
			_preferences.Init ();

			return isValueChanged;
		}
		#endregion

		private const string _hostNameKey = "HostName";
		private const string _loginInKey = "IsLogedIn";
		private const string _loginKey = "Login";
		private const string _passwordKey = "Password";

		private NSUserDefaults _editor = new NSUserDefaults ();
		private NSUserDefaults _preferences = NSUserDefaults.StandardUserDefaults;
		private const string _DefaultHostName = "http://193.105.219.5";
		private static Settings _instance = new Settings ();
	}
}

