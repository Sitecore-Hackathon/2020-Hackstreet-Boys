using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackathon.Foundation.Teams.Models;

namespace Hackathon.Feature.PageContent.Models.ViewModels
{
    public class TeamDetailsViewModel
    {
        public Team Team { get; set; }
        public List<TeamMember> TeamMembers { get; set; }
    }
}