using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Models;
using Routes;
using Utils;
using Utils.Exceptions;
using Utils.Extensions;

namespace Webapp.Controllers
{
    [ApiController]
    [Route("api/anonymous/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationApi authApi;

        public AuthenticationController(AuthenticationApi authApi)
        {
            this.authApi = authApi;
        }
        
        [HttpPost(AuthenticationRoutes.Register)]
        public async Task<IActionResult> Register([FromForm] RegistrationUser user)
        {
            try
            {
                await this.authApi.ActiveDirectoryRegisterAsync(user);
                return Ok();
            }
            catch(ArgumentException err)
            {
                return BadRequest(err.Message);
            }
            catch(InvalidActiveDirectoryUserException)
            {
                return this.WindowsAuthUnauthorized();
            }
        }

        [HttpPost(AuthenticationRoutes.Login)]
        public async Task<IActionResult> Login([FromForm] LoginUser user, [FromForm] bool rememberMe)
        {
            try
            {
                await this.authApi.ActiveDirectoryLoginAsync();
                return Ok();
            }
            catch(UnauthorizedAccessException)
            {
                return this.WindowsAuthUnauthorized();
            }
        }
    }
}
