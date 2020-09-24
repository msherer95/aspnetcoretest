using AnalyticsWebapps.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Routes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authentication
{
    public partial class AuthenticationApi
    {
        public async Task LoginAsync(LoginUser user, bool rememberMe)
        {
            string userName = user.UserName;
            ApplicationUser appUser = await this.userManager.FindByNameAsync(user.UserName);

            if (appUser == null)
            {
                this.logger.LogError($"Unauthorized login attempt by {user.UserName}");
                throw new UnauthorizedAccessException();
            }

            var result = await this.signInManager.CheckPasswordSignInAsync(appUser, user.Password, false);
            if (!result.Succeeded)
            {
                this.logger.LogError($"Unauthorized login attempt by {user.UserName}");
                throw new UnauthorizedAccessException();
            }

            this.AddAccessAndRefreshTokens(appUser);
        }

        public async Task RegisterAsync(RegistrationUser user)
        {
            var appUser = this.mapper.Map<ApplicationUser>(user);
            IdentityResult result = await this.userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {
                this.logger.LogInformation($"User {appUser.UserName} created");
                await this.GenerateConfirmationEmail(appUser);
                return;
            }

            this.logger.LogError($"Failed to register user {user.UserName}");
            throw new ArgumentException(JsonConvert.SerializeObject(result.Errors));
        }

        private async Task GenerateConfirmationEmail(ApplicationUser appUser)
        {
            string code = await this.userManager.GenerateEmailConfirmationTokenAsync(appUser);

            var path = UriHelper.BuildRelative(this.context.Request.Host.Value,
                AuthenticationRoutes.BaseUri + "/" + AuthenticationRoutes.ConfirmEmail,
                QueryString.Create("code", code)
            );

            await this.emailSender.SendEmailAsync(appUser.Email, "Confirm your email",
                $"Please confirm your account by <a href='{path}'>Clicking here</a>");
        }

        private void AddAccessAndRefreshTokens(ApplicationUser appUser)
        {
            string accessToken = this.jwtService.GenerateSecurityToken(appUser);
            string refreshToken = this.jwtService.GenerateRefreshToken();

            this.context.Response.Headers.Add("access_token", accessToken);
            this.context.Response.Headers.Add("refresh_token", refreshToken);
        }
    }
}
