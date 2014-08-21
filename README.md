Sitecore.Speak.Reference
========================
This projects implements a reference/documentation website and a Fiddle for Sitecore SPEAK.

You can watch a demo of the website on YouTube: <a href="https://www.youtube.com/watch?v=HvR6AGVJO-Y&list=PLWIbrolNZWflnBq32WcxNejEgiT2lyEwG&index=13">Sitecore SPEAK Reference and Fiddle tech demo</a>.

<h3>Installation</h3>

Download the Zip file: `Sitecore SPEAK Reference and Fiddle.zip` from the root of this repository and install it in a Sitecore CMS 7.5 installation. Please notice that the website will not work in previous version Sitecore.

DO NOT INSTALL IT IN A PRODUCTION WEBSITE.

When the package has been installed, navigate to: /sitecore/client/reference

Since the SPEAK Fiddle is posting Html and JavaScript to the server, you may encounter the ASP.NET validation error "A potentially dangerous Request.Form value was detected from the client" when running a Fiddle.

In that case set the attribute `requestValidationMode="2.0"` to the `httpRuntime` element in the web.config.

It is highly recommended that you try out the Fiddle tutorials.

<h3>Reference Website<h3>

The reference website is dynamically generated from View rendering items and their parameter templates. If you add a new rendering to the website, it will automatically appear in the reference web site.

The reference site also runs 11 validations on each rendering:
* The 'Datasource Location' should be set to 'PageSettings'
* The 'Datasource Template' should be set to the Parameters Template item ID
* Control has an invalid default parameter
* Control must have short help text
* Control must have long help text
* Control short help text must end with a dot
* Control long help text must end with a dot
* Parameter must have help text
* Parameter help text must end with a dot
* Control does not have a Parameter Template
* Parameter Template does not exist
 
<h3>SPEAK Fiddle<h3>

The SPEAK Fiddle is a sandbox where you can modify the MVC View, JavaScript/TypeScript, Layout Definition and Items of a rendering and see the results immediately.

The SPEAK Fiddle is heavily inspired by the JSFiddle.

There are some limitations to what can be done in the SPEAK Fiddle, but most basic stuff should work.

Again, it is highly recommended that you try out the Fiddle tutorials.
