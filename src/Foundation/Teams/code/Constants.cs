using Sitecore.Data;

namespace Hackathon.Foundation.Teams
{
    public static class Constants
    {
        public static class Templates
        {
            public static class TeamGithubData
            {
                public static string TemplateId = "{DD2A56A7-613E-44C0-9672-A6DC4EEB7BEB}";

                public static string Name = "name";
                public static string Id = "id";
                public static string Slug = "slug";
                public static string Description = "description";
                public static string Url = "url";
            }

            public static string TeamsTemplate = "{CD253AE6-D649-4AE8-A875-AF4ADDD42F6C}";
            public static string TeamsMemberTemplate = "{9A6802D0-3C3F-41C3-8198-B836ACA27CFC}";
        }

        public static class Settings
        {
            public static string TeamsFolder = "hackathon.TeamsFolder";
        }
    }
}