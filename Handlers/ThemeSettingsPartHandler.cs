using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using ACME.Theme.Medical.Models;

namespace ACME.Theme.Medical.Handlers {
    public class ThemeSettingsPartHandler : ContentHandler {
        private readonly IWorkContextAccessor _workContextAccessor;

        public ThemeSettingsPartHandler(IWorkContextAccessor workContextAccessor) {
            _workContextAccessor = workContextAccessor;

            Filters.Add(new ActivatingFilter<ThemeSettingsPart>("Site"));
            Filters.Add(new TemplateFilterForPart<ThemeSettingsPart>("ThemeSettings", "Parts/ThemeSettings", "Theme Settings"));

            T = NullLocalizer.Instance;

            OnGetEditorShape<ThemeSettingsPart>((content, part) => {
                var workcontext = _workContextAccessor.GetContext();
                var settings = part.For(workcontext.CurrentCulture);
                if (settings == null) {
                    part.Culture = workcontext.CurrentCulture;
                    return;
                }

                part.Culture = settings.Culture;

                part.HeaderLogo = settings.HeaderLogo;
                part.HeaderLogoUrl = settings.HeaderLogoUrl;

                part.FooterLogo = settings.FooterLogo;
                part.FooterLogoUrl = settings.FooterLogoUrl;
                part.FooterAudienceMessage = settings.FooterAudienceMessage;
                part.FooterCopyrightsMessage = settings.FooterCopyrightsMessage;

                part.PublishedDateText = settings.PublishedDateText;
                part.PublishedDateFormat = settings.PublishedDateFormat;
                part.LowercasePublishedDate = settings.LowercasePublishedDate;
                part.NonContentPublishedDateText = settings.NonContentPublishedDateText;
                part.NonContentReferenceNumber = settings.NonContentReferenceNumber;
                part.NonContentPreparationDateText = settings.NonContentPreparationDateText;

                part.CookiebarText = settings.CookiebarText;
                part.CookiebarYesText = settings.CookiebarYesText;
                part.CookiebarNoText = settings.CookiebarNoText;

                part.MemberLogo = settings.MemberLogo;

                part.ProviderLogo = settings.ProviderLogo;
            });

            OnUpdateEditorShape<ThemeSettingsPart>((context, part) => {
                var workcontext = _workContextAccessor.GetContext();
                bool isNew = false;
                var settings = part.For(workcontext.CurrentCulture);
                if (settings == null) {
                    settings = new ThemeSetting();
                    isNew = true;
                }

                settings.Culture = part.Culture;

                settings.HeaderLogo = part.HeaderLogo;
                settings.HeaderLogoUrl = part.HeaderLogoUrl;

                settings.FooterLogo = part.FooterLogo;
                settings.FooterLogoUrl = part.FooterLogoUrl;
                settings.FooterAudienceMessage = part.FooterAudienceMessage;
                settings.FooterCopyrightsMessage = part.FooterCopyrightsMessage;

                settings.DisplayPublishedDate = part.DisplayPublishedDate;
                settings.PublishedDateText = part.PublishedDateText;
                settings.PublishedDateFormat = part.PublishedDateFormat;
                settings.LowercasePublishedDate = part.LowercasePublishedDate;
                settings.NonContentPublishedDateText = part.NonContentPublishedDateText;
                settings.NonContentReferenceNumber = part.NonContentReferenceNumber;
                settings.NonContentPreparationDateText = part.NonContentPreparationDateText;

                settings.CookiebarEnabled = part.CookiebarEnabled;
                settings.CookiebarText = part.CookiebarText;
                settings.CookiebarYesText = part.CookiebarYesText;
                settings.CookiebarNoText = part.CookiebarNoText;

                settings.MemberLogo = part.MemberLogo;

                settings.ProviderLogo = part.ProviderLogo;

                if (isNew) {
                    part.All = new List<ThemeSetting>(part.All) {settings};
                }
                else {
                    var all = new List<ThemeSetting>(part.All);
                    all.RemoveAll(x => x.Culture == part.Culture);
                    all.Add(settings);
                    part.All = all;
                }
            });
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);

            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Theme Settings")));
        }
    }
}