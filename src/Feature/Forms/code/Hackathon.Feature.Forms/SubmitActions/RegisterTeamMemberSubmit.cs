using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using static Hackathon.Feature.Forms.Helper.SubmitActionHelper;
using Hackathon.Foundation.Teams.Repositories;
using System.Collections.Generic;
using Hackathon.Foundation.Account.Services;
using System;

namespace Hackathon.Feature.Forms.SubmitActions
{
    /// <summary>
    /// Executes a submit action for logging the form submit status.
    /// </summary>
    /// <seealso cref="Sitecore.ExperienceForms.Processing.Actions.SubmitActionBase{TParametersData}" />
    public class RegisterTeamMemberSubmit : SubmitActionBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSubmit"/> class.
        /// </summary>
        /// <param name="submitActionData">The submit action data.</param>
        public RegisterTeamMemberSubmit(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }


        public override void ExecuteAction(FormSubmitContext formSubmitContext, string parameters)
        {
            string tParametersDatum;
            Assert.ArgumentNotNull(formSubmitContext, "formSubmitContext");
            if (this.TryParse(parameters, out tParametersDatum))
            {
                try
                {
                    if (this.Execute(tParametersDatum, formSubmitContext))
                    {
                        return;
                    }
                }
                catch (ArgumentNullException argumentNullException)
                {
                }
            }
            formSubmitContext.Errors.Add(new FormActionError()
            {
                ErrorMessage = this.SubmitActionData.ErrorMessage
            });
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

            // create a new team using the data?

            return ExecuteMigrate(formSubmitContext.Fields); 
        }

        public bool ExecuteMigrate(IList<IViewModel> fields)
        {
            var firstName = fields.GetFieldValue("FirstName");
            var lastName = fields.GetFieldValue("LastName");
            var email = fields.GetFieldValue("Email");
            var password = fields.GetFieldValue("Password");
            //var country = fields.GetFieldValue("Country");
            var githubUser = fields.GetFieldValue("GithubUser");
            var twitterUser = fields.GetFieldValue("TwitterUser");
            var linkedInUser = fields.GetFieldValue("LinkedInUser");

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(email)
                && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(githubUser))
            {
                var loginUser = new LoginUser();
                var registerResponse = loginUser.Register(email, password, firstName, lastName, twitterUser, githubUser, linkedInUser);
                // make the sitecore user

                return registerResponse;
            } 

            return false; 
        }
    }
}