using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using AnalyticsWebapps.Models.Identity;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Web.Helpers;
using Models;

namespace Authentication
{
    public partial class AuthenticationApi
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly ILogger<AuthenticationApi> logger;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment env;
        private readonly HttpContext context;
        private readonly JwtService jwtService;

        public AuthenticationApi(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILogger<AuthenticationApi> logger,
            IEmailSender emailSender,
            IWebHostEnvironment env,
            IHttpContextAccessor context,
            JwtService jwtService
        )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.logger = logger;
            this.emailSender = emailSender;
            this.env = env;
            this.context = context.HttpContext;
            this.jwtService = jwtService;
        }
    }
}
