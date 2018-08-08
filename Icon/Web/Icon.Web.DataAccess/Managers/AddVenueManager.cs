using Icon.Web.DataAccess.Models;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class AddVenueManager
    {
        public AddVenueManager()
        {
        }

        public AddVenueManager(VenueModel venueModel)
        {
            LocaleName = venueModel.LocaleName;
            ParentLocaleId = venueModel.ParentLocaleId;
            LocaleTypeId = venueModel.LocaleTypeId;
            OpenDate = venueModel.OpenDate;
            CloseDate = venueModel.CloseDate;
            LocaleSubType = venueModel.LocaleSubType;
            VenueCode = venueModel.VenueCode;
            VenueOccupant = venueModel.VenueOccupant;
            LocaleSubTypeId = venueModel.LocaleSubTypeId;
            UserName = venueModel.UserName;
        }

        public int LocaleId { get; set; }
        public string LocaleName { get; set; }
        public int? ParentLocaleId { get; set; }
        public int? LocaleTypeId { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string LocaleSubType { get; set; }
        public string VenueCode { get; set; }
        public string VenueOccupant { get; set; }
        public int LocaleSubTypeId { get; set; }
        public string UserName { get; set; }
        public int OwnerOrgPartyId { get; set; }
    }
}