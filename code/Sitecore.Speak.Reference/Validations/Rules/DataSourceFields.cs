// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceFields.cs" company="">
//   
// </copyright>
// <summary>
//   The parameters template.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Validations.Rules
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  /// <summary>The parameters template.</summary>
  public class DataSourceFields : BaseValidation
  {
    #region Static Fields

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

      if (item["Datasource Location"] != "PageSettings")
      {
        output.Write(SeverityLevel.Suggestion, "The 'Datasource Location' should be set to 'PageSettings'", string.Format("The 'Datasource Location' field is currently set to '{0}'. When creating data sources for a SPEAK component, it should be created in the PageSettings section of the page.", item["Datasource Location"]), "Set 'Datasource Location' field to 'PageSettings'", item);
      }

      if (item["Datasource Template"] != parametersTemplateId)
      {
        output.Write(SeverityLevel.Suggestion, "The 'Datasource Template' should be set to the Parameters Template item ID", string.Format("The 'Datasource Template' field is currently set to '{0}'. When creating data sources for a SPEAK component, it should probably create an item with same template as the Parameters Template.", item["Datasource Template"]), string.Format("Set 'Datasource Template' field to '{0}'", parametersTemplateId), item);
      }
    }

    #endregion
  }
}