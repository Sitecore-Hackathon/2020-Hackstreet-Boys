using System.Collections.Generic;
using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using static Hackathon.Feature.Forms.Helper.SubmitActionHelper;
using Hackathon.Foundation.Teams.Repositories;
using Hackathon.Foundation.Account.Services;

namespace Hackathon.Feature.Forms.SubmitActions
{
    /// <summary>
    /// Executes a submit action for logging the form submit status.
    /// </summary>
    /// <seealso cref="Sitecore.ExperienceForms.Processing.Actions.SubmitActionBase{TParametersData}" />
    public class CreateOrganizationTeamSubmit : SubmitActionBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSubmit"/> class.
        /// </summary>
        /// <param name="submitActionData">The submit action data.</param>
        public CreateOrganizationTeamSubmit(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

            
            return Execute(formSubmitContext.Fields);
        }

        public bool Execute(IList<IViewModel> fields)
        {
            var teamName = fields.GetFieldValue("TeamName");
            var teamDescription = fields.GetFieldValue("TeamDescription");
            var loginUser = new LoginUser();
            var githubUsername = loginUser.GetCurrentUserGithub();
            if (!string.IsNullOrEmpty(teamName) && !string.IsNullOrEmpty(teamDescription) && !string.IsNullOrEmpty(githubUsername))
            {
                var teamsRepo = new TeamsRepository();
                
                var newTeamItem = teamsRepo.CreateHackathonTeam(teamName, teamDescription, githubUsername);

                if (newTeamItem != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}