using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Common
{
    public interface IGlobalControllerSettings
    {
        bool SendRetailUomChangeEmailAlerts { get; set; }
        string[] GlobalEvents { get; set; }
        int DbContextConnectionTimeout { get; set; }
        int MaxQueueEntriesToProcess { get; set; }
        string EmailSubjectEnvironment { get; set; }
        bool EnableInforUpdates { get; set; }
    }
}
