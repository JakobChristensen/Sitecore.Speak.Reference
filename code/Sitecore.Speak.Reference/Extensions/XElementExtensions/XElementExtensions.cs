// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XElementExtensions.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Sitecore.Extensions.XElementExtensions
{
  using System.ComponentModel;
  using System.Xml.Linq;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Defines the XElementExtensions class.
  /// </summary>
  public static class XElementExtensions
  {
    #region Public Methods and Operators

    /// <summary>Gets the element value.</summary>
    /// <param name="element">The element.</param>
    /// <param name="elementName">Name of the element.</param>
    /// <returns>Returns the string.</returns>
    [NotNull]
    public static string GetElementValue([NotNull] this XElement element, [NotNull] [Localizable(false)] string elementName)
    {
      Assert.ArgumentNotNull(element, "element");
      Assert.ArgumentNotNull(elementName, "elementName");

      var e = element.Element(elementName);

      return e == null ? string.Empty : e.Value;
    }

    #endregion
  }
}