﻿using iShop.Web.Server.Commons.BaseClasses;
using iShop.Web.Server.Core.Models;
using iShop.Web.Server.Core.Resources;

namespace iShop.Web.Server.Mapping
{
    public class ApplicationUserProfile:BaseProfile
    {
        public ApplicationUserProfile(string profileName) 
            : base(profileName)
        {
        }

        protected override void CreateMap()
        {
            CreateMap<ApplicationUser, ApplicationUserResource>();
            CreateMap<ApplicationUserResource, ApplicationUser>();
        }
    }
}