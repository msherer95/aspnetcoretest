using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Utils.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult WindowsAuthUnauthorized(this ControllerBase controller)
        {
            return new WindowsAuthUnauthorizedResult();
        }
    }
}
