using System;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateVenueCommand
    {
        public int LocaleId { get; set; }
        public string LocaleName { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }     
        public string UserName { get; set; }
        public string LocaleSubType { get; set; }
        public string VenueCode { get; set; }
        public string VenueOccupant { get; set; }
        public int ParentLocaleId { get; set; }
        public int? LocaleTypeId { get; set; }
        public string TouchPointGroupId { get; set; }
    }
}