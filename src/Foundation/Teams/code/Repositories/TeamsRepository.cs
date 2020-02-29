using Hackathon.Foundation.Teams.Models;
using Hackathon.Foundation.Teams.Services;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Configuration;

namespace Hackathon.Foundation.Teams.Repositories
{
	public class TeamsRepository
	{
        private GithubService _service = new GithubService();
        public Sitecore.Data.Items.Item CreateHackathonTeam(string teamName, string teamDescription)
        {
            var teamsfolder = Sitecore.Context.Database.GetItem(Settings.GetSetting(Constants.Settings.TeamsFolder));
            var friendlyTeamName = Sitecore.Data.Items.ItemUtil.ProposeValidItemName(teamName);

            var response = _service.CreateOrganizationTeam(friendlyTeamName, teamDescription);
            var teamItem = teamsfolder.Add(friendlyTeamName, new TemplateID(new ID(Constants.Templates.TeamsTemplate)));
            teamItem.Editing.BeginEdit();
            teamItem.Fields[Constants.Templates.TeamGithubData.Name].Value = response.name;
            teamItem.Fields[Constants.Templates.TeamGithubData.Id].Value = response.id.ToString() ;
            teamItem.Fields[Constants.Templates.TeamGithubData.Slug].Value = response.slug;
            teamItem.Fields[Constants.Templates.TeamGithubData.Description].Value = response.description;
            teamItem.Fields[Constants.Templates.TeamGithubData.Url].Value = response.url;
            teamItem.Editing.EndEdit();

            return teamItem;

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