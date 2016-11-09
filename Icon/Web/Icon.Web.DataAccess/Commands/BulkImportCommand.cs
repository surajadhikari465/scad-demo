using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class BulkImportCommand<T>
    {
        public List<T> BulkImportData { get; set; }
        public string UserName { get; set; }
        public bool UpdateAllItemFields { get; set; }
    }
}
