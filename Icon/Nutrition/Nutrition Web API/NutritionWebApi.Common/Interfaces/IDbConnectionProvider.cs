using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace NutritionWebApi.Common
{
    public interface IDbConnectionProvider
    {
        IDbConnection Connection { get; set; }
    }
}
