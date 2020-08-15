using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class CustomStatusCodes
    {
        public const int WindowsAuthUnauthorized = 460;
    }

    [DefaultStatusCode(CustomStatusCodes.WindowsAuthUnauthorized)]
    public class WindowsAuthUnauthorizedResult : StatusCodeResult
    {
        public WindowsAuthUnauthorizedResult() : base(CustomStatusCodes.WindowsAuthUnauthorized)
        {
        }
    }
}
