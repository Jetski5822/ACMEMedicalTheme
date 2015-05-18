# ACMEMedicalTheme
ACME Medical Theme for Orchard is for a fictitious company.

1. RTL support 
2. Ability to style different areas of the site by taxonomy 
3. Taxonomy layer rule for sitearea('site area name here') 
4. Bing SEO 
5. Google SEO 
6. Multi-Culture Theme Settings 
7. Cookie Bar built in 
8. External Link dialogs
9. Wordpress shortcodes for a bunch of things (Though, you will need to wait for me to upgrade tinymce for you to see them nicely surfaced in TinyMCE) 
10. Footer all configurable from the Theme Settings admin 
11. Uses LESS 

## To Use ##

First Clone:
git clone https://github.com/Jetski5822/ACMEMedicalTheme.git ACME.Theme.Medical

*Note: make sure the folder the theme is in is called ACME.Theme.Medical*

Second, Enable!

## RTL ##

This theme will inject a RTL CSS style sheet fi yo uare viewing the site as RTL based on the sitearea.

Snippet from Resources.cshtml:

    if (WorkContext.CurrentCultureInfo().TextInfo.IsRightToLeft) {
        Style.Require(string.Format("site_{0}_rtl", area));
    }
    else {
        Style.Require(string.Format("site_{0}", area));
    }

## Site Areas ##

Within any site you might want to style you site differently depending on the area, this is difficult if the page has no taxonomy telling you what it is, and you dont want to rely on the url. I have added a Site Area taxonomy with the ability to style by area.

The out of box terms are :

- Default
- Provider
- Nurse
- Member
- Partner

you can use sitearea layer rules too to say, sitearea('nurse')

Then to style, in Resources.cshtml you can specify what CSS you want to use based on site area.

## Work Context Helpers ##

Trying to know where you are in a site and having to write lots of code each time is a pain, so I have added helpers so you dont have to...

Wanna do something if you are in the Member area?

    if (WorkContext.IsMemberRequest()) {
    }
    // same as
    if (WorkContext.IsAreaRequest("Member")) {
    }

There are lots:

- WorkContext.IsMemberRequest()
- WorkContext.IsProviderRequest()
- WorkContext.IsDefaultRequest()
- WorkContext.IsNurseRequest()
- WorkContext.IsPartnerRequest()
- WorkContext.IsNonContentRequest()
- WorkContext.IsAreaRequest("Member")
- WorkContext.IsHomepageRequest()

How about checking if a feature is enabled!?

    if (WorkContext.IsFeatureEnabled("Orchard.Search")) {
    }

Or checking if Cookies have been accepted?

    if (WorkContext.HasAcceptedCookies()) {
    }

## Bing ##

You can add bing detail by going to the Admin screens and under the setting section, look for "Bing Settings"

From here you can add you Bing Verification Code.

## Google ##

You can add bing detail by going to the Admin screens and under the setting section, look for "Google Settings"

From here you can add you Google Verification Code and other details.

## Cookie Bar ##

Sites have cookie bars, but not all. You can configurate if you want the cookie bar or not by going to theme settings and ticking "Enable Cookie Bar". This settings is culture agnostic, which means, you turn it on for one culture it will be on for all, the text however is culture independent.

Rules:

1. If you are logged in, cookies are automatically accepted
2. If you only have a "No" button showing, then cookies are are automatically accepted
3. If you have a Yes and No button showing, then cookies are accepted only when you hit Yes, hitting no will hide the cookie bar for one request using Javascript to avoid caching issues.
