﻿using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Controller.EventOperations
{
    public interface IDataIssueMessageCollector
    {
        List<RegionalItemMessageModel> Message { get; set; }
        void SendDataIssueMessage();
    }
}
