using OpsgenieAlert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.Common
{
    public interface IOpsgenieTrigger
    {
        OpsgenieResponse TriggerAlert(string message, string description = "", Dictionary<string, string> details = null);
    }
}
