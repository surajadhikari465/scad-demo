using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGenerator.Models
{
    public class ConfigDataModel : IConfigDataModel
    {
        public ConfigDataModel()
        {
            Environment = string.Empty;
            Servers = new List<string>();
            RemoteServiceSearchTerms = new List<string>();
            EsbEnvironments = new List<IEsbEnvironmentDefinition>();
        }

        public ConfigDataModel(string environment, List<string> servers, List<string> remoteServiceSearchTerms, List<IEsbEnvironmentDefinition> esbEnvironments)
            : this()
        {
            Environment = environment;
            Servers = servers;
            RemoteServiceSearchTerms = remoteServiceSearchTerms;
            EsbEnvironments = esbEnvironments;
        }

        public string Environment { get; set; }
        public List<string> Servers { get; set; }
        public List<string> RemoteServiceSearchTerms { get; set; }
        public List<IEsbEnvironmentDefinition> EsbEnvironments { get; set; }
    }
}
