using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Feature.PageContent
{
    public class Constants
    {

        public struct Templates
        {
            public struct FullHero
            {
                public const string TemplateId = "{7E8BAB72-0746-45E1-9152-12E2FDAC3779}";

                public const string MainText = "Main Text";
                public const string SubText = "Sub Text";
                public const string Image = "Image";
            }

            public struct RichTextContent
            {
                public const string TemplateId = "{D7BECCE1-0DB0-498C-95E3-4AD0CB2EA10F}";

                public const string TextContent = "Text Content";
            }

            public struct Team
            {
                public const string TemplateId = "{5E67BE75-BDC1-495E-8BAE-75DF48D3B3DB}";

                public const string TeamName = "Team Name";
                public const string Thumbnail = "Thumbnail";
            }

            public struct TeamList
            {
                public const string TemplateId = "{4E1E7491-3140-4CD0-AAA4-F0E2E4CB2CE8}";

                public const string Title = "Title";
            }

            public struct TwoChoice
            {
                public const string TemplateId = "{EA160845-CD31-4DE2-A05A-1E0D8119F64C}";

                public const string Title = "Title";
                public const string Choice1Title = "Choice 1 Title";
                public const string Choice2Title = "Choice 2 Title";
            }

            public struct MenuLink
            {
                public const string TemplateId = "{08263923-7B27-4C12-8EB6-66FDF992C741}";

                public const string Label = "Label";
                public const string Link = "Link";
            }
        }
    }
}