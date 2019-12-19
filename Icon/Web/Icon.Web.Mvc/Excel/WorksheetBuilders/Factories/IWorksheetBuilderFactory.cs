using System.Collections.Generic;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders.Factories
{
    public interface IWorksheetBuilderFactory<T>
    {
        IEnumerable<IWorksheetBuilder> CreateWorksheetBuilders();
    }
}