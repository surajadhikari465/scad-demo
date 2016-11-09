using PushController.DataAccess.Interfaces;
using PushController.Common.Models;
using System;
using System.Collections.Generic;

namespace PushController.DataAccess.Queries
{
    public class GetCurrentLinkedScanCodesQuery : IQuery<List<CurrentLinkedScanCodeModel>>
    {
        public List<Tuple<string, int>> ScanCodesByBusinessUnit { get; set; }
    }
}
