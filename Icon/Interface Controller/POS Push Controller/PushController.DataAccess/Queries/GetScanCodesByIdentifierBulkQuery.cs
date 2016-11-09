using PushController.Common.Models;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace PushController.DataAccess.Queries
{
    public class GetScanCodesByIdentifierBulkQuery : IQuery<List<ScanCodeModel>>
    {
        public List<string> Identifiers { get; set; }
    }
}
