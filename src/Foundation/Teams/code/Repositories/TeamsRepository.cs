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

namespace Hackathon.Foundation.Teams.Repositories
{
	public class TeamsRepository
	{
        private GithubService _service = new GithubService();
        public Sitecore.Data.Items.Item CreateHackathonTeam(string teamName, string githubUsername, string teamDescription)
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
                teamItem.Fields[Constants.Templates.TeamGithubData.RepoName].Value = repoResponse.full_name;
                teamItem.Fields[Constants.Templates.TeamGithubData.RepoUrl].Value = repoResponse.html_url;
                teamItem.Editing.EndEdit();

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

        public Team GetMatchingTeamWithSlug(string teamSlug)
        {
            // iterate through the different teams 
            var teamFolderPath = Settings.GetSetting(Constants.Settings.TeamsFolder);
            var teamsfolder = Sitecore.Context.Database.GetItem(teamFolderPath);
            if (teamsfolder != null)
            {
                var matchingTeam = teamsfolder.Children.FirstOrDefault(i => !string.IsNullOrEmpty(i[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.Slug]) &&
                    i[Hackathon.Foundation.Teams.Constants.Templates.TeamGithubData.Slug] == teamSlug);

                if (matchingTeam != null)
                {
                    return GetTeamFromItem(matchingTeam); 
                }
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