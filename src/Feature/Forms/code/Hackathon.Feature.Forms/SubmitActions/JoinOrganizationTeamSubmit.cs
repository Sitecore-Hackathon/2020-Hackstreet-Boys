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
            
            Execute(formSubmitContext.Fields);
            return true;
        }

        public void Execute(IList<IViewModel> fields)
        {
            var teamJoinCode = fields.GetFieldValue("TeamJoinCode"); 

            if (!string.IsNullOrEmpty(teamJoinCode))
            {
                var teamsRepo = new TeamsRepository();
                var githubUsername = "kvn-prhn";

                //var newTeamItem = teamsRepo.Joi // TODO: join team
            }
            /*
            var username = _user.CurrentProfile?.ProfileUser?.LocalName;
            if (string.IsNullOrWhiteSpace(username))
                throw new System.UnauthorizedAccessException("Please log-in to change your password");

            var newPassword = fields.GetFieldValue("New Password");
            Assert.ArgumentNotNull(newPassword, "You should fill in the 'New Password' field.");

            var oldPassword = fields.GetFieldValue("Old Password");
            Assert.ArgumentNotNull(oldPassword, "You should fill in the 'Old Password' field.");

            var response = _data.ChangePassword(username, newPassword, oldPassword);
            if (response.ResponseCode != WebServices.SDK.Abstractions.Models.Api.ResponseCode.Success)
                throw new Exception(response.Message);
                */
        }
    }
}