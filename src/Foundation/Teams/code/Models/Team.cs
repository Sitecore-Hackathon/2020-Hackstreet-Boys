using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Foundation.Teams.Models
{
    public class Team
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Url { get; set; } 
        public string ThumbnailUrl { get; set; }
    }
}