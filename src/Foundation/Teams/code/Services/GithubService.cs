using Hackathon.Foundation.Teams.Models;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Hackathon.Foundation.Teams.Connectors;
using System.Net.Http;

namespace Hackathon.Foundation.Teams.Services
{
	public class GithubService
	{
        public CreateOrganizationTeamResponse CreateOrganizationTeam(string teamName, string teamDescription)
        {
            string orgName = Settings.GetSetting("hackathon.OrganizationName");
            string apiUrl = Settings.GetSetting("hackathon.GithubCreateOrganizationTeamUrl").Replace("{org-name}", orgName);
            dynamic body = new JObject();
            body.name = teamName;
            body.description = teamDescription;
            body.permission = "admin";

            CreateOrganizationTeamResponse response = GithubConnector.ExecuteRequest<CreateOrganizationTeamResponse>(HttpMethod.Post, apiUrl, body, "201 Created");
            return response;
        }

        public CreateTeamRepo CreateOrganizationTeamRepo(string teamId, string repoName)
        {
            ///orgs/{org-name}/repos
            string orgName = Settings.GetSetting("hackathon.OrganizationName");
            string apiUrl = Settings.GetSetting("hackathon.GithubCreateOrganizationTeamRepoUrl").Replace("{org-name}", orgName);
            dynamic body = new JObject();
            body.name = repoName;
            body.team_id = teamId;
            body.visibility = "public";

            CreateTeamRepo response = GithubConnector.ExecuteRequest<CreateTeamRepo>(HttpMethod.Put, apiUrl, body, "201 Created");
            return response;
        }

        public AddMemberToRepo AddMemberToRepoRepo(string teamId, string repoName, string username)
        {
            ///repos/{org-name}/{repo-name}/collaborators/{username}
            string orgName = Settings.GetSetting("hackathon.OrganizationName");
            string apiUrl = Settings.GetSetting("hackathon.GithubAddUserToRepoUrl").Replace("{org-name}", orgName).Replace("{repo-name}", repoName).Replace("{username}", username);
            dynamic body = new JObject();
            body.permission = "admin";

            AddMemberToRepo response = GithubConnector.ExecuteRequest<AddMemberToRepo>(HttpMethod.Put, apiUrl, body, "201 Created");
            return response;
        }
    }
}