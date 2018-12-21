using System.Collections.Generic;
using Icon.Dashboard.DataFileAccess.Models;

namespace Icon.Dashboard.DataFileGenerator.Models
{
    public interface IConfigDataModel
    {
        string Environment { get; set; }
        List<IEsbEnvironmentDefinition> EsbEnvironments { get; set; }
        List<string> Servers { get; set; }
    }
}