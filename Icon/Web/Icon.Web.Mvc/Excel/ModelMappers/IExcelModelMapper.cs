using System.Collections.Generic;

namespace Icon.Web.Mvc.Excel.ModelMappers
{
    public interface IExcelModelMapper<T, U>
    {
        IEnumerable<U> Map(IEnumerable<T> model);
    }
}
