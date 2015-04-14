using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage.InfosetStorage;

namespace ACME.Theme.Medical.Models {
    public class BingThemeSettingsPart : ContentPart {
        public string SiteVerificationCode {
            get { return Convert.ToString(this.As<InfosetPart>().Get<BingThemeSettingsPart>("SiteVerificationCode")); }
            set { this.As<InfosetPart>().Set<BingThemeSettingsPart>("SiteVerificationCode", value); }
        }
    }
}