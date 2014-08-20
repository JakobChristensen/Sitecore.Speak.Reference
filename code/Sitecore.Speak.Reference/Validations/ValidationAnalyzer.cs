// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Validations
{
  using System.Collections.Generic;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  /// <summary>Defines the <see cref="ValidationAnalyzer"/> class.</summary>
  public class ValidationAnalyzer
  {
    #region Fields

    /// <summary>The records.</summary>
    private readonly List<Record> messages = new List<Record>();

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the maximum messages.
    /// </summary>
    /// <value>The maximum messages.</value>
    public int MaxMessages { get; set; }

    /// <summary>
    /// Gets the records.
    /// </summary>
    /// <value>The records.</value>
    [NotNull]
    public IEnumerable<Record> Messages
    {
      get
      {
        return this.messages;
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Writes the specified severity.</summary>
    /// <param name="severity">The severity.</param>
    /// <param name="title">The title.</param>
    /// <param name="problem">The problem.</param>
    /// <param name="solution">The solution.</param>
    public void Write(SeverityLevel severity, [NotNull] string title, [NotNull] string problem, [NotNull] string solution)
    {
      Assert.ArgumentNotNull(title, "title");
      Assert.ArgumentNotNull(problem, "problem");
      Assert.ArgumentNotNull(solution, "solution");

      var record = new Record
      {
        Title = title, 
        Problem = problem, 
        Solution = solution, 
        Severity = severity
      };

      this.messages.Add(record);
    }

    /// <summary>Writes the specified severity.</summary>
    /// <param name="severity">The severity.</param>
    /// <param name="title">The title.</param>
    /// <param name="problem">The problem.</param>
    /// <param name="solution">The solution.</param>
    /// <param name="item">The item.</param>
    public void Write(SeverityLevel severity, [NotNull] string title, [NotNull] string problem, [NotNull] string solution, [NotNull] Item item)
    {
      Assert.ArgumentNotNull(title, "title");
      Assert.ArgumentNotNull(problem, "problem");
      Assert.ArgumentNotNull(solution, "solution");
      Assert.ArgumentNotNull(item, "item");

      var record = new Record
      {
        Title = title, 
        Problem = problem, 
        Solution = solution, 
        Severity = severity, 
        Item = item
      };

      this.messages.Add(record);
    }

    #endregion

    #region Methods

    /// <summary>Clears this instance.</summary>
    internal void Clear()
    {
      this.messages.Clear();
    }

    #endregion

    #region Nested type: Record

    /// <summary>Defines the <see cref="Record"/> class.</summary>
    public class Record
    {
      #region Public Properties

      /// <summary>
      /// Gets or sets the external link.
      /// </summary>
      /// <value>The external link.</value>
      [CanBeNull]
      public string ExternalLink { get; set; }

      /// <summary>
      /// Gets or sets the item.
      /// </summary>
      /// <value>The item.</value>
      [CanBeNull]
      public Item Item { get; set; }

      /// <summary>
      /// Gets or sets the problem.
      /// </summary>
      /// <value>The problem.</value>
      [CanBeNull]
      public string Problem { get; set; }

      /// <summary>
      /// Gets or sets the severity.
      /// </summary>
      /// <value>The severity.</value>
      public SeverityLevel Severity { get; set; }

      /// <summary>
      /// Gets or sets the solution.
      /// </summary>
      /// <value>The solution.</value>
      [CanBeNull]
      public string Solution { get; set; }

      /// <summary>
      /// Gets or sets the title.
      /// </summary>
      /// <value>The title.</value>
      [CanBeNull]
      public string Title { get; set; }

      #endregion
    }

    #endregion
  }
}