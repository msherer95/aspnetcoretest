using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnalyticsWebapps.Database;
using AnalyticsWebapps.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Webapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpContext _context;
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly HealthContext dbContext;

        public WeatherForecastController(
            HealthContext dbContext,
            ILogger<WeatherForecastController> logger, 
            IHttpContextAccessor context, 
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _context = context.HttpContext;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<WeatherForecastModel> Get()
        {
            var a = this.dbContext.SyndromicEvents
                .Include(c => c.DrugInformation)
                .FirstOrDefault();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
