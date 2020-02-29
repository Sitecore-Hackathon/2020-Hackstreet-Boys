using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using static Hackathon.Feature.Forms.Helper.SubmitActionHelper;
using Hackathon.Foundation.Teams.Repositories;
using System.Collections.Generic;
using Hackathon.Foundation.Account.Services;

namespace Hackathon.Feature.Forms.SubmitActions
{
    /// <summary>
    /// Executes a submit action for logging the form submit status.
    /// </summary>
    /// <seealso cref="Sitecore.ExperienceForms.Processing.Actions.SubmitActionBase{TParametersData}" />
    public class LoginTeamMemberSubmit : SubmitActionBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSubmit"/> class.
        /// </summary>
        /// <param name="submitActionData">The submit action data.</param>
        public LoginTeamMemberSubmit(ISubmitActionData submitActionData) : base(submitActionData)
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

            // create a new team using the data?
            
            return Execute(formSubmitContext.Fields); 
        }


        public bool Execute(IList<IViewModel> fields)
        {
            var email = fields.GetFieldValue("Email");
            var password = fields.GetFieldValue("Password");

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var loginUser = new LoginUser();
                return loginUser.Login(email, password); 
            } 

            return false;
        }
    }
}