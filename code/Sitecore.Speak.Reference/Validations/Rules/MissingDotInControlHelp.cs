// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingDotInControlHelp.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Validations.Rules
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  /// <summary>The parameters template.</summary>
  public class MissingDotInControlHelp : BaseValidation
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

      if (!string.IsNullOrEmpty(item.Help.ToolTip) && !item.Help.ToolTip.EndsWith("."))
      {
        output.Write(SeverityLevel.Suggestion, "Control short help text must end with a dot", string.Format("The short help text for the control '{0}' must be a complete sentence and end with a dot.", item.Name), "Add a dot to the short help text.", item);
      }

      if (!string.IsNullOrEmpty(item.Help.Text) && !item.Help.Text.EndsWith("."))
      {
        output.Write(SeverityLevel.Suggestion, "Control long help text must end with a dot", string.Format("The long help text for the control '{0}' must be a complete sentence and end with a dot.", item.Name), "Add a dot to the long help text.", item);
      }
    }

    #endregion
  }
}