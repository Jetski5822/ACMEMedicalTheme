/// <reference path="site.ui.js" />
/// <reference path="site.analytics.js" />
var Cookies = {};

Cookies.cookies_accepted = 'user_cookies_accepted';

(function ($, window, ui, analytics) {
    "use strict";

    var $ui = $(ui);

    $ui.bind(ui.events.cookiesAccepted, function (event) {
        if ($.cookie(Cookies.cookies_accepted) === undefined) {
            $.cookie(Cookies.cookies_accepted, 'true', { "path": '/' });
        }

        $('#prelayout').addClass('hidden');

        // Needs to be an event fired off to the site analytics js file
        if (window.enableGA !== undefined) {
            window.enableGA();
        }
    });

    $ui.bind(ui.events.handleExternalLink, function (event, self) {
        var modal = $('#external-modal');
        var language = $('html').attr('lang');

        // en-US/modaldialog/external-default
        var remote = '/' + language + '/modaldialog/external-default';

        var type = $(self).data('external-type');

        if (type !== undefined) {
            remote = '/' + language + '/modaldialog/external-' + type;
        }

        $.get(remote, {}, function (data) {
            var $response = $('<div />').html(data);
            $('.modal-header', modal).html($response.find('#modal-header'));
            $('.modal-body', modal).html($response.find('#modal-content'));
            $('.modal-close', modal).html($response.find('#modal-close'));
            $('.modal-continue', modal).html($response.find('#modal-continue')).attr('href', $(self).attr('href'));
            $('#modal').modal('show');
        }, 'html');
    });

    $(ui).bind(ui.events.handleExternalLinkNavigate, function(url) {
        analytics.trackexternallink(url);
    });

    $(ui).bind(ui.events.handlePDFLinkNavigate, function (url) {
        analytics.trackpdflink(url);
    });

    $(function () {
        console.log("UI Initializing...");
        ui.initialize();
        console.log("UI Initialized!");
    });

})(jQuery, window, window.site.ui, window.site.analytics);