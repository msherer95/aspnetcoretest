using AnalyticsWebapps.Models.Identity;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Utils.Exceptions;

namespace Authentication
{
    public partial class AuthenticationApi
    {
        public RegistrationUser GetActiveDirectoryUser(string userName)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(context, userName);
                return new RegistrationUser
                {
                    FirstName = user.GivenName,
                    LastName = user.Surname,
                    UserName = user.Name,
                    Email = user.EmailAddress
                };
            }
        }

        public async Task ActiveDirectoryRegisterAsync(RegistrationUser user)
        {
            if (!this.UsingWindowsAuthentication())
            {
                throw new InvalidActiveDirectoryUserException();
            }

            user.UserName = this.context.User.Identity.Name;
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                bool validUser = this.UsingWindowsAuthentication() ||
                    (!user.UserName.IsNullOrEmpty() && !user.Password.IsNullOrEmpty() && context.ValidateCredentials(user.UserName, user.Password));

                if (validUser)
                {
                    UserPrincipal adUser = UserPrincipal.FindByIdentity(context, user.UserName);
                    ApplicationUser appUser = this.mapper.Map<ApplicationUser>(user);
                    appUser.UserName = adUser.Name;
                    IdentityResult result = await this.userManager.CreateAsync(appUser, this.GenerateRandomPassword());

                    if (result.Succeeded)
                    {
                        this.logger.LogInformation($"User {appUser.UserName} created");
                        this.GenerateConfirmationEmail(appUser);
                        return;
                    }

                    this.logger.LogError($"Failed to register user {user.UserName}");
                    throw new ArgumentException(JsonConvert.SerializeObject(result.Errors));
                }

                throw new InvalidActiveDirectoryUserException();
            }
        }

        public bool UsingWindowsAuthentication()
        {
            var identity = this.context.User.Identity;
            return identity.AuthenticationType == "NTLM"
                || identity.AuthenticationType == "Negotiate"
                || identity.AuthenticationType == "Kerberos";
        }

        public async Task ActiveDirectoryLoginAsync()
        {
            if (!this.UsingWindowsAuthentication())
            {
                throw new InvalidActiveDirectoryUserException();
            }

            using (var context = new PrincipalContext(ContextType.Domain))
            {
                
                UserPrincipal adUser = UserPrincipal.FindByIdentity(context, this.context.User.Identity.Name);
                ApplicationUser appUser = await this.userManager.FindByNameAsync(adUser.Name);
                if (appUser != null)
                {
                    this.AddAccessAndRefreshTokens(appUser);
                }
                else
                {
                    throw new UserNotRegisteredException();
                }
            }
        }

        // Taken from: https://www.ryadel.com/en/c-sharp-random-password-generator-asp-net-core-mvc/
        private string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true,
                RequiredLength = 8,
                RequiredUniqueChars = 1
            };

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?_-"                        // non-alphanumeric
            };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}
