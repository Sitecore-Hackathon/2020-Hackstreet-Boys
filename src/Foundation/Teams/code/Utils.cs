using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Configuration;
using System.Web;
using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Sites;
using System.Linq;

namespace Hackathon.Foundation.Teams
{
    public static class Utils
    {
        public static bool IsMyTeam(this Item teamItem, Item memberItem)
        {
            Sitecore.Data.Fields.MultilistField members = (Sitecore.Data.Fields.MultilistField)teamItem.Fields[Constants.Templates.TeamGithubData.Members];
            if (members != null && members.TargetIDs.Contains(memberItem.ID))
            {
                return true;
            }
            return false;
        }

    }
}
