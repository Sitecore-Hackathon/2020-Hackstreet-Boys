using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Hackathon.Feature.Account.Models
{
    public class UserInfo
    {
        //[DisplayName("Email")]
        [Required]
        [EmailAddress(ErrorMessageResourceName = "Email is required.", ErrorMessageResourceType = typeof(UserInfo))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[DisplayName("Password")]
        [Required(ErrorMessageResourceName = "Password is required.", ErrorMessageResourceType = typeof(UserInfo))]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "The password must have at least a lowercase letter, uppercase letter and a number.", ErrorMessageResourceType = typeof(UserInfo))]
        [StringLength(25, ErrorMessage = "The password must be at least 6 characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        public string Country { get; set; }

        public string TwitterHandle { get; set; }

        [Required(ErrorMessage = "Github Username is required")]
        public string GithubUsername { get; set; }

        public string LinkedInUsername { get; set; }

        public string ReturnUrl { get; set; }
        //public IEnumerable<FedAuthLoginButton> LoginButtons { get; set; }
    }
}