using Sitecore.Data;

namespace Hackathon.Foundation.Teams
{
    public static class Constants
    {
        public static class Templates
        {
            public static class Team
            {
                public static string TemplateId = "{CD253AE6-D649-4AE8-A875-AF4ADDD42F6C}";

                public static string Name = "name";
                public static string Id = "id";
                public static string Slug = "slug";
                public static string Description = "description";
                public static string Url = "url";
                public static string RepoName = "repo-name";
                public static string RepoFullName = "Repo Full Name";
                public static string RepoUrl = "repo-url";
                public static string Members = "Members";
            }
            public static class TeamMember
            {
                public static string TemplateId = "{9A6802D0-3C3F-41C3-8198-B836ACA27CFC}";

                public static string FirstName = "First Name";
                public static string LastName = "Last Name";
                public static string Email = "Email";
                public static string GithubId = "Github Id";
            }
        }

        public static class Settings
        {
            public static string TeamsFolder = "hackathon.TeamsFolder";
            public static string TeamMembersFolder = "hackathon.TeamMembersFolder";
        }
    }
}