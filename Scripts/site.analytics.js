(function ($, window) {
    "use strict";

    var fileName = function (url) {
        return url.substr(url.lastIndexOf("/") + 1, url.length).toLowerCase();
    }

    var ga_enabled = ga !== undefined;

    var googleanalytics = {
        trackexternallink: function (url) {
            if (ga_enabled) {
                ga('send', 'event', 'outbound', 'click', url,
                    { 'nonInteraction': 1 }
                );
            }
        },

        trackpdflink: function (url) {
            if (ga_enabled) {
                ga('send', 'pageview', fileName(url));
            }
        }
    }

    var analytics = {
        trackexternallink: function(url) {
            googleanalytics.trackexternallink(url);
        },

        trackpdflink: function (url) {
            googleanalytics.trackpdflink(url);
        }
    }

    if (!window.site) {
        window.site = {};
    }

    window.site.analytics = analytics;

})(jQuery, window);