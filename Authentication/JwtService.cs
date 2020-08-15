using AnalyticsWebapps.Models.Identity;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Authentication
{
    public class JwtService
    {
        private readonly string secret;
        private readonly string expDate;
        private readonly HttpContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly SymmetricSecurityKey securityKey;
        private readonly string audience;
        private readonly string issuer;

        public JwtService(
            IConfiguration config, 
            IHttpContextAccessor context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            this.secret = config.GetSection("JwtConfig").GetSection("secret").Value;
            this.expDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
            this.context = context.HttpContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.secret));
            this.audience = this.context.Request.PathBase.Value.IsNullOrEmpty() ? "localhost" : this.context.Request.PathBase.Value;
            this.issuer = this.audience;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public bool ValidateSecurityToken(string token)
        {
            if (token.IsNullOrEmpty())
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = this.securityKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = this.issuer,
                    ValidAudience = this.audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string GenerateSecurityToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("FullName", user.FirstName + " " + user.LastName),
                    new Claim("UserName", user.UserName),
                    new Claim("Id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(expDate)),
                Issuer = this.issuer,
                Audience = this.audience,
                SigningCredentials = new SigningCredentials(this.securityKey, SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
