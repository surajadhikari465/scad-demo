using System;
using System.Collections.Generic;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class PostDeploymentScriptHistory
    {
        public string ScriptKey { get; set; }
        public DateTime RunTime { get; set; }
    }
}
