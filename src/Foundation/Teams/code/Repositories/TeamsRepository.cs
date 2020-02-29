using Hackathon.Foundation.Teams.Models;
using Hackathon.Foundation.Teams.Services;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Configuration;
using Sitecore.SecurityModel;
using Hackathon.Foundation.Teams;

namespace Hackathon.Foundation.Teams.Repositories
{
	public class TeamsRepository
	{
        private GithubService _service = new GithubService();
        /// <summary>
        /// Create Hackathon Team
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="teamDescription"></param>
        /// <param name="githubUsername"></param>
        /// <returns></returns>
        public Sitecore.Data.Items.Item CreateHackathonTeam(string teamName, string teamDescription, string githubUsername)
        {
            using (new SecurityDisabler())
            {
                var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                var teamsfolder = masterDb.GetItem(Settings.GetSetting(Constants.Settings.TeamsFolder));
                var friendlyTeamName = Sitecore.Data.Items.ItemUtil.ProposeValidItemName(teamName);

                var teamResponse = _service.CreateOrganizationTeam(friendlyTeamName, teamDescription);
                var repoResponse = _service.CreateOrganizationTeamRepo(teamResponse.id.ToString(), DateTime.Now.Year + "-" + friendlyTeamName);

                var teamItem = teamsfolder.Add(friendlyTeamName, new TemplateID(new ID(Constants.Templates.TeamsTemplate)));
                
                teamItem.Editing.BeginEdit();
                teamItem.Fields[Constants.Templates.TeamGithubData.Name].Value = teamResponse.name;
                teamItem.Fields[Constants.Templates.TeamGithubData.Id].Value = teamResponse.id.ToString();
                teamItem.Fields[Constants.Templates.TeamGithubData.Slug].Value = teamResponse.slug;
                teamItem.Fields[Constants.Templates.TeamGithubData.Description].Value = teamResponse.description;
                teamItem.Fields[Constants.Templates.TeamGithubData.Url].Value = teamResponse.html_url;
                teamItem.Fields[Constants.Templates.TeamGithubData.RepoName].Value = repoResponse.name;
                teamItem.Fields[Constants.Templates.TeamGithubData.RepoFullName].Value = repoResponse.full_name;
                teamItem.Fields[Constants.Templates.TeamGithubData.RepoUrl].Value = repoResponse.html_url;
                teamItem.Editing.EndEdit();

                if (Sitecore.Context.User.IsAuthenticated)
                {
                    var teamMember = CreateHackathonTeamMember(Sitecore.Context.User.Profile.FullName, "", Sitecore.Context.User.Profile.Email, githubUsername);
                    teamItem.Editing.BeginEdit();
                    Sitecore.Data.Fields.MultilistField members = (Sitecore.Data.Fields.MultilistField)teamItem.Fields[Constants.Templates.TeamGithubData.Members];
                    members.Add(teamMember.ID.ToString());
                    teamItem.Editing.EndEdit();
                }

                try
                {
                    _service.AddMemberToRepoRepo(repoResponse.name, githubUsername);
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                }

                return teamItem;
            }

        }

        /// <summary>
        /// Create Hackathon team member
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="githubUsername"></param>
        /// <returns></returns>
        public Sitecore.Data.Items.Item CreateHackathonTeamMember(string firstName, string lastName, string email, string githubUsername)
        {
            using (new SecurityDisabler())
            {
                var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                var teamMembersfolder = masterDb.GetItem(Settings.GetSetting(Constants.Settings.TeamMembersFolder));
                var friendlyTeamName = Sitecore.Data.Items.ItemUtil.ProposeValidItemName(firstName + " " + lastName);

                var teamMemberItem = teamMembersfolder.Add(friendlyTeamName, new TemplateID(new ID(Constants.Templates.TeamsMemberTemplate)));
                teamMemberItem.Editing.BeginEdit();
                teamMemberItem.Fields[Constants.Templates.TeamMember.FirstName].Value = firstName;
                teamMemberItem.Fields[Constants.Templates.TeamMember.LastName].Value = lastName;
                teamMemberItem.Fields[Constants.Templates.TeamMember.Email].Value = email;
                teamMemberItem.Fields[Constants.Templates.TeamMember.GithubId].Value = githubUsername;
                teamMemberItem.Editing.EndEdit();
                return teamMemberItem;
            }
        }

