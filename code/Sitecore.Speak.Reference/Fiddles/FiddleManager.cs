// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FiddleManager.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Fiddles
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Web;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.IO;
  using Sitecore.Web;

  /// <summary>
  /// Defines the FiddleManager class.
  /// </summary>
  public static class FiddleManager
  {
    #region Public Methods and Operators

    /// <summary>Creates the fiddle based on component.</summary>
    /// <param name="componentId">The component identifier.</param>
    /// <returns>Returns the fiddle based on component.</returns>
    [NotNull]
    public static Fiddle CreateFiddleBasedOnComponent([NotNull] string componentId)
    {
      Assert.ArgumentNotNull(componentId, "componentId");

      var item = Context.Database.GetItem(componentId);

      var fiddle = CreateNewFiddle();
      fiddle.CreateBasedOnComponent(item);

      return fiddle;
    }

    /// <summary>Creates the fiddle from template.</summary>
    /// <param name="templateId">The template identifier.</param>
    /// <returns>Returns the fiddle from template.</returns>
    [NotNull]
    public static Fiddle CreateFiddleFromTemplate([NotNull] string templateId)
    {
      Assert.ArgumentNotNull(templateId, "templateId");

      var fiddle = CreateNewFiddle();
      fiddle.CreateFromTemplate(templateId);

      return fiddle;
    }

    /// <summary>Creates the fiddle from upload.</summary>
    /// <param name="file">The file.</param>
    /// <returns>Returns the fiddle from upload.</returns>
    [NotNull]
    public static Fiddle CreateFiddleFromUpload([NotNull] HttpPostedFileBase file)
    {
      Assert.ArgumentNotNull(file, "file");

      var fiddle = CreateNewFiddle();
      fiddle.CreateFromFileUpload(file);

      return fiddle;
    }

    /// <summary>Creates the fiddle using component.</summary>
    /// <param name="componentId">The component identifier.</param>
    /// <returns>Returns the fiddle using component.</returns>
    [NotNull]
    public static Fiddle CreateFiddleUsingComponent([NotNull] string componentId)
    {
      Assert.ArgumentNotNull(componentId, "componentId");

      var item = Context.Database.GetItem(componentId);

      var fiddle = CreateNewFiddle();
      fiddle.CreateUsingComponent(item);

      return fiddle;
    }

    /// <summary>Forks this instance.</summary>
    /// <param name="fiddle">The fiddle.</param>
    public static void Fork([NotNull] Fiddle fiddle)
    {
      Assert.ArgumentNotNull(fiddle, "fiddle");

      var fiddleId = GetNewFiddleId();
      fiddle.Fork(fiddleId);

      fiddle.Save();
    }

    /// <summary>
    /// Loads the fiddle from post.
    /// </summary>
    /// <returns>Returns the fiddle.</returns>
    [NotNull]
    public static Fiddle LoadFiddleFromPost()
    {
      var fiddleId = GetFiddleId();
      return LoadFiddleFromPost(fiddleId);
    }

    /// <summary>
    /// Gets the fiddle.
    /// </summary>
    /// <returns>Returns the fiddle.</returns>
    [NotNull]
    public static Fiddle LoadFiddleFromStorage()
    {
      var componentId = WebUtil.GetQueryString("c");
      if (!string.IsNullOrEmpty(componentId))
      {
        return LoadFromComponent(componentId);
      }

      var fiddleId = GetFiddleId();
      return LoadFiddleFromStorage(fiddleId);
    }

    /// <summary>
    /// Loads the templates.
    /// </summary>
    /// <returns>Returns the templates.</returns>
    [NotNull]
    public static IEnumerable<string> LoadTemplates()
    {
      var folder = FileUtil.MapPath("/sitecore/shell/client/Reference/Fiddles/Templates");

      return Directory.GetFiles(folder, "*.xml", SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension).OrderBy(f => f);
    }

    /// <summary>Saves this instance.</summary>
    /// <param name="fiddle">The fiddle.</param>
    public static void Save([NotNull] Fiddle fiddle)
    {
      Assert.ArgumentNotNull(fiddle, "fiddle");

      fiddle.Save();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Creates the new fiddle.
    /// </summary>
    /// <returns>Returns the new fiddle.</returns>
    [NotNull]
    private static Fiddle CreateNewFiddle()
    {
      var fiddleId = GetNewFiddleId();

      return new Fiddle(fiddleId);
    }

    /// <summary>
    /// Gets the fiddle identifier.
    /// </summary>
    /// <returns>Returns the string.</returns>
    [NotNull]
    private static string GetFiddleId()
    {
      var id = WebUtil.GetFormValue("fiddleid");
      if (!string.IsNullOrEmpty(id))
      {
        return id;
      }

      id = WebUtil.GetQueryString("f");
      if (!string.IsNullOrEmpty(id))
      {
        return id;
      }

      return GetNewFiddleId();
    }

    /// <summary>
    /// Gets the new fiddle identifier.
    /// </summary>
    /// <returns>Returns the new fiddle identifier.</returns>
    [NotNull]
    private static string GetNewFiddleId()
    {
      var id = ID.NewID.ToShortID().ToString();

      var path = "Fiddle/" + id;

      return path;
    }

    /// <summary>Loads the fiddle from storage.</summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Returns the fiddle.</returns>
    [NotNull]
    private static Fiddle LoadFiddleFromPost([NotNull] string id)
    {
      Debug.ArgumentNotNull(id, "id");

      var fiddle = new Fiddle(id);
      fiddle.LoadFromPost();

      return fiddle;
    }

    /// <summary>Loads the fiddle.</summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Returns the fiddle.</returns>
    [NotNull]
    private static Fiddle LoadFiddleFromStorage([NotNull] string id)
    {
      Debug.ArgumentNotNull(id, "id");

      var fiddle = new Fiddle(id);
      fiddle.LoadFromStorage();

      return fiddle;
    }

    /// <summary>Loads from component.</summary>
    /// <param name="componentId">The component identifier.</param>
    /// <returns>Returns the from component.</returns>
    [NotNull]
    private static Fiddle LoadFromComponent([NotNull] string componentId)
    {
      Debug.ArgumentNotNull(componentId, "componentId");

      var fileName = FileUtil.MapPath("/sitecore/shell/client/reference/Fiddles/Components/" + componentId);
      if (!File.Exists(fileName))
      {
        var fiddleId = GetFiddleId();
        return new Fiddle(fiddleId);
      }

      return CreateNewFiddle();
    }

    #endregion
  }
}