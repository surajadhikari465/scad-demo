using System;

namespace Icon.Web.DataAccess.Commands
{
    public class AddLocaleCommand
    {
        public int LocaleId { get; set; }
        public string LocaleName { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public int? LocaleParentId { get; set; }
        public int LocaleTypeId { get; set; }
        public int OwnerOrgPartyId { get; set; }
        public string BusinessUnit { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public string StoreAbbreviation { get; set; }
        public string EwicAgencyId { get; set; }
        public string Fax { get; set; }
        public string IrmaStoreId { get; set; }
        public string StorePosType { get; set; }

        public string UserName { get; set; }
    }
}
