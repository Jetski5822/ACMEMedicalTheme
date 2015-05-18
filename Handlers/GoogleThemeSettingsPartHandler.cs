using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using ACME.Theme.Medical.Models;

namespace ACME.Theme.Medical.Handlers {
    public class GoogleThemeSettingsPartHandler : ContentHandler {
        public GoogleThemeSettingsPartHandler() {
            Filters.Add(new ActivatingFilter<GoogleThemeSettingsPart>("Site"));
            Filters.Add(new TemplateFilterForPart<GoogleThemeSettingsPart>("GoogleSettings", "Parts/GoogleSettings", "Google Settings"));

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);

            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Google Settings")));
        }
    }
}