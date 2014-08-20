// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidDefaultParameter.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Validations.Rules
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;
  using Sitecore.Text;

  /// <summary>The parameters template.</summary>
  public class InvalidDefaultParameter : BaseValidation
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

      var template = TemplateManager.GetTemplate(new ID(parametersTemplateId), item.Database);
      if (template == null)
      {
        return;
      }

      var defaultParameters = new UrlString(item["Default Parameters"]);

      foreach (string key in defaultParameters.Parameters.Keys)
      {
        if (string.IsNullOrEmpty(key))
        {
          continue;
        }

        if (key == "Placeholder")
        {
          continue;
        }

        output.MaxMessages++;

        if (template.GetField(key) == null)
        {
          output.Write(SeverityLevel.Warning, "Control has an invalid default parameter", string.Format("The control '{1}' defines the default parameter '{0}', but this parameter does not exist in the controls Parameter Template.", key, item.Name), "Remove the default parameter or add it to the Parameter Template.", item);
        }
      }
    }

    #endregion
  }
}