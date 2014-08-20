// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingDotInParameterHelp.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Validations.Rules
{
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  /// <summary>The parameters template.</summary>
  public class MissingDotInParameterHelp : BaseValidation
  {
    #region Fields

    /// <summary>The parameters template field id.</summary>
    public static readonly ID ParametersTemplateFieldId = new ID("{7D24E54F-5C16-4314-90C9-6051AA1A7DA1}");

    #endregion

    #region Public Methods and Operators

    /// <summary>Checks the specified output.</summary>
    /// <param name="output">The output.</param>
    /// <param name="item">The item.</param>
    public override void Check(ValidationAnalyzer output, Item item)
    {
      Assert.ArgumentNotNull(output, "output");
      Assert.ArgumentNotNull(item, "item");

      var parametersTemplateId = item[ParametersTemplateFieldId];
      if (string.IsNullOrEmpty(parametersTemplateId))
      {
        return;
      }

      var parameterTemplateItem = item.Database.GetItem(parametersTemplateId);
      if (parameterTemplateItem == null)
      {
        return;
      }

      var template = new TemplateItem(parameterTemplateItem);

      foreach (var templateFieldItem in template.Fields)
      {
        var templateItem = new TemplateItem(templateFieldItem.InnerItem.Parent.Parent);
        if (!templateItem.BaseTemplates.Any())
        {
          continue;
        }

        output.MaxMessages++;

        var toolTip = templateFieldItem.ToolTip.Trim();
        if (!string.IsNullOrEmpty(toolTip) && !toolTip.EndsWith("."))
        {
          output.Write(SeverityLevel.Suggestion, "Parameter help text must end with a dot", string.Format("The help text for the '{0}' parameter in the control '{1}' must be a complete sentence and end with a dot.", templateFieldItem.Name, item.Name), "Add a dot to the help text.", templateFieldItem.InnerItem);
        }
      }
    }

    #endregion
  }
}