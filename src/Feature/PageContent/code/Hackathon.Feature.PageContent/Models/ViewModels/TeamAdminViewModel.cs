using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Feature.PageContent.Models.ViewModels
{
    public class TeamAdminViewModel
    {
        public Hackathon.Foundation.Teams.Models.Team Team { get; set; }
        public string SecretJoinString { get; set; }
    }
}