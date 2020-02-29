using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Feature.PageContent.Models.ViewModels
{
    public class TeamListViewModel
    {
        public string Title { get; set; }
        public List<Team> Teams { get; set; }
    }
}