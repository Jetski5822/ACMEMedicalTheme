using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Orchard.Services;

namespace ACME.Theme.Medical.Services {
    public class ImageTemplateFilter : IHtmlFilter {
        private const string ImageStart = "<p>[image";
        private const string ImageStartReplaced = "[image";
        private const string ImageEndRegex = @"(<p>\[/image\])[\s+]{0,}(</p>)";

        // Can be moved somewhere else once we have IoC enabled body text filters.
        public string ProcessContent(string text, string flavor) {
            var regex = new Regex(GetShortcodeRegex("image"), RegexOptions.None);

            var matches = regex.Matches(text);

            if (matches.Count == 0)
                return text;

            var imgregex = new Regex(@"<\s*?img\s+[^>]*?\s*src\s*=\s*([""'])(((?>\\?).)*?)\1[^>]*?>", RegexOptions.None);

            var sb = new StringBuilder(text.Replace(ImageStart, ImageStartReplaced));

            foreach (Match myMatch in regex.Matches(text)) {
                if (myMatch.Success) {
                    Dictionary<string, string> attributes = 
                        myMatch
                        .Groups[3]
                        .Value
                        .Trim()
                        .Split(' ')
                        .Where(s => s != String.Empty)
                        .Select(x => x.Split('='))
                        .ToDictionary(x => x[0], x => x[1]);

                    var classes = new List<string>();

                    if (attributes.ContainsKey("fullsize") && attributes["fullsize"].Trim('\'').ToLowerInvariant() == "true") {
                        classes.Add("img-fullscreen");
                    }

                    //
                    var imgUrl = "#";
                    var imgMatchInternal = imgregex.Match(myMatch.Groups[5].Value);
                    if (imgMatchInternal.Success) {
                        imgUrl = imgMatchInternal.Groups[2].Value;
                    }

                    sb.Replace(string.Format("[{0}{1}]", myMatch.Groups[2], myMatch.Groups[3]),
                        string.Format("<a href='{1}'{0}>",
                        classes.Count > 0 ? " class='" + string.Join(" ", classes) + "'" : string.Empty,
                        imgUrl));

                    if (imgMatchInternal.Success) {
                        var value = myMatch.Groups[5].Value.Substring(4, myMatch.Groups[5].Value.Length - 4).Trim();
                        value = value.Substring(0, value.Length - 3).Trim();
                        value = value.Substring(0, value.Length - 4).Trim();

                        sb.Replace(myMatch.Groups[5].Value,
                            value +
                            "<span class='img-fullscreen-trigger'></span></p><p>");
                    }
                    else {
                        sb.Replace(myMatch.Groups[5].Value,
                            myMatch.Groups[5].Value.Substring(4, myMatch.Groups[5].Value.Length - 4));
                    }
                }
            }

            return Regex.Replace(sb.ToString(), ImageEndRegex, "</a>", RegexOptions.Multiline);
        }

        public string GetShortcodeRegex(string value) {
            return
                string.Format(
                    @"\[(\[?)({0})(?![\w-])([^\]\/]*(?:\/(?!\])[^\]\/]*)*?)(?:(\/)\]|\](?:((?>[^\[]*)(?>(?:\[(?!\/\2\])(?>[^\[]*))*))\[\/\2\])?)(\]?)", value);
        }
    }
}