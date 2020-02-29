using Hackathon.Foundation.Teams.Services;
using Sitecore.Configuration;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Foundation.Teams.Repositories
{
	public class TeamsRepository
	{
        private GithubService _service = new GithubService();
        public Sitecore.Data.Items.Item CreateHackathonTeam(string teamName, string teamDescription)
        {
            var teamsfolder = Sitecore.Context.Database.GetItem(Settings.GetSetting("hackathon.TeamsFolder"));
            var friendlyTeamName = Sitecore.Data.Items.ItemUtil.ProposeValidItemName(teamName);

            var response = _service.CreateOrganizationTeam(friendlyTeamName, teamDescription);
            var teamItem = teamsfolder.Add(friendlyTeamName, new TemplateID(new ID(Constants.Templates.TeamsTemplate)));
            teamItem.Editing.BeginEdit();
            teamItem.Fields["name"].Value = response.name;
            teamItem.Fields["id"].Value = response.id.ToString() ;
            teamItem.Fields["slug"].Value = response.slug;
            teamItem.Fields["description"].Value = response.description;
            teamItem.Fields["url"].Value = response.url;
            teamItem.Editing.EndEdit();

            return teamItem;

        }
	}
}