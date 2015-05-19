using System.Collections.Generic;
using System.Linq;
using Orchard;
using Orchard.Alias;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment.Descriptor.Models;
using Orchard.Taxonomies.Models;

namespace ACME.Theme.Medical {

    public static class WorkContextExtensions {
        public static bool IsHomepageRequest(this WorkContext workContext) {
            var requestPath = GetRequestUrl(workContext);

            if (string.IsNullOrEmpty(requestPath))
                return true;

            var contentItem = GetByPath(workContext, requestPath);

            if (contentItem.As<ILocalizableAspect>() != null) {
                if (contentItem.ContentItem.Parts.All(p => p.TypePartDefinition.PartDefinition.Name != "LocalizationPart"))
                    return false;

                var localizationPart = ((dynamic)contentItem).LocalizationPart;

                if ((bool)localizationPart.HasTranslationGroup) {
                    if (((IContent)localizationPart.MasterContentItem).As<IAliasAspect>().Path == string.Empty) {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsMemberRequest(this WorkContext workContext) {
            return workContext.IsAreaRequest("Member");
        }

        public static bool IsProviderRequest(this WorkContext workContext) {
            return workContext.IsAreaRequest("Provider");
        }

        public static bool IsDefaultRequest(this WorkContext workContext) {
            return workContext.IsAreaRequest("Default");
        }

        public static bool IsNurseRequest(this WorkContext workContext) {
            return workContext.IsAreaRequest("Nurse");
        }

        public static bool IsPartnerRequest(this WorkContext workContext) {
            return workContext.IsAreaRequest("Partner");
        }

        public static bool IsNonContentRequest(this WorkContext workContext) {
            var requestPath = GetRequestUrl(workContext);

            var contentRouting = workContext.Resolve<IAliasService>().Get(requestPath);

            return contentRouting == null;
        }

        public static bool IsAreaRequest(this WorkContext workContext, string area) {
            var siteArea = workContext.GetAreaName();

            return !string.IsNullOrEmpty(siteArea) && siteArea.Equals(area, System.StringComparison.OrdinalIgnoreCase);
        }

        public static string GetAreaName(this WorkContext workContext) {
            var requestPath = GetRequestUrl(workContext);

            var contentItem = GetByPath(workContext, requestPath);

            if (contentItem == null)
                return null;

            if (contentItem.ContentItem.Parts.All(p => p.TypePartDefinition.PartDefinition.Name != "PageMetadataPart"))
                return null;

            var siteArea = ((IEnumerable<TermPart>)((dynamic)contentItem).PageMetadataPart.SiteArea.Terms).SingleOrDefault();

            return siteArea != null ? siteArea.Name : null;
        }

        public static IContent GetByPath(WorkContext workContext, string aliasPath) {
            var contentRouting = workContext.Resolve<IAliasService>().Get(aliasPath);

            if (contentRouting == null)
                return null;

            object id;
            if (contentRouting.TryGetValue("id", out id)) {
                int contentId;
                if (int.TryParse(id as string, out contentId)) {
                    return workContext.Resolve<IContentManager>().Get(contentId);
                }
            }

            return null;
        }

        public static bool IsFeatureEnabled(this WorkContext workContext, string featureName) {
            return workContext.Resolve<ShellDescriptor>().Features.Any(x => x.Name == featureName);
        }

        public static bool HasAcceptedCookies(this WorkContext workContext) {
            return workContext.HttpContext.Request.Cookies.Get("user_cookies_accepted") != null;
        }

        private static string GetRequestUrl(WorkContext workContext) {
            var path = workContext.HttpContext.Request.Path;

            if (workContext.HttpContext.Request.Url != null) {
                path = workContext.HttpContext.Request.Url.AbsolutePath;
            }

            return path.TrimStart('/').ToLowerInvariant();
        }
    }
}
