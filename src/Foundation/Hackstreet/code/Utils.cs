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

namespace Hackathon.Foundation.Hackstreet.Client
{
    public class Utils
    {
        public static string StubLinkField(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            return $"<link linktype=\"anchor\" text=\"{HttpUtility.HtmlEncode(input)}\" url=\"\" anchor=\"\" />";
        }


        public static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;
            return HttpUtility.HtmlDecode(Regex.Replace(html, @"<[^>]+?>", "").Trim());
        }

        public static string SeoUrl(string s)
        {
            return SeoName(s).Replace("-/","/").Replace("/-","/");
        }

        public static string SeoName(string s)
        {
            var seoName = FriendlyUrl(s);
            if (string.IsNullOrWhiteSpace(seoName))
                return "invalid-name";

            // seoName = ItemUtil.ProposeValidItemName(seoName);
            var max = Settings.MaxItemNameLength;
            if (seoName.Length > max)
            {
                seoName = seoName.Substring(0, max).TrimEnd('-');
            }
            return seoName.ToLowerInvariant();
        }



        public static string NameToTitle(string seoName)
        {
            if (string.IsNullOrWhiteSpace(seoName))
                return string.Empty;

            var title = seoName.Replace(".aspx",string.Empty).Replace("-", " ");
            var myTi = new CultureInfo("en-US", false).TextInfo;
            return myTi.ToTitleCase(title);
        }

        public static string GetNameFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            var name = url.Trim(' ');
            
            if (url.Contains("/"))
            {
                var endPos = name.IndexOf('?');
                if (endPos == -1)
                    endPos = name.Length - 1;
                var pos = url.LastIndexOf('/', endPos);
                if (pos != -1)
                    name = url.Substring(pos + 1);
            }

            if (string.IsNullOrWhiteSpace(name))
                name = "home";

            return name;
        }

        /// <summary>
        /// Root is commonly Settings.ImportDestination
        /// </summary>
        /// <param name="gcId"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public static Item GetItemByGcId(int gcId, Item root)
        {
            var query =$"fast:{EscapeItemNamesWithDashes(root.Paths.FullPath).TrimEnd('/')}//*[@#GatherContent Id#=\"{gcId}\"]";

            return root.Database.SelectSingleItem(query);
        }

        public static string EscapeItemNamesWithDashes(string queryPath)
        {
            if (!queryPath.Contains("-"))
                return queryPath;

            var strArray = queryPath.Split(new char[] { '/' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].IndexOf('-') > 0)
                    strArray[i] = "#" + strArray[i] + "#";
            }
            return string.Join("/", strArray);
        }



        public static SiteContext GetSiteContextByItem(Item item)
        {
            if (item == null)
                return null;
            var siteName = GetSiteNameByItemPath(item.Paths.FullPath);
            if (string.IsNullOrWhiteSpace(siteName))
                return null;
            return SiteContextFactory.GetSiteContext(siteName);
        }
        public static string GetSiteNameByItem(Item item)
        {
            if (item == null)
                return null;
            return GetSiteNameByItemPath(item.Paths.FullPath);
        }
        public static string GetSiteNameByItemPath(string itemPath)
        {
            if (string.IsNullOrWhiteSpace(itemPath))
                return null;

            itemPath = StringUtil.EnsurePrefix('/', itemPath);
            itemPath = StringUtil.EnsurePostfix('/', itemPath);
            itemPath = itemPath.ToUpperInvariant();

            var siteFactory = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<Sitecore.Abstractions.BaseSiteContextFactory>();
            var sites = siteFactory.GetSites();
            foreach (var site in sites)
            {
                // skip system sites
                if (SystemSites.IndexOf(site.Name.ToLowerInvariant()) >= 0)
                    continue;

                // match site
                var sitePath = site.RootPath.ToUpperInvariant();
                sitePath = StringUtil.EnsurePrefix('/', sitePath);
                sitePath = StringUtil.EnsurePostfix('/', sitePath);

                if (itemPath.StartsWith(sitePath))
                    return site.Name;
            }

            return null;
        }

        public static readonly IList<string> SystemSites = new List<string> { "shell", "login", "admin", "service", "modules_shell", "modules_website", "scheduler", "system", "publisher", "system_layouts" };

        public static string FriendlyUrl(string title)
        {
            if (title == null) return "";

            const int maxlen = 200;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    // sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

    }
}
