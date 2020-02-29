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
    }

    public class MenuLink
    {
        public string Label { get; set; }
        public string LinkUrl { get; set; }
    }
}