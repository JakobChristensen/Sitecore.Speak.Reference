// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseValidation.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Validations
{
  using Sitecore.Data.Items;

  /// <summary>
  /// Class BaseValidation.
  /// </summary>
  public abstract class BaseValidation
  {
    #region Public Methods and Operators

    /// <summary>Checks the specified output.</summary>
    /// <param name="output">The output.</param>
    /// <param name="item">The item.</param>
    public abstract void Check([NotNull] ValidationAnalyzer output, [NotNull] Item item);

    #endregion
  }
}