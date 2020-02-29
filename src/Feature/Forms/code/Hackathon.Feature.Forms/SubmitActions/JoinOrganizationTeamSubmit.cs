using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using System;
using System.Collections.Generic;
using System.Linq; 
using static Hackathon.Feature.Forms.Helper.SubmitActionHelper;
using Hackathon.Foundation.Teams.Repositories;
using Hackathon.Foundation.Account.Services;

namespace Hackathon.Feature.Forms.SubmitActions
{
    /// <summary>
    /// Executes a submit action for logging the form submit status.
    /// </summary>
    /// <seealso cref="Sitecore.ExperienceForms.Processing.Actions.SubmitActionBase{TParametersData}" />
    public class JoinOrganizationTeamSubmit : SubmitActionBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSubmit"/> class.
        /// </summary>
        /// <param name="submitActionData">The submit action data.</param>
        public JoinOrganizationTeamSubmit(ISubmitActionData submitActionData) : base(submitActionData)
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
            var teamJoinCode = fields.GetFieldValue("TeamJoinCode");
            var loginUser = new LoginUser();
            var loggedInProfile = loginUser.GetCurrentUserProfile();
            var githubUsername = loginUser.GetCurrentUserGithub();
            var country = loginUser.GetCurrentUserCountry();

            if (!string.IsNullOrEmpty(teamJoinCode) && !string.IsNullOrEmpty(githubUsername) && !string.IsNullOrEmpty(country))
            {
                var teamsRepo = new TeamsRepository();

                var teamMember = teamsRepo.JoinHackathonTeam(loggedInProfile.FullName, "", loggedInProfile.Email, githubUsername, teamJoinCode); 

                if (teamMember != null) { 
                    return true;
                }
            }

            return false;
        }
    }
}