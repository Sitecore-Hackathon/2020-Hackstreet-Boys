using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Foundation.Teams.Models
{
    public class CreateOrganizationTeamResponse
    {
        public string name { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
        public string privacy { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string members_url { get; set; }
        public string repositories_url { get; set; }
        public string permission { get; set; }
        public object parent { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int members_count { get; set; }
        public int repos_count { get; set; }
        public Organization organization { get; set; }
    }
    public class Organization
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string hooks_url { get; set; }
        public string issues_url { get; set; }
        public string members_url { get; set; }
        public string public_members_url { get; set; }
        public string avatar_url { get; set; }
        public object description { get; set; }
        public bool is_verified { get; set; }
        public bool has_organization_projects { get; set; }
        public bool has_repository_projects { get; set; }
        public int public_repos { get; set; }
        public int public_gists { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
        public string html_url { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string type { get; set; }
    }

  
}