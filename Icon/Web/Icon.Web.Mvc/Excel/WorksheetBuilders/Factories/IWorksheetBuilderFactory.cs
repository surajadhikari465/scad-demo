using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders.Factories
{
    public interface IWorksheetBuilderFactory<T>
    {
        IEnumerable<IWorksheetBuilder> CreateWorksheetBuilders();
    }
}