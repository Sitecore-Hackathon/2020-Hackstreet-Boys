using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Hackathon.Foundation.Teams.Connectors
{
    public class GithubConnector
    {
        private static HttpClient _httpClient = new HttpClient();

        private static string version => Sitecore.Configuration.Settings.GetSetting("Foundation.SalesforceConnector.ApiVersion");

        public static T ExecuteRequest<T>( HttpMethod method, string apiUrl, JObject contentObject , string expectedStatus)
        {
            var ghLog = new System.Text.StringBuilder();
            var watch = new System.Diagnostics.Stopwatch();

            ghLog.Append($"gitHubConnector:{method.ToString()}-{apiUrl}");


            HttpRequestMessage request = new HttpRequestMessage(method, "https://api.github.com" + apiUrl );
            request.Headers.Add("Authorization", $"token {Sitecore.Configuration.Settings.GetSetting("hackathon.GithubPersonalAccessKey")}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            request.Content = new StringContent(contentObject.ToString(), Encoding.UTF8, "application/json");

            watch.Start();
            using (HttpResponseMessage response = _httpClient.SendAsync(request).Result)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;

                ghLog.Append($" [{watch.ElapsedMilliseconds} ms]");

                if (!response.Headers.GetValues("Status").Contains(expectedStatus))
                {
                   
                    throw new Exception($"Unexpected status result {response.Headers.GetValues("Status")}");
                }

                ghLog.Append(" SUCCESS " + response.StatusCode);
                Sitecore.Diagnostics.Log.Info(ghLog.ToString(), typeof(GithubConnector));

                return JsonConvert.DeserializeObject<T>(responseBody);
            }

        }

    }
}

