using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Orchard.Services;

namespace ACME.Theme.Medical.Services {
    public class BoxTemplateFilter : IHtmlFilter {
        private const string BoxStart = "<p>[box";
        private const string BoxStartReplaced = "[box";
        private const string BoxEndRegex = @"(<p>\[/box\])[\s+]{0,}(</p>)";

        public string ProcessContent(string text, string flavor) {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var regex = new Regex(GetShortcodeRegex("box"), RegexOptions.None);

            var matches = regex.Matches(text);

            if (matches.Count == 0)
                return text;

            var sb = new StringBuilder(text.Replace(BoxStart, BoxStartReplaced));

            foreach (Match myMatch in matches) {
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

                    var classes = new List<string> {"container-box"};

                    if (!attributes.ContainsKey("area")) {
                        classes.Add("container-fluid");
                    }
                    else {
                        var area = attributes["area"].Trim('\'').ToLowerInvariant();

                        if (area == "full")
                            classes.Add("container-fluid");
                        else
                            classes.Add("container");

                        if (area == "right")
                            classes.Add("pull-right");
                        if (area == "left")
                            classes.Add("pull-left");
                    }

                    if (!attributes.ContainsKey("colour")) {
                        classes.Add("container-box-style-default");
                    }
                    else {
                        var colour = attributes["colour"].Trim('\'').ToLowerInvariant();

                        classes.Add("container-box-style-" + colour);
                    }

                    sb.Replace(string.Format("[{0}{1}]", myMatch.Groups[2], myMatch.Groups[3]),
                        string.Format("<div{0}>",
                        classes.Count > 0 ? " class='" + string.Join(" ", classes) + "'" : string.Empty));

                    sb.Replace(myMatch.Groups[5].Value,
                        myMatch.Groups[5].Value.Substring(4, myMatch.Groups[5].Value.Length - 4));
                }
            }

            return Regex.Replace(sb.ToString(), BoxEndRegex, "</div>", RegexOptions.Multiline);
        }

        public string GetShortcodeRegex(string value) {
            return
                string.Format(
                    @"\[(\[?)({0})(?![\w-])([^\]\/]*(?:\/(?!\])[^\]\/]*)*?)(?:(\/)\]|\](?:((?>[^\[]*)(?>(?:\[(?!\/\2\])(?>[^\[]*))*))\[\/\2\])?)(\]?)", value);
        }
    }
}