(function ($, window) {
    "use strict";

    var checkDomain = function (url) {
        if (url.indexOf('//') === 0) { url = location.protocol + url; }
        return url.toLowerCase().replace(/([a-z])?:\/\//, '$1').split('/')[0];
    };

    var isExternal = function (url) {
        return ((url.indexOf(':') > -1 || url.indexOf('//') > -1) && checkDomain(location.href) !== checkDomain(url));
    };

    var fileExtension = function(url) {
        return url.split('.').pop().split(/\#|\?/)[0];
    }

    var ui = {
        events: {
            cookiesAccepted: "site.ui.cookiesAccepted",
            handleExternalLink: "site.ui.handleExternalLink",
            handleExternalLinkNavigate: "site.ui.handleExternalLinkNavigate",
            handlePDFLinkNavigate: "site.ui.navigatingtopdflink"
        },

        initialize: function () {
            var $ui = $(this);

            $('.faq-question').nyroModal();
            $('.img-fullscreen').nyroModal();

            $('a').not($('a', '#external-modal')).each(function () {
                var self = $(this);

                var url = self.attr('href');

                if (url !== undefined && isExternal(url)) {
                    self.addClass('external-link');
                    self.data('target', '#external-modal');
                }
                else if (fileExtension(url) === 'pdf') {
                    self.addClass('pdf-link');
                }
            });

            $('a.modal-continue', '#external-modal').on('click', function () {
                $.when($ui.trigger(ui.events.handleExternalLinkNavigate, $(this).attr('href')));
            });

            // What if it is false?
            if ($.cookie(Cookies.cookies_accepted) === undefined) {
                var links = $('#cookiebar-buttons a');
                if (links === undefined || links == null || links.length == 0) {
                    $ui.trigger(ui.events.cookiesAccepted);
                } else {
                    $('#cookiebar-button-yes').on('click', function (e) {
                        $('#prelayout').slideUp(function () {
                            $ui.trigger(ui.events.cookiesAccepted);
                        });
                        e.preventDefault();
                        return false;
                    });

                    $('#cookiebar-button-no').on('click', function (e) {
                        $('#prelayout').slideUp();
                        e.preventDefault();
                        return false;
                    });
                }
            } else {
                $ui.trigger(ui.events.cookiesAccepted);
            }

            // Show or hide the sticky footer button
            $(window).scroll(function () {
                if ($(this).scrollTop() > 200) {
                    $('#toTop').fadeIn(200);
                } else {
                    $('#toTop').fadeOut(200);
                }
            });

            // Animate the scroll to top
            $('#toTop').click(function (event) {
                event.preventDefault();

                $('html, body').animate({ scrollTop: 0 }, 300);
            });

            $(document).on('click', 'a.external-link', function (e) {
                $ui.trigger(ui.events.handleExternalLink, [this]);

                e.preventDefault();
                return false;
            });

            $(document).on('click', 'a.pdf-link', function (e) {
                $.when($ui.trigger(ui.events.handlePDFLinkNavigate, $(this).attr('href')));
            });

            $('#sidenavigation-menu').on('click', '.open', function () {
                var self = $(this);

                self.removeClass('open').addClass('closed');
                self.parent().children("ul").slideToggle();
            });

            $('#sidenavigation-menu').on('click', '.closed', function () {
                var self = $(this);

                self.removeClass('closed').addClass('open');
                self.parent().children("ul").slideToggle();
            });

            $('.panel-collapse').on('show', function (e) {
                $(this).parent().find('.panel-heading').addClass('active');
            });

            /* Bootstrap Panel (accordions) */
            $('.panel-group')
              .on('show.bs.collapse', function (e) {
                  $(e.target).prev('.panel-heading').addClass('active');
              })
              .on('hide.bs.collapse', function (e) {
                  $(e.target).prev('.panel-heading').removeClass('active');
              });
        },
    }

    if (!window.site) {
        window.site = {};
    }

    window.site.ui = ui;
})(jQuery, window);