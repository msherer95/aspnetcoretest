using System;
using System.IO;

namespace Routes
{
    public class AuthenticationRoutes
    {
        public const string BaseUri = BaseRoutes.AnonymousBase + "/authentication";
        public const string Login = "Login";
        public const string Logout = "Logout";
        public const string Register = "Register";
        public const string ConfirmEmail = "ConfirmEmail";
    }
}
