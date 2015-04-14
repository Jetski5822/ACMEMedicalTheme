using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Orchard.Services;

namespace ACME.Theme.Medical.Services {
    public class CollapseTemplateFilter : IHtmlFilter {
        private const string CollapseGroupStart = "<p>[collapse_group";
        private const string CollapseGroupStartReplaced = "[collapse_group";
        private const string CollapseGroupEndRegex = @"(<p>\[/collapse_group\])[\s+]{0,}(</p>)";

        private const string CollapseStart = "<p>[collapse";
        private const string CollapseStartReplaced = "[collapse";
        private const string CollapseEndRegex = @"(<p>\[/collapse\])[\s+]{0,}(</p>)";

        private static readonly Regex attributeRegex = new Regex(@"\w+='(.*?)'");

        public string ProcessContent(string text, string flavor) {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text.Replace(CollapseGroupStart, CollapseGroupStartReplaced), CollapseGroupEndRegex, "[/collapse_group]", RegexOptions.Multiline);

            text = Regex.Replace(text.Replace(CollapseStart, CollapseStartReplaced), CollapseEndRegex, "[/collapse]", RegexOptions.Multiline);

            var regex = new Regex(GetShortcodeRegex("collapse_group"), RegexOptions.None);

            var sb = new StringBuilder(text);

            var index = 0;
            var match = regex.Match(text);
            while (match.Success) 
            {
                var value = match.Value;
                // Do FAQ Group

                var sbToManipulate = FormatCollapseGroup(match, index);

                var attributes = GetAttributes(match.Groups[3].Value);

                string indexToOpenByDefault = "0";
                if (attributes.ContainsKey("showindex")) {
                    indexToOpenByDefault = attributes["showindex"].Trim('\'');
                }

                FormatCollapse(index, indexToOpenByDefault, sbToManipulate);

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

        private StringBuilder FormatCollapseGroup(Match match, int index) {
            var classes = new List<string> { "panel-group" };

            var centralText = match.Groups[5].Value.Remove(0, 4);

            var newValue =
                string.Format(
                    "<div{0} id='accordion-{1}' role='tablist' aria-multiselectable='true'>" +
                        "{2}" +
                    "</div>",
                    classes.Count > 0 ? " class='" + string.Join(" ", classes) + "'" : string.Empty,
                    index,
                    centralText);

            return new StringBuilder(newValue);
        }

        private void FormatCollapse(int groupIndex, string indexToOpenByDefault, StringBuilder sb) {
            var regex = new Regex(GetShortcodeRegex("collapse"), RegexOptions.None);

            var index = 0;
            var match = regex.Match(sb.ToString());
            while (match.Success) {
                var attributes = GetAttributes(match.Groups[3].Value);

                bool expanded = indexToOpenByDefault == index.ToString(CultureInfo.InvariantCulture);
                string bodyCollapsedClass = expanded ? "collapse in" : "collapse";

                IList<string> headerClasses = new List<string>();
                if (!expanded)
                    headerClasses.Add("collapsed");

                var newValue =
                        string.Format(
                        "<div class=\"panel panel-default\">" +
                            "<div class=\"panel-heading{7}\" role=\"tab\" id=\"heading_{0}_{1}\">" +
                                "<h4 class=\"panel-title\">" +
                                    "<a data-toggle=\"collapse\" href=\"#collapse{0}_{1}\" aria-expanded=\"{5}\" aria-controls=\"collapse{0}_{1}\"{6}>" +
                                        "{2}" +
                                    "</a>" +
                                "</h4>" +
                            "</div>" +
                            "<div id=\"collapse{0}_{1}\" class=\"panel-collapse {4}\" role=\"tabpanel\" aria-expanded=\"{5}\" aria-labelledby=\"heading_{0}_{1}\">" +
                                "<div class=\"panel-body\">" +
                                "{3}" +
                                "</div>" +
                            "</div>" +
                        "</div>",
                        groupIndex,
                        index,
                        attributes["title"].Trim('\''),
                        match.Groups[5].Value.Remove(0, 4),
                        bodyCollapsedClass,
                        expanded.ToString().ToLowerInvariant(),
                        headerClasses.Count > 0 ? " class='" + string.Join(" ", headerClasses) + "'" : string.Empty,
                        expanded ? " active" : string.Empty);

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