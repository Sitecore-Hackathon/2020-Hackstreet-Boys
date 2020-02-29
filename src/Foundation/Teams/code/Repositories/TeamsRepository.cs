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

                var teamItem = teamsfolder.Add(friendlyTeamName, new TemplateID(new ID(Constants.Templates.Team.TemplateId)));
                
                teamItem.Editing.BeginEdit();
                teamItem.Fields[Constants.Templates.Team.Name].Value = teamResponse.name;
                teamItem.Fields[Constants.Templates.Team.Id].Value = teamResponse.id.ToString();
                teamItem.Fields[Constants.Templates.Team.Slug].Value = teamResponse.slug;
                teamItem.Fields[Constants.Templates.Team.Description].Value = teamResponse.description;
                teamItem.Fields[Constants.Templates.Team.Url].Value = teamResponse.html_url;
                teamItem.Fields[Constants.Templates.Team.RepoName].Value = repoResponse.name;
                teamItem.Fields[Constants.Templates.Team.RepoFullName].Value = repoResponse.full_name;
                teamItem.Fields[Constants.Templates.Team.RepoUrl].Value = repoResponse.html_url;
                teamItem.Editing.EndEdit();

                if (Sitecore.Context.User.IsAuthenticated)
                {
                    var teamMember = CreateHackathonTeamMember(Sitecore.Context.User.Profile.FullName, "", Sitecore.Context.User.Profile.Email, githubUsername);
                    teamItem.Editing.BeginEdit();
                    Sitecore.Data.Fields.MultilistField members = (Sitecore.Data.Fields.MultilistField)teamItem.Fields[Constants.Templates.Team.Members];
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

                var teamMemberItem = teamMembersfolder.Add(friendlyTeamName, new TemplateID(new ID(Constants.Templates.TeamMember.TemplateId)));
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
                    _service.AddMemberToRepoRepo(teamItem[Hackathon.Foundation.Teams.Constants.Templates.Team.RepoName], githubUsername);
                    teamItem.Editing.BeginEdit();
                    Sitecore.Data.Fields.MultilistField members = (Sitecore.Data.Fields.MultilistField)teamItem.Fields[Constants.Templates.Team.Members];
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
                var matchingTeam = teamsfolder.Axes.GetDescendants().FirstOrDefault(i => !string.IsNullOrEmpty(i[Hackathon.Foundation.Teams.Constants.Templates.Team.Slug]) &&
                    i[Hackathon.Foundation.Teams.Constants.Templates.Team.Slug].ToLower() == teamSlug.ToLower());

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
                var matchingTeamMember = teamMembersfolder.Axes.GetDescendants().FirstOrDefault(i => !string.IsNullOrEmpty(i[Hackathon.Foundation.Teams.Constants.Templates.TeamMember.GithubId]) &&
                    i[Hackathon.Foundation.Teams.Constants.Templates.TeamMember.GithubId].ToLower() == username.ToLower());

                return matchingTeamMember;
            }

            return null;
        }

        public int GetTeamMemberCount()
        {
            var teamMembersFolderPath = Settings.GetSetting(Constants.Settings.TeamMembersFolder);
            var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
            var teamMembersfolder = masterDb.GetItem(teamMembersFolderPath);
            if (teamMembersfolder != null)
            {
                return teamMembersfolder.Axes.GetDescendants().Count(i => i.TemplateID.ToString() == Constants.Templates.TeamMember.TemplateId);
            }
            return 0;
        }

        public int GetTeamsCount()
        {
            var teamFolderPath = Settings.GetSetting(Constants.Settings.TeamsFolder);
            var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
            var teamsfolder = masterDb.GetItem(teamFolderPath);
            if (teamsfolder != null)
            {
                return teamsfolder.Axes.GetDescendants().Count(i => i.TemplateID.ToString() == Constants.Templates.Team.TemplateId);
            }
            return 0; 
        }

        public Item GetMyTeam(string githubUsername)
        {
            var myMemberItem = GetMatchingTeamMemberItemWithGithubUsername(githubUsername);
            if(myMemberItem!=null)
            {
                var teamFolderPath = Settings.GetSetting(Constants.Settings.TeamsFolder);
                var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                var teamsfolder = masterDb.GetItem(teamFolderPath);
                var matchingTeam = teamsfolder.Axes.GetDescendants().FirstOrDefault(i => i.IsMyTeam(myMemberItem));
                return matchingTeam;
            }
            return null;
        }

        public Team GetTeamFromItem(Item item)
        {
            return new Team()
            {
                Name = item[Hackathon.Foundation.Teams.Constants.Templates.Team.Name],
                Id = item[Hackathon.Foundation.Teams.Constants.Templates.Team.Id],
                Slug = item[Hackathon.Foundation.Teams.Constants.Templates.Team.Slug],
                Description = item[Hackathon.Foundation.Teams.Constants.Templates.Team.Description],
                Url = item[Hackathon.Foundation.Teams.Constants.Templates.Team.Url],
            };
        }

        public TeamMember GetTeamMemberFromItem(Item item)
        {
            return new TeamMember()
            {
                FirstName = item[Constants.Templates.TeamMember.FirstName],
                LastName = item[Constants.Templates.TeamMember.LastName],
                Email = item[Constants.Templates.TeamMember.Email],
                GithubId = item[Constants.Templates.TeamMember.GithubId]
            };
        }
    }
}