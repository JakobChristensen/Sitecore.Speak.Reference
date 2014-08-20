// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParametersTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Validations.Rules
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  /// <summary>The parameters template.</summary>
  public class ParametersTemplate : BaseValidation
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

      output.MaxMessages += 2;

      if (string.IsNullOrEmpty(parametersTemplateId))
      {
        output.Write(SeverityLevel.Warning, "Control does not have a Parameter Template", string.Format("The control '{0}' does not have a Parameter Template. Parameter Templates define the parameters that the control accepts. These help the user set parameters correctly and appear in Property Windows when configuring the control in Sitecore Rocks or the Page Editor.", item.Name), "Create an Parameter Template item and set the Parameter Template field in the control.", item);
        return;
      }

      var parameterTemplate = item.Database.GetItem(parametersTemplateId);
      if (parameterTemplate == null)
      {
        output.Write(SeverityLevel.Error, "Parameter Template does not exist", string.Format("The control '{0}' points to a Parameter Templates item that does not exist.", item.Name), "Create an Parameter Template item or remove the reference.", item);
      }
    }

    #endregion
  }
}