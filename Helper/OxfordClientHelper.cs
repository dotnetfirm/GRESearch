using System.IO;
using System.Net;
namespace GRESearch.Helper
{
    public class OxfordClientHelper
    {
        public string SearchWord(string url, string app_id, string app_key)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var content = string.Empty;
            request.Method = "GET";
            request.ContentType = "application/json";

            if (!string.IsNullOrEmpty(app_id))
            {
                request.Headers.Add("app_id:" + app_id);
            }
            if (!string.IsNullOrEmpty(app_key))
            {
                request.Headers.Add("app_key:" + app_key);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }

            return content;
        }
    }
}