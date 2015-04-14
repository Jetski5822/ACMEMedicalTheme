using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage.InfosetStorage;

namespace ACME.Theme.Medical.Models {
    public class GoogleThemeSettingsPart : ContentPart {
        public string SiteVerificationCode {
            get { return Convert.ToString(this.As<InfosetPart>().Get<GoogleThemeSettingsPart>("SiteVerificationCode")); }
            set { this.As<InfosetPart>().Set<GoogleThemeSettingsPart>("SiteVerificationCode", value); }
        }

        public bool NoTranslate {
            get { return Convert.ToBoolean(this.As<InfosetPart>().Get<GoogleThemeSettingsPart>("NoTranslate")); }
            set { this.As<InfosetPart>().Set<GoogleThemeSettingsPart>("NoTranslate", value.ToString()); }
        }

        public bool NoSiteLinksSearchBox {
            get { return Convert.ToBoolean(this.As<InfosetPart>().Get<GoogleThemeSettingsPart>("NoSiteLinksSearchBox")); }
            set { this.As<InfosetPart>().Set<GoogleThemeSettingsPart>("NoSiteLinksSearchBox", value.ToString()); }
        }

        public string TrackingId {
            get { return Convert.ToString(this.As<InfosetPart>().Get<GoogleThemeSettingsPart>("TrackingId")); }
            set { this.As<InfosetPart>().Set<GoogleThemeSettingsPart>("TrackingId", value); }
        }

        public bool IsDebug {
            get { return Convert.ToBoolean(this.As<InfosetPart>().Get<GoogleThemeSettingsPart>("IsDebug")); }
            set { this.As<InfosetPart>().Set<GoogleThemeSettingsPart>("IsDebug", value.ToString()); }
        }
    }
}