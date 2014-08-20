// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fiddle.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

/*
<r>
  <d id="{FE5D7FDF-89C0-4D99-9AA3-B5FBD009C9F3}" l="{99C9A84D-AA93-4B2C-ADE1-D349B804590D}">
    <r id="{DAFAFFB8-74AF-4141-A96A-70B16834CEC6}" ph="Page.Code" uid="{8F5DB9F1-65E0-4D6E-A347-32EC598A6021}" />
    <r id="{700DA48B-CB23-4459-8569-9E8241A2DCA6}" par="Id=NavBar1&amp;HasInnerContainer=1&amp;Fixed=Top&amp;BrandText=SPEAK%2bBootstrap" ph="Page.Body" uid="{989DF6EE-0C2F-4691-AA52-0633C64984D9}" />
    <r id="{FE37A6F2-69E0-4AD4-A423-69F165F8D3A9}" par="LinkField=%252525252524itemid&amp;Id=Nav1&amp;ItemsDataSource=%7bA463BE78-118D-4FE6-9A22-2540B86C4928%7d&amp;Active=Scaffolding" ph="NavBar.Inner" uid="{17328353-8393-4588-B98D-1F79A5176D68}" />
    <r id="{E6FC07D8-2303-4D10-8380-573CCBC82B89}" par="Id=Container1" ph="Page.Body" uid="{4C620A63-1E37-4EEB-A69C-39F78A6F4028}" />
    <r id="{4B77BB29-C937-4620-A6CC-A14B1A38DE8A}" par="Id=RenderView1&amp;Path=%2fsitecore%2fshell%2fclient%2fSpeakStuff%2fContent%2fSpeakFiddle%2fSpeakFiddle.cshtml" ph="Page.Body" uid="{8686AEEB-6B16-4DE6-BD13-948BEF4CE2D8}" />
  </d>
</r>     
*/
namespace Sitecore.Fiddles
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Web;
  using System.Xml;
  using System.Xml.Linq;
  using System.Xml.XPath;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Data.Templates;
  using Sitecore.Diagnostics;
  using Sitecore.Extensions.StringExtensions;
  using Sitecore.Extensions.XElementExtensions;
  using Sitecore.IO;
  using Sitecore.Mvc.Names;
  using Sitecore.Reflection;
  using Sitecore.Text;
  using Sitecore.Web;
  using Sitecore.Web.UI.Controls.Common.ItemRenderers;

  /// <summary>
  /// Defines the Fiddle class.
  /// </summary>
  public class Fiddle
  {
    #region Constants

    /// <summary>
    /// The root folder
    /// </summary>
    public const string RootFolder = @"/sitecore/shell/client/Reference/Fiddles";

    /// <summary>
    /// The temporary folder
    /// </summary>
    public const string TempFolder = @"/sitecore/shell/client/Reference/Temp";

    /// <summary>
    /// The sitecore client fiddle pagesettings items
    /// </summary>
    private const string ItemsPath = "/sitecore/client/Reference/Fiddle/PageSettings/Items";

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="Fiddle"/> class. Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
    /// <param name="fiddleId">The id.</param>
    public Fiddle([NotNull] string fiddleId)
    {
      Assert.ArgumentNotNull(fiddleId, "fiddleId");

      this.FiddleId = fiddleId;
      this.Code = string.Empty;
      this.View = string.Empty;
      this.Next = string.Empty;
      this.Previous = string.Empty;
      this.Renderings = string.Empty;
      this.ReadOnly = false;
      this.Text = string.Empty;
      this.Items = string.Empty;
      this.CodeLanguage = string.Empty;
      this.TypeScriptCompilerPath = string.Empty;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the java script.
    /// </summary>
    /// <value>The java script.</value>
    [NotNull]
    public string Code { get; private set; }

    /// <summary>
    /// Gets the code language.
    /// </summary>
    /// <value>The code language.</value>
    [NotNull]
    public string CodeLanguage { get; private set; }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [NotNull]
    public string FiddleId { get; private set; }

    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    /// <value>The name of the file.</value>
    [NotNull]
    public string FileName
    {
      get
      {
        return FileUtil.UnmapPath(this.GetFileName(this.FiddleId), false);
      }
    }

    /// <summary>
    /// Gets the items.
    /// </summary>
    /// <value>The items.</value>
    [NotNull]
    public string Items { get; private set; }

    /// <summary>
    /// Gets the next.
    /// </summary>
    /// <value>The next.</value>
    [NotNull]
    public string Next { get; private set; }

    /// <summary>
    /// Gets the previous.
    /// </summary>
    /// <value>The previous.</value>
    [NotNull]
    public string Previous { get; private set; }

    /// <summary>
    /// Gets a value indicating whether [read only].
    /// </summary>
    /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
    public bool ReadOnly { get; private set; }

    /// <summary>
    /// Gets the renderings.
    /// </summary>
    /// <value>The renderings.</value>
    [NotNull]
    public string Renderings { get; private set; }

    /// <summary>
    /// Gets the text.
    /// </summary>
    /// <value>The text.</value>
    [NotNull]
    public string Text { get; private set; }

    /// <summary>
    /// Gets or sets the type script compiler path.
    /// </summary>
    /// <value>The type script compiler path.</value>
    [NotNull]
    public string TypeScriptCompilerPath { get; set; }

    /// <summary>
    /// Gets the MVC view.
    /// </summary>
    /// <value>The MVC view.</value>
    [NotNull]
    public string View { get; private set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>Creates the based on component.</summary>
    /// <param name="item">The item.</param>
    public void CreateBasedOnComponent([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      var path = item["Path"];

      var viewFileName = FileUtil.MapPath(Path.ChangeExtension(path, "cshtml"));
      if (File.Exists(viewFileName))
      {
        this.View = File.ReadAllText(viewFileName);
      }

      var codeFileName = FileUtil.MapPath(Path.ChangeExtension(path, "ts"));
      if (!File.Exists(codeFileName))
      {
        codeFileName = FileUtil.MapPath(Path.ChangeExtension(path, "js"));
      }

      if (File.Exists(codeFileName))
      {
        var code = File.ReadAllText(codeFileName);
        this.Code = code.Replace(item.Name, "Fiddle");
      }

      this.CodeLanguage = Path.GetExtension(codeFileName).Replace(".", string.Empty);

      var parametersTemplate = item["Parameters Template"];
      if (!string.IsNullOrEmpty(parametersTemplate))
      {
        parametersTemplate = " ParametersTemplate=\"" + parametersTemplate + "\"";
      }

      this.Renderings = "<Layout>\r  <PageCode />\r  <Fiddle" + parametersTemplate + " />\r</Layout>";
    }

    /// <summary>Creates from file upload.</summary>
    /// <param name="file">The file.</param>
    public void CreateFromFileUpload([NotNull] HttpPostedFileBase file)
    {
      Assert.ArgumentNotNull(file, "file");

      var fileName = this.GetFileName(this.FiddleId);

      var folder = Path.GetDirectoryName(fileName);
      if (string.IsNullOrEmpty(folder))
      {
        return;
      }

      Directory.CreateDirectory(folder);
      file.SaveAs(fileName);

      this.LoadFromFile(fileName);
    }

    /// <summary>Loads from template.</summary>
    /// <param name="templateId">The template identifier.</param>
    public void CreateFromTemplate([NotNull] string templateId)
    {
      Assert.ArgumentNotNull(templateId, "templateId");

      var fileName = FileUtil.MapPath(RootFolder);
      fileName = Path.Combine(fileName, "Templates");
      fileName = Path.Combine(fileName, templateId);
      fileName = Path.ChangeExtension(fileName, "xml");

      if (!File.Exists(fileName))
      {
        throw new InvalidOperationException(string.Format("Template '{0}' not found", templateId));
      }

      this.LoadFromFile(fileName);
    }

    /// <summary>Creates the using component.</summary>
    /// <param name="item">The item.</param>
    public void CreateUsingComponent([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      var name = item.Name;
      string component;

      if (Regex.IsMatch(name, "[\\w_-]", RegexOptions.IgnoreCase))
      {
        component = "<" + name + " />";
      }
      else
      {
        component = "<r id=\"" + item.ID + "\" />";
      }

      this.View = "@using Sitecore.Mvc\r\r@model Sitecore.Mvc.Presentation.RenderingModel\r<p>Fiddle Component</p>";
      this.Renderings = "<Layout>\r  <PageCode />\r  " + component + "\r  <Fiddle />\r</Layout>";
    }

    /// <summary>Forks the specified identifier.</summary>
    /// <param name="fiddleId">The identifier.</param>
    public void Fork([NotNull] string fiddleId)
    {
      Assert.ArgumentNotNull(fiddleId, "fiddleId");

      this.FiddleId = fiddleId;
      this.ReadOnly = false;
      this.Text = string.Empty;
      this.Next = string.Empty;
      this.Previous = string.Empty;
    }

    /// <summary>
    /// Loads this instance.
    /// </summary>
    public void LoadFromPost()
    {
      this.LoadFromStorage();

      var form = HttpContext.Current.Request.Unvalidated.Form;

      this.Code = form["code"] ?? string.Empty;
      this.CodeLanguage = form["codelanguage"] ?? string.Empty;
      this.View = form["view"] ?? string.Empty;
      this.Renderings = form["renderings"] ?? string.Empty;
      this.Items = form["items"] ?? string.Empty;
    }

    /// <summary>
    /// Loads this instance.
    /// </summary>
    public void LoadFromStorage()
    {
      var fileName = this.GetFileName(this.FiddleId);

      if (File.Exists(fileName))
      {
        this.LoadFromFile(fileName);
      }
    }

    /// <summary>
    /// Renders this instance.
    /// </summary>
    /// <returns>Returns the string.</returns>
    [NotNull]
    public string Render()
    {
      var fileName = FileUtil.MapPath(Path.Combine(TempFolder, this.FiddleId));
      FileUtil.CreateFolder(Path.GetDirectoryName(fileName));

      var viewFileName = Path.ChangeExtension(fileName, ".cshtml");
      File.WriteAllText(viewFileName, this.View, Encoding.UTF8);

      var codeFileName = Path.ChangeExtension(fileName, ".js");

      if (!string.IsNullOrWhiteSpace(this.Code))
      {
        this.WriteCode(codeFileName);
      }
      else if (File.Exists(codeFileName))
      {
        File.Delete(codeFileName);
      }

      try
      {
        if (string.IsNullOrEmpty(this.Renderings))
        {
          throw new InvalidOperationException("The Layout Renderings field must be a well-formed XML document.");
        }

        XDocument doc;
        try
        {
          doc = XDocument.Parse(this.Renderings);
        }
        catch
        {
          throw new InvalidOperationException("The Layout Renderings field is not well-formed.");
        }

        var root = doc.Root;
        if (root == null)
        {
          throw new InvalidOperationException("The Layout Renderings field is not well-formed.");
        }

        var fiddleRendering = this.CreateFiddleRendering();
        try
        {
          this.CreateItems(fiddleRendering, root, FileUtil.UnmapPath(viewFileName, false));

          var child = fiddleRendering.Children.FirstOrDefault() ?? Context.Item;

          var renderings = this.ParseLayout(root, fiddleRendering);

          var itemRenderer = new ItemRenderer(renderings, "/")
          {
            UsePlaceholders = true
          };

          using (new ContextItemSwitcher(child))
          {
            return itemRenderer.Render();
          }
        }
        finally
        {
          fiddleRendering.Delete();
        }
      }
      catch (Exception ex)
      {
        var text = string.Empty;
        while (ex != null)
        {
          text += "<p>" + ex.Message + "</p>";
          ex = ex.InnerException;
        }

        WebUtil.SetSessionValue("SC_FIDDLE_ERROR", text);
        HttpContext.Current.Response.Redirect("/sitecore/client/Reference/Fiddle/FiddleError");
        return string.Empty;
      }
    }

    /// <summary>
    /// Saves this instance.
    /// </summary>
    public void Save()
    {
      var fileName = this.GetFileName(this.FiddleId);

      this.Save(fileName);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Creates the items.
    /// </summary>
    /// <returns>Returns the items.</returns>
    /// <exception cref="System.InvalidOperationException">The items could not be created since the /sitecore/client/fiddle/PageSettings/Items item is missing
    /// or
    /// The items could not be created since the parent item could not be created</exception>
    [NotNull]
    private Item CreateFiddleRendering()
    {
      var itemRoot = Context.Database.GetItem(ItemsPath);
      if (itemRoot == null)
      {
        throw new InvalidOperationException("The items could not be created since the /sitecore/client/Reference/Fiddle/PageSettings/Items item is missing");
      }

      var fiddleRendering = itemRoot.Add("Fiddle", new TemplateID(TemplateIds.ViewRendering));
      if (fiddleRendering == null)
      {
        throw new InvalidOperationException("The items could not be created since the parent item could not be created");
      }

      return fiddleRendering;
    }

    /// <summary>Creates the item.</summary>
    /// <param name="templates">The templates.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="element">The element.</param>
    private void CreateItem([NotNull] IEnumerable<Template> templates, [NotNull] Item parent, [NotNull] XElement element)
    {
      Diagnostics.Debug.ArgumentNotNull(templates, "templates");
      Diagnostics.Debug.ArgumentNotNull(parent, "parent");
      Diagnostics.Debug.ArgumentNotNull(element, "element");

      var templateName = element.Name.ToString();
      if (templateName == "item")
      {
        templateName = element.GetAttributeValue("tname");

        if (string.IsNullOrEmpty(templateName))
        {
          templateName = element.GetAttributeValue("tid");
        }

        if (string.IsNullOrEmpty(templateName))
        {
          throw new InvalidOperationException(this.FormatException("An element with name 'item' must specify the 'tname' or 'tid' attribute", element));
        }
      }

      Template template;
      if (ID.IsID(templateName))
      {
        var templateId = new ID(templateName);
        template = templates.FirstOrDefault(t => t.ID == templateId);
      }
      else
      {
        var matches = templates.Where(t => string.Compare(t.Name, templateName, StringComparison.InvariantCultureIgnoreCase) == 0).ToList();
        if (matches.Count > 1)
        {
          throw new InvalidOperationException(this.FormatException("The template '{0}' is ambigeous (matches {1} templates)", element, templateName, matches.Count));
        }

        template = matches.FirstOrDefault();
      }

      if (template == null)
      {
        throw new InvalidOperationException(this.FormatException("The template '{0}' was not found", element, templateName));
      }

      var itemName = element.GetAttributeValue("n");
      if (string.IsNullOrEmpty(itemName))
      {
        throw new InvalidOperationException(this.FormatException("The 'n' attribute (the name of the item) must be specified", element));
      }

      var item = parent.Add(itemName, new TemplateID(template.ID));
      if (item == null)
      {
        throw new InvalidOperationException(this.FormatException("The item '{0}' could not be created", element, itemName));
      }

      item.Editing.BeginEdit();

      foreach (var attribute in element.Attributes())
      {
        var fieldName = attribute.Name.ToString();
        if (fieldName == "tname" || fieldName == "tid" || fieldName == "n")
        {
          continue;
        }

        item[fieldName] = attribute.Value;
      }

      item.Editing.EndEdit();

      foreach (var child in element.Elements())
      {
        this.CreateItem(templates, item, child);
      }
    }

    /// <summary>Creates the items.</summary>
    /// <param name="fiddleRendering">The fiddle rendering.</param>
    /// <param name="root">The root.</param>
    /// <param name="viewFileName">Name of the view file.</param>
    private void CreateItems([NotNull] Item fiddleRendering, [NotNull] XElement root, [NotNull] string viewFileName)
    {
      Diagnostics.Debug.ArgumentNotNull(fiddleRendering, "fiddleRendering");
      Diagnostics.Debug.ArgumentNotNull(root, "root");
      Diagnostics.Debug.ArgumentNotNull(viewFileName, "viewFileName");

      var model = this.GetModel(viewFileName);
      var parametersTemplate = this.GetParametersTemplate(root);

      fiddleRendering.Editing.BeginEdit();

      fiddleRendering["Path"] = viewFileName;
      if (!string.IsNullOrEmpty(model))
      {
        fiddleRendering["Model"] = model;
      }

      if (!string.IsNullOrEmpty(parametersTemplate))
      {
        fiddleRendering["Parameters Template"] = parametersTemplate;
      }

      fiddleRendering.Editing.EndEdit();

      var items = this.Items.Trim();
      if (string.IsNullOrEmpty(items))
      {
        return;
      }

      var doc = XDocument.Parse(items, LoadOptions.SetLineInfo);
      var itemsRoot = doc.Root;
      if (itemsRoot == null)
      {
        return;
      }

      var templates = TemplateManager.GetTemplates(Context.Database).Values;

      this.CreateItem(templates, fiddleRendering, itemsRoot);
    }

    /// <summary>Formats the exception.</summary>
    /// <param name="text">The text.</param>
    /// <param name="element">The element.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>Returns the exception.</returns>
    [NotNull]
    private string FormatException([NotNull] string text, [NotNull] XElement element, [NotNull] params object[] args)
    {
      Diagnostics.Debug.ArgumentNotNull(text, "text");
      Diagnostics.Debug.ArgumentNotNull(element, "element");
      Diagnostics.Debug.ArgumentNotNull(args, "args");
      IXmlLineInfo lineInfo = element;
      return string.Format(text, args) + string.Format(" [at {0}, {1}]", lineInfo.LinePosition, lineInfo.LineNumber);
    }

    /// <summary>Gets the name of the file.</summary>
    /// <param name="path">The identifier.</param>
    /// <returns>Returns the string.</returns>
    [NotNull]
    private string GetFileName([NotNull] string path)
    {
      Diagnostics.Debug.ArgumentNotNull(path, "path");

      var fileName = FileUtil.MapPath(RootFolder);
      fileName = Path.Combine(fileName, path);
      fileName = Path.ChangeExtension(fileName, "xml");

      return fileName;
    }

    /// <summary>Gets the model.</summary>
    /// <param name="viewFileName">Name of the view file.</param>
    /// <returns>Returns the model.</returns>
    [NotNull]
    private string GetModel([NotNull] string viewFileName)
    {
      Diagnostics.Debug.ArgumentNotNull(viewFileName, "viewFileName");

      if (!FileUtil.Exists(viewFileName))
      {
        return string.Empty;
      }

      var lines = File.ReadAllLines(FileUtil.MapPath(viewFileName));
      foreach (var l in lines)
      {
        var line = l.Trim();

        if (!line.StartsWith("@model"))
        {
          continue;
        }

        var result = line.Mid(7).Trim();

        var type = ReflectionUtil.GetTypeInfo(result);
        if (type != null)
        {
          result = ReflectionUtil.GetTypeString(type);
        }

        return result;
      }

      return string.Empty;
    }

    /// <summary>Gets the parameters template.</summary>
    /// <param name="root">The root.</param>
    /// <returns>Returns the parameters template.</returns>
    [NotNull]
    private string GetParametersTemplate([NotNull] XElement root)
    {
      Diagnostics.Debug.ArgumentNotNull(root, "root");

      var element = root.XPathSelectElement("//Fiddle");
      if (element == null)
      {
        return string.Empty;
      }

      return element.GetAttributeValue("ParametersTemplate");
    }

    /// <summary>Loads from file.</summary>
    /// <param name="fileName">Name of the file.</param>
    private void LoadFromFile([NotNull] string fileName)
    {
      Diagnostics.Debug.ArgumentNotNull(fileName, "fileName");

      XDocument doc;
      try
      {
        doc = XDocument.Load(fileName);
      }
      catch
      {
        return;
      }

      var root = doc.Root;
      if (root == null)
      {
        return;
      }

      this.Code = root.GetElementValue("code");
      this.CodeLanguage = root.Element("code").GetAttributeValue("language");
      this.View = root.GetElementValue("view");
      this.Next = root.GetElementValue("next");
      this.Previous = root.GetElementValue("previous");
      this.Renderings = root.GetElementValue("renderings");
      this.Items = root.GetElementValue("items");
      this.ReadOnly = root.GetElementValue("readonly") == "true";
      this.Text = root.GetElementValue("text");
    }

    /// <summary>Parses the layout.</summary>
    /// <param name="root">The root.</param>
    /// <param name="fiddleRendering">The data source.</param>
    /// <returns>Returns the string.</returns>
    /// <exception cref="System.Exception">The Layout Renderings field is not well-formed.
    /// or
    /// The Layout Renderings field is not well-formed.</exception>
    [CanBeNull]
    private string ParseLayout([NotNull] XElement root, [NotNull] Item fiddleRendering)
    {
      Diagnostics.Debug.ArgumentNotNull(root, "root");
      Diagnostics.Debug.ArgumentNotNull(fiddleRendering, "fiddleRendering");

      var writer = new StringWriter();
      var output = new XmlTextWriter(writer)
      {
        Formatting = Formatting.Indented
      };

      output.WriteStartElement("r");

      this.ParseLayoutDevice(output, fiddleRendering, root);

      output.WriteEndElement();

      return writer.ToString();
    }

    /// <summary>Parses the layout.</summary>
    /// <param name="output">The output.</param>
    /// <param name="fiddleRendering">The data source.</param>
    /// <param name="root">The root.</param>
    private void ParseLayoutDevice([NotNull] XmlTextWriter output, [NotNull] Item fiddleRendering, [NotNull] XElement root)
    {
      Diagnostics.Debug.ArgumentNotNull(output, "output");
      Diagnostics.Debug.ArgumentNotNull(fiddleRendering, "fiddleRendering");
      Diagnostics.Debug.ArgumentNotNull(root, "root");

      var layoutItemPath = root.GetAttributeValue("Path");
      if (string.IsNullOrEmpty(layoutItemPath))
      {
        layoutItemPath = "/sitecore/client/Reference/Fiddle/PageSettings/Layouts/Layouts/Fiddle-Layout";
      }

      var item = fiddleRendering.Database.GetItem(layoutItemPath);
      Assert.IsNotNull(item, "Layout not found");

      var renderings = fiddleRendering.Database.SelectItems("fast://*[@@templateid='{99F8905D-4A87-4EB8-9F8B-A9BEBFB3ADD6}']");

      output.WriteStartElement("d");
      output.WriteAttributeString("id", Context.Device.ID.ToString());
      output.WriteAttributeString("l", item.ID.ToString());

      foreach (var element in root.Elements())
      {
        this.ParseRendering(output, renderings, fiddleRendering, element);
      }

      output.WriteEndElement();
    }

    /// <summary>Parses the rendering.</summary>
    /// <param name="output">The output.</param>
    /// <param name="renderings">The renderings.</param>
    /// <param name="fiddleRendering">The database.</param>
    /// <param name="element">The element.</param>
    /// <exception cref="System.InvalidOperationException">Ambigeous rendering name:  + name</exception>
    private void ParseRendering([NotNull] XmlTextWriter output, [NotNull] Item[] renderings, [NotNull] Item fiddleRendering, [NotNull] XElement element)
    {
      Diagnostics.Debug.ArgumentNotNull(output, "output");
      Diagnostics.Debug.ArgumentNotNull(renderings, "renderings");
      Diagnostics.Debug.ArgumentNotNull(fiddleRendering, "fiddleRendering");
      Diagnostics.Debug.ArgumentNotNull(element, "element");

      var name = element.Name.ToString();
      if (name == "r")
      {
        output.WriteStartElement("r");

        foreach (var attribute in element.Attributes())
        {
          output.WriteAttributeString(attribute.Name.ToString(), attribute.Value);
        }

        output.WriteEndElement();
        return;
      }

      string renderingId;
      var par = new UrlString();

      var ph = element.GetAttributeValue("placeholder");
      var ds = element.GetAttributeValue("datasource");

      if (string.Compare(name, "Fiddle", StringComparison.InvariantCultureIgnoreCase) == 0)
      {
        // <r id="{4B77BB29-C937-4620-A6CC-A14B1A38DE8A}" par="Id=RenderView1&amp;Path=%2fsitecore%2fshell%2fclient%2fSpeakStuff%2fContent%2fSpeakFiddle%2fSpeakFiddle.cshtml" ph="Page.Body" uid="{8686AEEB-6B16-4DE6-BD13-948BEF4CE2D8}" />
        renderingId = fiddleRendering.ID.ToString();
        par["Id"] = "Fiddle";

        var child = fiddleRendering.Children.FirstOrDefault();
        if (child != null)
        {
          ds = child.ID.ToString();
        }
      }
      else
      {
        var matches = renderings.Where(r => string.Compare(r.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0).ToList();
        if (matches.Count > 1)
        {
          throw new InvalidOperationException("Ambigeous rendering name: " + name);
        }

        var item = matches.FirstOrDefault();
        Assert.IsNotNull(item, "Rendering not found:" + name);

        renderingId = item.ID.ToString();
      }

      output.WriteStartElement("r");
      output.WriteAttributeString("id", renderingId);

      if (!string.IsNullOrEmpty(ph))
      {
        output.WriteAttributeString("ph", ph);
      }

      if (!string.IsNullOrEmpty(ds))
      {
        output.WriteAttributeString("ds", ds);
      }

      foreach (var attribute in element.Attributes())
      {
        var attributeName = attribute.Name.ToString();

        if (string.Compare(attributeName, "placeholder", StringComparison.InvariantCultureIgnoreCase) == 0)
        {
          continue;
        }

        if (string.Compare(attributeName, "datasource", StringComparison.InvariantCultureIgnoreCase) == 0)
        {
          continue;
        }

        if (string.Compare(attributeName, "parameterstemplate", StringComparison.InvariantCultureIgnoreCase) == 0)
        {
          continue;
        }

        par[attributeName] = attribute.Value;
      }

      output.WriteAttributeString("par", par.ToString());

      output.WriteEndElement();
    }

    /// <summary>Saves the specified file name.</summary>
    /// <param name="fileName">Name of the file.</param>
    private void Save([NotNull] string fileName)
    {
      Diagnostics.Debug.ArgumentNotNull(fileName, "fileName");

      var writer = new StringWriter();
      var output = new XmlTextWriter(writer)
      {
        Formatting = Formatting.Indented
      };

      var codeLanguage = this.CodeLanguage == "ts" ? "ts" : "js";

      output.WriteStartElement("fiddle");

      output.WriteStartElement("code");
      output.WriteAttributeString("language", codeLanguage);
      output.WriteCData(this.Code);
      output.WriteEndElement();

      output.WriteStartElement("view");
      output.WriteCData(this.View);
      output.WriteEndElement();

      if (!string.IsNullOrEmpty(this.Next))
      {
        output.WriteElementString("next", this.Next);
      }

      if (!string.IsNullOrEmpty(this.Previous))
      {
        output.WriteElementString("previous", this.Previous);
      }

      output.WriteStartElement("renderings");
      output.WriteCData(this.Renderings);
      output.WriteEndElement();

      output.WriteStartElement("items");
      output.WriteCData(this.Items);
      output.WriteEndElement();

      if (this.ReadOnly)
      {
        output.WriteElementString("readonly", "true");
      }

      if (!string.IsNullOrEmpty(this.Text))
      {
        output.WriteStartElement("text");
        output.WriteCData(this.Text);
        output.WriteEndElement();
      }

      output.WriteEndElement();

      var folder = Path.GetDirectoryName(fileName);
      if (string.IsNullOrEmpty(folder))
      {
        return;
      }

      Directory.CreateDirectory(folder);
      File.WriteAllText(fileName, writer.ToString(), Encoding.UTF8);
    }

    /// <summary>Writes the code.</summary>
    /// <param name="codeFileName">Name of the code file.</param>
    private void WriteCode([NotNull] string codeFileName)
    {
      Diagnostics.Debug.ArgumentNotNull(codeFileName, "codeFileName");

      if (this.CodeLanguage != "ts")
      {
        File.WriteAllText(codeFileName, this.Code, Encoding.UTF8);
        return;
      }

      if (string.IsNullOrEmpty(this.TypeScriptCompilerPath))
      {
        throw new InvalidOperationException("TypeScript compiler path is not set. Try changing the path in the FiddleRun.cshtml file");
      }

      if (!File.Exists(this.TypeScriptCompilerPath))
      {
        throw new InvalidOperationException(string.Format("TypeScript compiler not found in \"{0}\". Try changing the path in the FiddleRun.cshtml file", this.TypeScriptCompilerPath));
      }

      var typeScriptFileName = Path.ChangeExtension(codeFileName, "ts");
      File.WriteAllText(typeScriptFileName, this.Code, Encoding.UTF8);

      var info = new ProcessStartInfo(this.TypeScriptCompilerPath)
      {
        CreateNoWindow = true,
        WorkingDirectory = FileUtil.MapPath("/"),
        Arguments = string.Format("\"sitecore/shell/client/Speak/Assets/lib/core/1.2/Sitecore.Speak.d.ts\" \"sitecore/shell/client/Reference/Assets/typings/jquery/jquery.d.ts\" \"{0}\" -m amd", typeScriptFileName),
        UseShellExecute = false,
        RedirectStandardError = true
      };

      var process = Process.Start(info);
      if (process == null)
      {
        throw new InvalidOperationException("TypeScript compiler failed to start");
      }

      if (!process.WaitForExit(30 * 1000))
      {
        throw new InvalidOperationException("TypeScript compiler timed out");
      }

      if (process.ExitCode == 0 && File.Exists(codeFileName))
      {
        return;
      }

      var errors = process.StandardError.ReadToEnd();
      throw new InvalidOperationException("TypeScript compiler failed to create output: " + errors);
    }

    #endregion
  }
}