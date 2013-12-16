using System;
using TeamProMobileApplicationIOS.Internals;

namespace TeamProMobileApplicationIOS.Model
{
    class UserContext
    {
        public String UserName { get; private set; }
        public String Password { get; private set; }
        public String Domain { get; private set; }

        public UserContext(String userName, String password, String domain)
        {
            UserName = userName;
            Password = password;
            Domain = domain;
        }

        public static UserContext GetUserContext()
        {
            String login = Settings.Instance.Login;
            String password = Settings.Instance.Password;
            String userName = null;
            String domain = null;
            if (!String.IsNullOrEmpty(login))
            {
                String[] loginParts = login.Split('\\');

                if (loginParts.Length > 1)
                {
                    domain = loginParts[0];
                    userName = loginParts[1];
                }
                else
                {
                    userName = login;
                }
            }
            return new UserContext(userName, password, domain);
        }
    }
}
