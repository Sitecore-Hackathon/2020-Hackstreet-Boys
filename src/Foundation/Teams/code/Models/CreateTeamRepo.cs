using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Foundation.Teams.Models
{
    public class CreateTeamRepo
    {
        public string name { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string url { get; set; }
        public string full_name { get; set; }
        public string html_url { get; set; }
    }
}