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
        /// <summary>
        /// Create team inside Github Organization
        /// </summary>
        /// <param name="teamName">Your team name</param>
        /// <param name="teamDescription">your team description</param>
        /// <returns></returns>
        public CreateOrganizationTeamResponse CreateOrganizationTeam(string teamName, string teamDescription)
        {
            string orgName = Settings.GetSetting("hackathon.OrganizationName");
            string apiUrl = Settings.GetSetting("hackathon.GithubCreateOrganizationTeamUrl").Replace("{org-name}", orgName);
            dynamic body = new JObject();
            body.name = teamName;
            body.description = teamDescription;
            body.permission = "admin";
            body.privacy = "closed";

            CreateOrganizationTeamResponse response = GithubConnector.ExecuteRequest<CreateOrganizationTeamResponse>(HttpMethod.Post, apiUrl, body, "201 Created");
            return response;
        }

        /// <summary>
        /// Create a github reporsitory and assign it to a team from the organization
        /// </summary>
        /// <param name="teamId">team ID, this is stored in sitecore team item</param>
        /// <param name="repoName">repositoy name</param>
        /// <returns></returns>
        public CreateTeamRepo CreateOrganizationTeamRepo(string teamId, string repoName)
        {
            ///orgs/{org-name}/repos
            string orgName = Settings.GetSetting("hackathon.OrganizationName");
            string apiUrl = Settings.GetSetting("hackathon.GithubCreateOrganizationTeamRepoUrl").Replace("{org-name}", orgName);
            dynamic body = new JObject();
            body.name = repoName;
            body.team_id = teamId;
            body.Private = false;
            body.visibility = "public";


            CreateTeamRepo response = GithubConnector.ExecuteRequest<CreateTeamRepo>(HttpMethod.Post, apiUrl, body, "201 Created");
            return response;
        }

        /// <summary>
        /// Add a github user to a repository
        /// </summary>
        /// <param name="repoName">repositoy name</param>
        /// <param name="username">github username</param>
        /// <returns></returns>
        public AddMemberToRepo AddMemberToRepoRepo( string repoName, string username)
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