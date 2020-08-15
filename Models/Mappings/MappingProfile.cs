using AnalyticsWebapps.Models.Identity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<RegistrationUser, ApplicationUser>();
        }
    }
}
