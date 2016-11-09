using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetNutriFactsQuery : IQuery<List<NutriFacts>>
    {
        public string ScanCode { get; set; }
    }
}
