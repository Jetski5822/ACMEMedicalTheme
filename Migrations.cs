using System;
using ACME.Theme.Medical.Services;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ACME.Theme.Medical {
    public class Migrations : DataMigrationImpl {
        private readonly ITaxonomyImportService _taxonomyImportService;

        public Migrations(ITaxonomyImportService taxonomyImportService) {
            _taxonomyImportService = taxonomyImportService;
        }

        public int Create() {
            var siteareaTaxonomyPart = _taxonomyImportService.CreateTaxonomy("SiteArea");
            var tagsTaxonomyPart = _taxonomyImportService.CreateTaxonomy("Tags");

            _taxonomyImportService.CreateTermFor(siteareaTaxonomyPart, "Default", "default");
            var areaMemberTerm = _taxonomyImportService.CreateTermFor(siteareaTaxonomyPart, "Member", "member");
            _taxonomyImportService.CreateChildTermFor(siteareaTaxonomyPart, areaMemberTerm, "Partner", "member/partner");
            var areaProviderTerm = _taxonomyImportService.CreateTermFor(siteareaTaxonomyPart, "Provider", "provider");
            _taxonomyImportService.CreateChildTermFor(siteareaTaxonomyPart, areaProviderTerm, "Nurse", "provider/nurse");

            ContentDefinitionManager.AlterPartDefinition("PageMetadataPart", builder => builder
                .WithField("SiteArea", cfg => cfg
                    .OfType("TaxonomyField")
                    .WithSetting("TaxonomyFieldSettings.AllowCustomTerms", "false")
                    .WithSetting("TaxonomyFieldSettings.SingleChoice", "true")
                    .WithSetting("TaxonomyFieldSettings.Required", "true")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", siteareaTaxonomyPart.Name)
                    .WithDisplayName("Site Area"))
                .WithField("Tags", cfg => cfg
                    .OfType("TaxonomyField")
                    .WithSetting("TaxonomyFieldSettings.AllowCustomTerms", "true")
                    .WithSetting("TaxonomyFieldSettings.SingleChoice", "false")
                    .WithSetting("TaxonomyFieldSettings.Required", "false")
                    .WithSetting("TaxonomyFieldSettings.Autocomplete", "true")
                    .WithSetting("TaxonomyFieldSettings.Hint", "Tagging content will feed into the keywords associated to the page.")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", tagsTaxonomyPart.Name))
                .WithField("Description", fld => fld
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "Large")
                    .WithSetting("TextFieldSettings.Hint", "Max of 155 characters, if you go over the extra characters will be trimmed off. For information go here, https://support.google.com/webmasters/answer/35624?rd=1")
                    .WithDisplayName("Description"))
                .WithField("ReferenceNumber", fld => fld
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "Wide")
                    .WithDisplayName("Reference Number"))
                .WithField("PreparationDate", fld => fld
                    .OfType("DateTimeField")
                    .WithSetting("DateTimeFieldSettings.Display", "DateOnly")
                    .WithDisplayName("Preparation Date"))
                .Attachable());
            
            ContentDefinitionManager.AlterTypeDefinition("Page", cfg => cfg
                .WithPart("PageMetadataPart"));

            var modalTypeTaxonomyPart = _taxonomyImportService.CreateTaxonomy("ModalType");

            _taxonomyImportService.CreateTermFor(modalTypeTaxonomyPart, "Internal Default", "internal-default");
            _taxonomyImportService.CreateTermFor(modalTypeTaxonomyPart, "External Default", "external-default");
            _taxonomyImportService.CreateTermFor(modalTypeTaxonomyPart, "External Alternative", "external-alternative");

            ContentDefinitionManager.AlterPartDefinition("ModalDialogPart", builder => builder
                .WithField("ModalType", cfg => cfg
                    .OfType("TaxonomyField")
                    .WithSetting("TaxonomyFieldSettings.AllowCustomTerms", "false")
                    .WithSetting("TaxonomyFieldSettings.SingleChoice", "true")
                    .WithSetting("TaxonomyFieldSettings.Required", "true")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", modalTypeTaxonomyPart.Name)
                    .WithDisplayName("Modal Type"))
                .WithField("CancelButtonText", fld => fld
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "Wide")
                    .WithDisplayName("Cancel Button Text"))
                .WithField("ContinueButtonText", fld => fld
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "Wide")
                    .WithDisplayName("Continue Button Text"))
            );

            ContentDefinitionManager.AlterTypeDefinition("ModalDialog", cfg => cfg
                    .WithPart("ModalDialogPart")
                    .WithPart("CommonPart")
                    .WithPart("TitlePart")
                    .WithPart("AutoroutePart", builder => builder
                        .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                        .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                        .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: '{Content.Culture.Name}/modaldialog/{Content.Slug}', Description: 'culture/modaldialog/slug'}]")
                        .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
                    .WithPart("BodyPart")
                    .DisplayedAs("Modal Dialog")
                    .Creatable()
            );

            return 1;
        }
    }
}
