using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackathon.Foundation.Teams.Models;

namespace Hackathon.Feature.PageContent.Models.ViewModels
{
    public class TeamListViewModel
    {
        public string Title { get; set; }
        public List<Team> Teams { get; set; }
    }
}