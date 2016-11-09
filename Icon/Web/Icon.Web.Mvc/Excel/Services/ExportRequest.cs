using System.Collections.Generic;

namespace Icon.Web.Mvc.Excel.Services
{
    public class ExportRequest<T>
    {
        public List<T> Rows { get; set; }
        public bool CreateTemplate  { get; set; }

        public ExportRequest()
        {
            Rows = new List<T>();
        }
    }
}