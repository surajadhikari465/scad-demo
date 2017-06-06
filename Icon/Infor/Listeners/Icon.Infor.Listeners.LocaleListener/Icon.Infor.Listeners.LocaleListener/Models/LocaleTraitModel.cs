using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.LocaleListener.Models
{
    public class LocaleTraitModel
    {
        public  LocaleTraitModel(int traitId,string traitValue,string uomId, int? businessUnitId)
        {
            TraitId = traitId;
            TraitValue = traitValue;
            UomId = uomId;
            BusinessUnitId = businessUnitId;
        }
        public int TraitId { get; set; }
        public string TraitValue { get; set; }
        public string UomId { get; set; }
        public int? BusinessUnitId { get; set; }
    }
}
