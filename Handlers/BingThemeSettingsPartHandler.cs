using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using ACME.Theme.Medical.Models;

namespace ACME.Theme.Medical.Handlers {
    [UsedImplicitly]
    public class BingThemeSettingsPartHandler : ContentHandler {
        public BingThemeSettingsPartHandler() {
            Filters.Add(new ActivatingFilter<BingThemeSettingsPart>("Site"));
            Filters.Add(new TemplateFilterForPart<BingThemeSettingsPart>("BingSettings", "Parts/BingSettings", "Bing Settings"));

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);

            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Bing Settings")));
        }
    }
}