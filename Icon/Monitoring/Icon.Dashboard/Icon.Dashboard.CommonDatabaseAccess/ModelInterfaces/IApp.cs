using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IApp
    {
        int AppID { get; set; }
        string AppName { get; set; }        
    }
}
