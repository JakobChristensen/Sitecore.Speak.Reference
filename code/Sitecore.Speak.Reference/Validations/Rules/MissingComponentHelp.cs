// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingComponentHelp.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Validations.Rules
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  /// <summary>The parameters template.</summary>
  public class MissingComponentHelp : BaseValidation
  {
    #region Public Methods and Operators

    /// <summary>Checks the specified output.</summary>
    /// <param name="output">The output.</param>
    /// <param name="item">The item.</param>
    public override void Check(ValidationAnalyzer output, Item item)
    {
      Assert.ArgumentNotNull(output, "output");
      Assert.ArgumentNotNull(item, "item");

      output.MaxMessages += 2;

      if (string.IsNullOrEmpty(item.Help.ToolTip))
      {
        output.Write(SeverityLevel.Suggestion, "Control must have short help text", string.Format("The control '{0}' does not have a short help text. The short help text is part of the documentation and is displayed in the Documentation web site", item.Name), "Write a short help text.", item);
      }

      if (string.IsNullOrEmpty(item.Help.Text))
      {
        output.Write(SeverityLevel.Suggestion, "Control must have long help text", string.Format("The control '{0}' does not have a long help text. The long help text is part of the documentation and is displayed in the Documentation web site", item.Name), "Write a long help text.", item);
      }
    }

    #endregion
  }
}