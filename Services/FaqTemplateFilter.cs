using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Orchard.Services;

namespace ACME.Theme.Medical.Services {
    public class FaqTemplateFilter : IHtmlFilter {
        private const string FAQGroupStart = "<p>[faq_group";
        private const string FAQGroupStartReplaced = "[faq_group";
        private const string FAQGroupEndRegex = @"(<p>\[/faq_group\])[\s+]{0,}(</p>)";

        private const string FAQStart = "<p>[faq";
        private const string FAQStartReplaced = "[faq";
        private const string FAQEndRegex = @"(<p>\[/faq\])[\s+]{0,}(</p>)";

        private static readonly Regex attributeRegex = new Regex(@"\w+='(.*?)'");

        public string ProcessContent(string text, string flavor) {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text.Replace(FAQGroupStart, FAQGroupStartReplaced), FAQGroupEndRegex, "[/faq_group]", RegexOptions.Multiline);

            text = Regex.Replace(text.Replace(FAQStart, FAQStartReplaced), FAQEndRegex, "[/faq]", RegexOptions.Multiline);

            var regex = new Regex(GetShortcodeRegex("faq_group"), RegexOptions.None);

            var sb = new StringBuilder(text);

            var index = 0;
            var match = regex.Match(text);
            while (match.Success) 
            {
                var value = match.Value;
                // Do FAQ Group

                var sbToManipulate = FormatFAQGroup(match, index);
                FormatFAQ(index, sbToManipulate);

                sb = sb.Replace(
                    sb.ToString().Substring(match.Index, match.Length), 
                    sbToManipulate.ToString(),
                    match.Index, 
                    match.Length);
                match = regex.Match(sb.ToString());
                index++;
            }

            return sb.ToString();
        }

        private StringBuilder FormatFAQGroup(Match match, int index) {
            var attributes = GetAttributes(match.Groups[3].Value);

            var classes = new List<string> { "container-faq" };

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
                classes.Add("container-faq-style-default");
            }
            else {
                var colour = attributes["colour"].Trim('\'').ToLowerInvariant();

                classes.Add("container-faq-style-" + colour);
            }

            var centralText = match.Groups[5].Value.Remove(0, 4);

            var formattedTitle = string.Empty;

            if (attributes.ContainsKey("title")) {
                var title = attributes["title"].Trim('\'');
                formattedTitle = title.Length >= 1 ? string.Format("<header><h1>{0}</h1></header>", title) : string.Empty;
            }

            var newValue =
                string.Format(
                    "<article{0} id='faq_{1}'>" +
                        "{2}" +
                        "{3}" +
                    "</article>",
                    classes.Count > 0 ? " class='" + string.Join(" ", classes) + "'" : string.Empty,
                    index,
                    formattedTitle,
                    centralText);

            return new StringBuilder(newValue);
        }

        private void FormatFAQ(int groupIndex, StringBuilder sb) {
            var regex = new Regex(GetShortcodeRegex("faq"), RegexOptions.None);

            var index = 0;
            var match = regex.Match(sb.ToString());
            while (match.Success) {
                var attributes = GetAttributes(match.Groups[3].Value);

                var newValue =
                        string.Format(
                        "<p><a href='#faq_{0}_question_{1}' class='faq-question'>{2}</a></p>" + 
                        "<div id='faq_{0}_question_{1}' class='hidden'>" +
                            "{3}" +
                        "</div>",
                        groupIndex,
                        index,
                        attributes["title"].Trim('\''),
                        match.Groups[5].Value.Remove(0, 4));

                sb = sb.Replace(sb.ToString().Substring(match.Index, match.Length), newValue, match.Index, match.Length);
                match = regex.Match(sb.ToString());
                index++;
            }
        }

        private Dictionary<string, string> GetAttributes(string value) {
            value = value.Trim();

            Dictionary<string, string> attributes = attributeRegex
                .Matches(value)
                .Cast<Match>()
                .Select(x => x.Value.Split('='))
                .ToDictionary(x => x[0], x => x[1]);

            if (!attributes.Any()) {
                var split = value.Split('=');
                if (split.Length == 2) {
                    attributes.Add(split[0], split[1].Trim('\''));
                }
            }

            return attributes;
        }

        public string GetShortcodeRegex(string value) {
            return
                string.Format(
                    @"\[(\[?)({0})(?![\w-])([^\]\/]*(?:\/(?!\])[^\]\/]*)*?)(?:(\/)\]|\](?:((?>[^\[]*)(?>(?:\[(?!\/\2\])(?>[^\[]*))*))\[\/\2\])?)(\]?)", value);
        }
    }
}