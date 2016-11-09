using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Mvc.Excel.ModelMappers
{
    public interface IExcelModelMapper<T, U>
    {
        IEnumerable<U> Map(IEnumerable<T> model);
    }
}
