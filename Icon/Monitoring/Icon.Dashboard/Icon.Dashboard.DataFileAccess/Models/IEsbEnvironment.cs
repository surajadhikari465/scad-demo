﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileAccess.Models
{
    public interface IEsbEnvironment
    {
        string Name { get; set; }
        string ServerUrl { get; set; }
        string TargetHostName { get; set; }
        string JmsUsername { get; set; }
        string JmsPassword { get; set; }
        string JndiUsername { get; set; }
        string JndiPassword { get; set; }

        IList<IconApplicationIdentifier> Applications { get; set; }

        IconApplicationIdentifier AddApplication(string name, string server);
    }
}
