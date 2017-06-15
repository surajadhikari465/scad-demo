using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetTaxAbbreviationQuery : IQuery<string>
    {
        public int HierarchyClassId { get; set; }

        /// <summary>
        /// string used to identify the TaxAbbreviation trait ("ABR")
        /// </summary>
        public string TaxTraitCode
        {
            get
            {
                return TraitCodes.TaxAbbreviation;
            }
        }
    }
}
