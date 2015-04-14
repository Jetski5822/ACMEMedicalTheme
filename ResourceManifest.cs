using Orchard.UI.Resources;

namespace ACME.Theme.Medical {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            // Nyro Modal
            manifest.DefineScript("nyroModal").SetUrl("jquery.nyroModal.custom.min.js").SetVersion("2.0.0").SetDependencies("jQuery");
            manifest.DefineStyle("nyroModal").SetUrl("nyroModal.min.css", "nyroModal.css").SetVersion("2.0.0");

            manifest.DefineScript("bootstrap").SetUrl("bootstrap-3.3.2/js/bootstrap.min.js", "bootstrap-3.3.2/js/bootstrap.js").SetVersion("3.3.2").SetDependencies("jQuery");
            manifest.DefineScript("jquery_cookie").SetUrl("jquery.cookie.js").SetVersion("1.4.1").SetDependencies("jQuery");
            
            manifest.DefineScript("siteui").SetUrl("site.ui.min.js", "site.ui.js").SetDependencies("jQuery", "jquery_cookie", "nyroModal");
            manifest.DefineScript("site").SetUrl("site.min.js", "site.js").SetDependencies("siteui");

            manifest.DefineScript("Respond")
                .SetUrl("respond.min.js")
                .SetVersion("1.4.2");
            
            DefineSiteStyle(manifest, "default");
            DefineSiteStyle(manifest, "member");
        }

        private void DefineSiteStyle(Orchard.UI.Resources.ResourceManifest manifest, string area) {
            var formatted = area.Replace('-', '_');

            manifest.DefineStyle("site_" + formatted)
                .SetUrl("site-" + area + ".min.css", "site-" + area + ".css")
                .SetDependencies("nyroModal");

            manifest.DefineStyle("site_" + formatted + "_rtl")
                .SetUrl("site-" + area + "-rtl.min.css", "site-" + area + "-rtl.css")
                .SetDependencies("site_" + formatted);
        }
    }
}
