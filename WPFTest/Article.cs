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
            var headMatchs = Regex.Matches(result, headReg);
            var timeMatchs = Regex.Matches(result, timeTagReg);
            List<Article> list = new List<Article>();
            for (int i = 0; i < matchs.Count; i++)
            {
                if (i == 0) continue; //过滤掉对应的广告
                Match m = matchs[i];
                string hrefs = Regex.Match(m.Value, hrefReg).Value;
                var realHrefs = Regex.Match(hrefs, realHrefReg).Value;
                var str = Regex.Replace(m.Value, "</p>", "\n");
                str = Regex.Replace(str, "</?[^>]+/?>", "");
                str = Regex.Replace(str, "&nbsp;", " ");
                str = Regex.Replace(str, "继续阅读", " ");
                str = str.TrimStart(new char[] { ' ', '\n' }).TrimEnd(new char[] { ' ', '\n' });
                var heads = Regex.Replace(headMatchs[i].Value, "</?[^>]+/?>", "");
                var time = Regex.Replace(timeMatchs[i].Value, "</?[^>]+/?>", "");
                list.Add(new Article() { Content = str, Url = realHrefs, Head = heads, Time = time });
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
        public string Time { get; set; }
        private static HttpClient client = new HttpClient();
        private static string shortContentReg = "<div class=\"short\">[\\s\\S]*?<div class=\"shot\">[\\s\\S]*?</div>[\\s\\S]*?</div>";
        private static string shotContentReg = "<div class=\"shot\">[\\s\\S]*?</div>";
        private static string hrefReg = "<a href=\"http://meiwenrishang.com/post/(.*?) class=\"cont\">";
        private static string realHrefReg = @"http://meiwenrishang.com/post/[\w-/]*";
        private static string headReg = "<h2>.*</h2>";
        private static string imageReg = "http://([\\s\\S]*?)jpeg";
        private static string contentReg = "<div class=\"shot\">[\\s\\S]*?<p>——————————————————————————</p>";
        private static string timeTagReg = "<strong id=\"timebg\">.*?</strong>";
    }
}
