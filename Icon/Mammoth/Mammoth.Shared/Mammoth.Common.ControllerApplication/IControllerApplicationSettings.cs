﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.ControllerApplication
{
    public interface IControllerApplicationSettings
    {
        int Instance { get; set; }
        int MaxNumberOfRowsToMark { get; set; }
        string ControllerName { get; set; }
    }
}
