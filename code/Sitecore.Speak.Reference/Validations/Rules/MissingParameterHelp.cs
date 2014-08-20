// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingParameterHelp.cs" company="Sitecore A/S">
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
  public class MissingParameterHelp : BaseValidation
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

        if (string.IsNullOrEmpty(templateFieldItem.ToolTip))
        {
          output.Write(SeverityLevel.Suggestion, "Parameter must have help text", string.Format("The parameter '{0}' in the '{1}' control does not have a short help text. The short help text is part of documentation and is displayed in the Documentation web site", templateFieldItem.Name, item.Name), "Write a help text.", templateFieldItem.InnerItem);
        }
      }
    }

    #endregion
  }
}