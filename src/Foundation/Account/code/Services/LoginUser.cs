using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Hackathon.Foundation.Account.Services
{
    public class LoginUser
    {
        public bool Register(string email, string password, string firstName, string lastName, string twitterHandle, string githubUsername, string linkedInUsername)
        {
            //var isRegistered = false;

            if (AddUser(email, password, firstName, lastName, twitterHandle, githubUsername, linkedInUsername))
            {
                //isRegistered = true;
                return Login(email, password);
            }

            return false;
        }

        public bool Login(string email, string password)
        {
            var domain = Sitecore.Context.Domain;

            if (domain != null)
            {
                var accountName = domain.GetFullName(email);
                return AuthenticationManager.Login(accountName, password);
            }

            return false;
        }

        private bool AddUser(string email, string password, string firstName, string lastName, string twitterHandle, string githubUsername, string linkedInUsername)
        {
            string userName = email;
            var success = false;
            //userName = string.Format(@"{0}\{1}", domain, userName);
            //string newPassword = Membership.GeneratePassword(10, 3);
            try
            {
                if (!User.Exists(userName))
                {
                    var domain = Sitecore.Context.Domain;
                    
                   var accountName = domain.GetFullName(userName);
                   Membership.CreateUser(accountName, password, email);

                    // Edit the profile information
                    User user = User.FromName(userName, true);
                    Sitecore.Security.UserProfile userProfile = user.Profile;
                    userProfile.FullName = string.Format("{0} {1}", firstName, lastName);
                    //userProfile.Comment = comment;

                    // Assigning the user profile template
                    userProfile.SetPropertyValue("ProfileItemId", "{BBCCF1F4-A638-4FE3-9B2E-8E5D7D73AE9E}");
                    
                    userProfile.SetCustomProperty("Twitter Handle", twitterHandle != null ? twitterHandle : string.Empty);
                    userProfile.SetCustomProperty("Github Username", githubUsername);
                    userProfile.SetCustomProperty("LinkedIn Username", linkedInUsername != null ? linkedInUsername : string.Empty);
                    userProfile.Save();

                    success = true;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(string.Format("Error in Client.Project.Security.UserMaintenance (AddUser): Message: {0}; Source:{1}", ex.Message, ex.Source), this);
            }

            return success;
        }

        public string GetCurrentUserGithub()
        {
            var githubUsername = string.Empty;

            if (Sitecore.Context.User.IsAuthenticated)
            {
                githubUsername = Sitecore.Context.User.Profile.GetCustomProperty("Github Username");
            }

            return githubUsername;
        }

        public string GetCurrentUserCountry()
        {
            var country = string.Empty;

            if (Sitecore.Context.User.IsAuthenticated)
            {
                country = Sitecore.Context.User.Profile.GetCustomProperty("Country");
            }

            return country; 
        }

        public Sitecore.Security.UserProfile GetCurrentUserProfile()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {
                return Sitecore.Context.User.Profile; 
            }

            return null;
        }


        //private bool UsernameOrPasswordFieldIsNull(LoginUserFormFields field)
        //{
        //    Assert.ArgumentNotNull(field, nameof(field));
        //    return field.Username == null || field.Password == null;
        //}

        //private bool UsernameOrPasswordValueIsNull(LoginUserFieldValues values)
        //{
        //    Assert.ArgumentNotNull(values, nameof(values));
        //    return string.IsNullOrEmpty(values.Username) || string.IsNullOrEmpty(values.Password);
        //}

        //private bool AbortForm(FormSubmitContext formSubmitContext)
        //{
        //    formSubmitContext.Abort();
        //    return false;
        //}

        //internal class LoginUserFormFields
        //{
        //    public IViewModel Username { get; set; }
        //    public IViewModel Password { get; set; }

        //    public LoginUserFieldValues GetFieldValues()
        //    {
        //        return new LoginUserFieldValues
        //        {
        //            Username = FieldHelper.GetValue(Username),
        //            Password = FieldHelper.GetValue(Password)
        //        };
        //    }
        //}

        //internal class LoginUserFieldValues
        //{
        //    public string Username { get; set; }
        //    public string Password { get; set; }
        //}
    }
}