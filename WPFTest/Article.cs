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
            var matchs = Regex.Matches(result, shotContentReg);
            List<Article> list = new List<Article>();
            foreach(Match m in matchs)
            {
                string hrefs = Regex.Match(m.Value, hrefReg).Value;
                var realHrefs = Regex.Match(hrefs, realHrefReg).Value;
                var str = Regex.Replace(m.Value, "</p>", "\n");
                str = Regex.Replace(str, "</?[^>]+/?>","");
                str = Regex.Replace(str, "&nbsp;", " ");

                var heads = Regex.Match(result, headReg).Value;
                heads = Regex.Replace(heads, "</?[^>]+/?>", "");
                list.Add(new Article() { Content = str, Url = realHrefs, Head = heads });
            }
            return list;
        }

        public static Article CreateArticle(string url)
        {
            var result = client.GetStringAsync(new Uri(url, UriKind.Absolute)).Result;
            var match = Regex.Match(result, shotContentReg).Value;
            var heads = Regex.Match(result, headReg).Value;
            var imageurl = Regex.Match(match, imageReg).Value;
            var content = Regex.Match(match, contentReg).Value;
            content = Regex.Replace(content.Substring(0, content.IndexOf("<p>——————————————————————————</p>")), "</p>", "\n");
            content = Regex.Replace(content, "</?[^>]+/?>", "");
            content = Regex.Replace(content, "&nbsp;", " ");

            heads = Regex.Replace(heads, "</?[^>]+/?>", "");
            return new Article() { Content = content, ImageUrl = imageurl, Head = heads };
        }
        public string Author { get; set; }
        public string Hot { get; set; }
        public DateTime PublishTime { get; set; }
        public string Head { get; set; }
        public string ShortContent { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        private static HttpClient client = new HttpClient();
        private static string shortContentReg = "<div class=\"short\">[\\s\\S]*?<div class=\"shot\">[\\s\\S]*?</div>[\\s\\S]*?</div>";
        private static string shotContentReg = "<div class=\"shot\">[\\s\\S]*?</div>";
        private static string hrefReg = "<a href=\"http://meiwenrishang.com/post/(.*?) class=\"cont\">";
        private static string realHrefReg = @"http://meiwenrishang.com/post/[\w-/]*";
        private static string headReg = "<h2>.*</h2>";
        private static string imageReg = "http://.*?jpeg";
        private static string contentReg = "<div class=\"shot\">[\\s\\S]*?<p>——————————————————————————</p>";
    }
}
