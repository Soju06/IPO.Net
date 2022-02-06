using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IPO.Net.utility {
    internal static class HtmlUtility {
        public static HtmlDocument Html(this Stream stream) {
            var doc = new HtmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static async Task<HtmlDocument> HtmlAsync(this HttpContent content, Encoding encoding) {
            var doc = new HtmlDocument();
            doc.Load(await content.ReadAsStreamAsync(), encoding);
            return doc;
        }

        public static string HtmlDecode(this string input) => System.Web.HttpUtility.HtmlDecode(input);

        public static HtmlNode[] GetElements(this HtmlNode value) =>
            value.ChildNodes.Where(elem => elem.NodeType == HtmlNodeType.Element).ToArray();
    }
}
