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