        /// <summary>
        /// Join Hackathon Team
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="githubUsername"></param>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public Sitecore.Data.Items.Item JoinHackathonTeam(string firstName, string lastName, string email, string githubUsername, string teamName)
        {

            using (new SecurityDisabler())
            {
                var teamItem = GetMatchingTeamItemWithSlug(teamName);
                if (teamItem == null)
                {
                    return null;
                }
                var teamMember = GetMatchingTeamMemberItemWithGithubUsername(githubUsername);
                if (teamMember == null)
                {
                    teamMember = CreateHackathonTeamMember(firstName, lastName, email, githubUsername);
                }

                if (teamMember != null)
                {
                    _service.AddMemberToRepoRepo(teamItem[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.RepoName], githubUsername);
                    teamItem.Editing.BeginEdit();
                    Sitecore.Data.Fields.MultilistField members = (Sitecore.Data.Fields.MultilistField)teamItem.Fields[Constants.Templates.TeamGithubData.Members];
                    members.Add(teamMember.ID.ToString());
                    teamItem.Editing.EndEdit();
                }
                return teamMember;
            }
        }

        /// <summary>
        /// Get Matching team using slug
        /// </summary>
        /// <param name="teamSlug"></param>
        /// <returns></returns>
        public Team GetMatchingTeamWithSlug(string teamSlug)
        {
            var matchingTeam = GetMatchingTeamItemWithSlug(teamSlug);
            if (matchingTeam != null)
            {
                return GetTeamFromItem(matchingTeam);
            }

            return null; 
        }

        /// <summary>
        /// Get matching team
        /// </summary>
        /// <param name="teamSlug"></param>
        /// <returns></returns>
        public Sitecore.Data.Items.Item GetMatchingTeamItemWithSlug(string teamSlug)
        {
            // iterate through the different teams 
            var teamFolderPath = Settings.GetSetting(Constants.Settings.TeamsFolder);
            var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
            var teamsfolder = masterDb.GetItem(teamFolderPath);
            if (teamsfolder != null)
            {
                var matchingTeam = teamsfolder.Children.FirstOrDefault(i => !string.IsNullOrEmpty(i[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.Slug]) &&
                    i[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.Slug].ToLower() == teamSlug.ToLower());

                return matchingTeam;
            }

            return null;
        }

        public Sitecore.Data.Items.Item GetMatchingTeamMemberItemWithGithubUsername(string username)
        {
            // iterate through the different teams 
            var teamMembersFolderPath = Settings.GetSetting(Constants.Settings.TeamMembersFolder);
            var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
            var teamMembersfolder = masterDb.GetItem(teamMembersFolderPath);
            if (teamMembersfolder != null)
            {
                var matchingTeamMember = teamMembersfolder.Children.FirstOrDefault(i => !string.IsNullOrEmpty(i[Hackathon.Foundation.Teams.Constants.Templates.TeamMember.GithubId]) &&
                    i[Hackathon.Foundation.Teams.Constants.Templates.TeamMember.GithubId].ToLower() == username.ToLower());

                return matchingTeamMember;
            }

            return null;
        }

        public Item GetMyTeam(string githubUsername)
        {
            var myMemberItem = GetMatchingTeamMemberItemWithGithubUsername(githubUsername);
            if(myMemberItem!=null)
            {
                var teamFolderPath = Settings.GetSetting(Constants.Settings.TeamsFolder);
                var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                var teamsfolder = masterDb.GetItem(teamFolderPath);
                var matchingTeam = teamsfolder.Children.FirstOrDefault(i => i.IsMyTeam(myMemberItem));
                return matchingTeam;
            }
            return null;
        }

        

        public Team GetTeamFromItem(Item item)
        {
            return new Team()
            {
                Name = item[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.Name],
                Id = item[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.Id],
                Slug = item[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.Slug],
                Description = item[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.Description],
                Url = item[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.Url],
            };
        }
    }
}