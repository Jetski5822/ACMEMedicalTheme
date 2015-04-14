using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Security;
using Orchard.Settings;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace ACME.Theme.Medical.Services {
    public interface ITaxonomyImportService : IDependency {
        TaxonomyPart CreateTaxonomy(string taxonomyName);
        TermPart CreateTermFor(TaxonomyPart taxonomy, string termName, string termSlug);
        TermPart CreateChildTermFor(TaxonomyPart taxonomy, TermPart parent, string termName, string termSlug);
    }

    public class TaxonomyImportService : ITaxonomyImportService {
        private readonly ISiteService _siteService;
        private readonly IContentManager _contentManager;
        private readonly ITaxonomyService _taxonomyService;
        private readonly IMembershipService _membershipService;

        public TaxonomyImportService(ISiteService siteService, IContentManager contentManager,
            ITaxonomyService taxonomyService, IMembershipService membershipService) {
            _siteService = siteService;
            _contentManager = contentManager;
            _taxonomyService = taxonomyService;
            _membershipService = membershipService;
        }

        public TaxonomyPart CreateTaxonomy(string taxonomyName) {
            var existingTaxonomy = _taxonomyService.GetTaxonomyByName(taxonomyName);
            if (existingTaxonomy != null)
                return existingTaxonomy;

            var taxonomy = _contentManager.Create<TaxonomyPart>("Taxonomy", VersionOptions.Published, (content) => {
                content.As<ICommonPart>().Owner = _membershipService.GetUser(_siteService.GetSiteSettings().SuperUser);

                content.Slug = taxonomyName.ToLowerInvariant();
                content.Name = taxonomyName;
                content.IsInternal = false;
            });

            return taxonomy;
        }

        public TermPart CreateTermFor(TaxonomyPart taxonomy, string termName, string termSlug) {
            var existingTerm = _taxonomyService.GetTermByName(taxonomy.Id, termName);

            if (existingTerm != null)
                return existingTerm;

            return CreateChildTermFor(taxonomy, null, termName, termSlug);
        }

        public TermPart CreateChildTermFor(TaxonomyPart taxonomy, TermPart parent, string termName, string termSlug) {
            var existingTerm = _taxonomyService.GetTermByName(taxonomy.Id, termName);

            if (existingTerm != null)
                return existingTerm;

            var term = _taxonomyService.NewTerm(taxonomy, parent);
            _contentManager.Create(term, VersionOptions.Draft);

            term.Weight = 0;
            term.Name = termName.Trim();

            if (!string.IsNullOrEmpty(termSlug) || !string.IsNullOrWhiteSpace(termSlug))
                term.Slug = termSlug.Trim();

            _contentManager.Publish(term.ContentItem);

            return term;
        }
    }
}