using Icon.Esb.Schemas.Wfm.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.LocaleListener.Models
{
    public class LocaleModel
    {
       public LocaleModel()
        {

        }

        public LocaleModel(int localeId, string name, int? parentLocaleId, string typeCode, ActionEnum action)
        {
            this.LocaleId = localeId;
            this.Name = name;
            this.ParentLocaleId = parentLocaleId;
            this.TypeCode = typeCode;
            this.Action = action;
        }
        public LocaleModel(int localeId, int? parentLocaleId, int businessUnitId, string name, string typeCode, DateTime openDate,
                         DateTime closeDate, string ewicAgency, ActionEnum action, LocaleAddress address,
                         IEnumerable<LocaleTraitModel> localeTraitModelCollection)
        {
            this.LocaleId = localeId;
            this.ParentLocaleId = parentLocaleId;
            this.BusinessUnitId = businessUnitId;
            this.Name = name;
            this.TypeCode = typeCode;
            this.OpenDate = openDate;
            this.CloseDate = closeDate;
            this.EwicAgency = ewicAgency;
            this.Action = action;
            this.Address = address;
            this.LocaleTraits = localeTraitModelCollection;
        }

        public LocaleModel(int localeId, int? parentLocaleId, int businessUnitId, string name, string typeCode, string ewicAgency, ActionEnum action, LocaleAddress address, 
                           IEnumerable<LocaleTraitModel> localeTraitModelCollection)
        {
            this.LocaleId = localeId;
            this.ParentLocaleId = parentLocaleId;
            this.BusinessUnitId = businessUnitId;
            this.Name = name;
            this.TypeCode = typeCode;   
            this.EwicAgency = ewicAgency;
            this.Action = action;
            this.Address = address;
            this.LocaleTraits = localeTraitModelCollection;
        }
        public int LocaleId { get; set; }
        public int? ParentLocaleId { get; set; }
        public int BusinessUnitId { get; set; }
        public string Name { get; set; }
        public string TypeCode { get; set; }
        public IEnumerable<LocaleModel> Locales { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string EwicAgency { get; set; }
        public ActionEnum Action { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
        public LocaleAddress Address { get; set; }
        public IEnumerable<LocaleTraitModel> LocaleTraits { get; set; }
        public int SequenceId { get; set; }
        public string InforMessageId { get; set; }

    }
}
