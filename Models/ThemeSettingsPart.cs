using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage.InfosetStorage;

namespace ACME.Theme.Medical.Models {
    public class ThemeSettingsPart : ContentPart {
        public ThemeSetting For(string culture) {
            return All.SingleOrDefault(x => x.Culture == culture) ?? new ThemeSetting{ Culture = culture };
        }

        public IEnumerable<ThemeSetting> All {
            get {
                var value = this.As<InfosetPart>().Get<ThemeSettingsPart>("All");
                if (value == null)
                    return Enumerable.Empty<ThemeSetting>();

                return JsonConvert.DeserializeObject<IEnumerable<ThemeSetting>>(value);
            }
            set {
                var serializedValue = JsonConvert.SerializeObject(value);
                this.As<InfosetPart>().Set<ThemeSettingsPart>("All", serializedValue); 
            }
        }

        public string Culture { get; set; }

        public string HeaderLogo { get; set; }
        public string HeaderLogoUrl { get; set; }

        public string FooterLogo { get; set; }
        public string FooterLogoUrl { get; set; }
        public string FooterAudienceMessage { get; set; }
        public string FooterCopyrightsMessage { get; set; }

        public bool DisplayPublishedDate {
            get { return Convert.ToBoolean(this.As<InfosetPart>().Get<ThemeSettingsPart>("DisplayPublishedDate")); }
            set { this.As<InfosetPart>().Set<ThemeSettingsPart>("DisplayPublishedDate", value.ToString()); }
        }

        public string PublishedDateText { get; set; }
        public string PublishedDateFormat { get; set; }
        public bool LowercasePublishedDate { get; set; }
        public string NonContentPublishedDateText { get; set; }
        public string NonContentReferenceNumber { get; set; }
        public string NonContentPreparationDateText { get; set; }

        public bool CookiebarEnabled {
            get { return Convert.ToBoolean(this.As<InfosetPart>().Get<ThemeSettingsPart>("CookiebarEnabled")); }
            set { this.As<InfosetPart>().Set<ThemeSettingsPart>("CookiebarEnabled", value.ToString()); }
        }

        public string CookiebarText { get; set; }
        public string CookiebarYesText { get; set; }
        public string CookiebarNoText { get; set; }

        public string MemberLogo { get; set; }

        public string ProviderLogo { get; set; }
    }

    public class ThemeSetting {
        public string Culture { get; set; }

        public string HeaderLogo { get; set; }
        public string HeaderLogoUrl { get; set; }

        public string FooterLogo { get; set; }
        public string FooterLogoUrl { get; set; }
        public string FooterAudienceMessage { get; set; }
        public string FooterCopyrightsMessage { get; set; }

        public bool DisplayPublishedDate { get; set; }
        public string PublishedDateText { get; set; }
        public string PublishedDateFormat { get; set; }
        public bool LowercasePublishedDate { get; set; }
        public string NonContentPublishedDateText { get; set; }
        public string NonContentReferenceNumber { get; set; }
        public string NonContentPreparationDateText { get; set; }

        public bool CookiebarEnabled { get; set; }
        public string CookiebarText { get; set; }
        public string CookiebarYesText { get; set; }
        public string CookiebarNoText { get; set; }

        public string MemberLogo { get; set; }

        public string ProviderLogo { get; set; }
    }
}