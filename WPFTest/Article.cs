using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WPFTest
{
    class Article
    {
        private Article()
        {

        }
        /// <summary>
        /// 根据URL返回摘要
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IList<Article> CreateArticles(string url)
        {
            var result = client.GetStringAsync(new Uri(url, UriKind.Absolute)).Result;
            var matchs = regexShot.Matches(result);
            List<Article> list = new List<Article>();
            foreach(Match m in matchs)
            {
                var str = Regex.Replace(m.Value, "</p>", "\n");
                str = Regex.Replace(str, "</?[^>]+/?>","");
                str = Regex.Replace(str, "&nbsp;", " ");
                list.Add(new Article() { Content =str});
            }
            return list;
        }
        public string Author { get; set; }
        public string Hot { get; set; }
        public DateTime PublishTime { get; set; }
        public string Head { get; set; }
        public string ShortContent { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }

        private static HttpClient client = new HttpClient();
        private static string shortContent = @"<div class=""short"">[\s\S]*?<div class=""shot"">[\s\S]*?</div>[\s\S]*?</div>";
        private static Regex regex = new Regex(shortContent);
        private static string shotContent = "<div class=\"shot\">[\\s\\S]*?</div>";
        private static Regex regexShot = new Regex(shotContent);
    }
}
