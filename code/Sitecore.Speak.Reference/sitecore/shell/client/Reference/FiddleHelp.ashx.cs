// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FiddleHelp.ashx.cs" company="">
//   
// </copyright>
// <summary>
//   Summary description for FiddleHelp
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sitecore.Shell.Client.Reference
{
  using System;
  using System.IO;
  using System.Linq;
  using System.Web;
  using System.Web.SessionState;
  using System.Xml;

  using Sitecore.Data.Items;
  using Sitecore.Text;
  using Sitecore.Web;

  /// <summary>
  /// Class FiddleHelp.
  /// </summary>
  public class FiddleHelp : IHttpHandler, IRequiresSessionState
  {
    #region Public Properties

    /// <summary>
    /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
    /// </summary>
    /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
    public bool IsReusable
    {
      get
      {
        return false;
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.</summary>
    /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/plain";

      var componentName = WebUtil.GetQueryString("t");

      var writer = new StringWriter();
      var output = new XmlTextWriter(writer)
      {
        Formatting = Formatting.Indented  
      };

      output.WriteStartElement("section");

      this.Render(output, componentName);

      output.WriteEndElement();

      context.Response.Write(writer.ToString());
    }

    #endregion

    #region Methods

    /// <summary>Renders the specified component name.</summary>
    /// <param name="output">The output.</param>
    /// <param name="componentName">Name of the component.</param>
    private void Render(XmlTextWriter output, string componentName)
    {
      var renderings = Context.Database.SelectItems("fast://*[@@templateid='{99F8905D-4A87-4EB8-9F8B-A9BEBFB3ADD6}']");

      var matches = renderings.Where(r => string.Compare(r.Name, componentName, StringComparison.InvariantCultureIgnoreCase) == 0).ToList();

      if (matches.Count == 0)
      {
        matches = renderings.Where(r => r.Name.IndexOf(componentName, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();
      }

      if (matches.Count > 1)
      {
        output.WriteString(string.Format("Ambigeous match. {0} matches found.", matches.Count));
        return;
      }

      if (matches.Count == 0)
      {
        output.WriteString("The component was not found.");
        return;
      }

      this.Render(output, matches.First());
    }

    /// <summary>Renders the specified output.</summary>
    /// <param name="output">The output.</param>
    /// <param name="component">Name of the component.</param>
    private void Render(XmlTextWriter output, Item component)
    {
      output.WriteElementString("h3", component.Name + " Component");

      if (!string.IsNullOrEmpty(component.Help.ToolTip))
      {
        output.WriteElementString("p", component.Help.ToolTip);
      }

      if (!string.IsNullOrEmpty(component.Help.Text))
      {
        output.WriteElementString("p", component.Help.Text);
      }

      var t = component["Parameters Template"];
      if (string.IsNullOrEmpty(t))
      {
        return;
      }

      var i = ClientHost.Databases.Database.GetItem(t);
      if (i == null)
      {
        return;
      }

      var defaultParameters = new UrlString(component["Default Parameters"]);
      var template = new TemplateItem(i);

      output.WriteElementString("h4", "Properties");

      output.WriteRaw(@"
    <table class=""table table-bordered table-striped table-condensed"">
      <thead>
        <tr>
          <th>
            Name
          </th>
          <th>
            Description
          </th>
          <th>
            Types
          </th>
          <th>
            Binding Mode
          </th>
          <th>
            Default Value
          </th>
        </tr>
      </thead>
      <tbody>");

      foreach (var templateSection in template.Fields.GroupBy(f => f.Section.Name).Select(g => new
      {
        Name = g.Key, Fields = g.OrderBy(f => f.DisplayName)
      }).OrderBy(g => g.Name))
      {
        var first = true;

        foreach (var field in templateSection.Fields.OrderBy(f => f.Name))
        {
          var templateItem = new TemplateItem(field.InnerItem.Parent.Parent);
          if (!templateItem.BaseTemplates.Any())
          {
            continue;
          }

          var source = new UrlString(field.Source);
          var values = string.Empty;

          var bindMode = source["bindMode"];
          if (string.IsNullOrEmpty(bindMode))
          {
            bindMode = "readwrite";
          }

          if (field.Type == "Droplist")
          {
            values = this.GetValues(field, source["datasource"] ?? string.Empty);
          }

          if (first)
          {
            output.WriteStartElement("tr");
            output.WriteAttributeString("class", "info");

            output.WriteStartElement("td");
            output.WriteAttributeString("colspan", "5");
            output.WriteValue(templateSection.Name);
            output.WriteEndElement();

            output.WriteEndElement();
          }

          first = false;
          var defaultValue = defaultParameters.Parameters[@field.Name] ?? string.Empty;
          output.WriteStartElement("tr");

          output.WriteStartElement("td");
          output.WriteElementString("code", field.Name);
          output.WriteEndElement();

          output.WriteStartElement("td");
          output.WriteRaw(field.ToolTip);
          if (!string.IsNullOrEmpty(field.ToolTip) && !string.IsNullOrEmpty(values))
          {
            output.WriteRaw("<br />");
          }

          output.WriteRaw(values);
          output.WriteEndElement();

          output.WriteElementString("td", field.Type);
          output.WriteElementString("td", bindMode);
          output.WriteElementString("td", defaultValue);

          output.WriteEndElement();
        }
      }

      output.WriteRaw(@"
    </tbody>
  </table>");
    }

    /// <summary>
    /// Gets the values.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="datasource">The datasource.</param>
    /// <returns>Returns the values.</returns>
    private string GetValues(TemplateFieldItem field, string datasource)
    {
      if (string.IsNullOrEmpty(datasource))
      {
        return string.Empty;
      }

      var root = field.InnerItem.Database.GetItem(datasource);
      if (root == null)
      {
        return string.Empty;
      }

      return "Values: " + string.Join(", ", root.Children.Select(i => "<code>" + i.Name + "</code>"));
    }

    #endregion
  }
}