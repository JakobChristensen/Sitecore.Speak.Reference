// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationManager.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Validations
{
  using System.Collections.Generic;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Validations.Rules;

  /// <summary>
  /// Class ValidationManager.
  /// </summary>
  public static class ValidationManager
  {
    #region Fields

    /// <summary>
    /// The validations
    /// </summary>
    private static readonly List<BaseValidation> Validations = new List<BaseValidation>();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes static members of the <see cref="ValidationManager"/> class.
    /// </summary>
    static ValidationManager()
    {
      Validations.Add(new ParametersTemplate());
      Validations.Add(new MissingComponentHelp());
      Validations.Add(new MissingDotInControlHelp());
      Validations.Add(new MissingParameterHelp());
      Validations.Add(new MissingDotInParameterHelp());
      Validations.Add(new InvalidDefaultParameter());
      Validations.Add(new DataSourceFields());
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Checks the specified item.</summary>
    /// <param name="item">The item.</param>
    /// <returns>Returns the .</returns>
    [NotNull]
    public static ValidationAnalyzer Check([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      var output = new ValidationAnalyzer();

      foreach (var validation in Validations)
      {
        validation.Check(output, item);
      }

      return output;
    }

    #endregion
  }
}