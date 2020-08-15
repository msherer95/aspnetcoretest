using AnalyticsWebapps.Models;
using AnalyticsWebapps.Models.Identity;
using Database;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AnalyticsWebapps.Database
{
    public class ApplicationDbContext : RoleBasedAuthorizationDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private IConfiguration configuration;

        [PersonalData, Required]
        public string FirstName { get; set; }

        [PersonalData, Required]
        public string LastName { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IConfiguration configuration,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRoleClaim<Guid>>(entity => { entity.ToTable(name: "RoleClaims"); });
            builder.Entity<ApplicationRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserClaim<Guid>>(entity => { entity.ToTable(name: "UserClaims"); });
            builder.Entity<IdentityUserRole<Guid>>(entity => { entity.ToTable(name: "UserRoles"); });
            builder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<IdentityUserLogin<Guid>>(entity => { entity.ToTable(name: "UserLogins"); });
            builder.Entity<IdentityUserToken<Guid>>(entity => { entity.ToTable(name: "UserTokens"); });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.configuration.GetConnectionString("Authentication"));
        }
    }
}
