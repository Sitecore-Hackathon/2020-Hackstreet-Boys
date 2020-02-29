using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Feature.PageContent.Models.ViewModels
{
    public class MenuViewModel
    {
        public List<MenuLink> MenuLinks { get; set; }
        public bool UserLoggedIn { get; set; }
    }

    public class MenuLink
    {
        public string Label { get; set; }
        public string LinkUrl { get; set; }
        public bool ShowOnlyOnLogin { get; set; }
        public bool HideWithLoggedInUser { get; set; }
    }
}