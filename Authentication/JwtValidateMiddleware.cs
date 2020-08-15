using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Routes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Authentication
{
    public class JwtValidateMiddleware : IMiddleware
    {
        private readonly JwtService jwtService;
        private readonly AuthenticationApi authApi;

        public JwtValidateMiddleware(JwtService jwtService, AuthenticationApi authApi)
        {
            this.jwtService = jwtService;
            this.authApi = authApi;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Path.Value.StartsWith(BaseRoutes.ApiBase) || context.Request.Path.Value.StartsWith(BaseRoutes.AnonymousBase))
            {
                await next(context);
                return;
            }

            string token = context.Request.Headers["Bearer"];
            if (!this.authApi.UsingWindowsAuthentication() || jwtService.ValidateSecurityToken(token))
            {
                await next(context);
                return;
            }

            if (this.authApi.UsingWindowsAuthentication())
            {
                await this.authApi.ActiveDirectoryLoginAsync();
            }

            context.Response.StatusCode = StatusCodes.Status305UseProxy;
            return;
        }
    }
}
