using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Tests.Queries
{
    /// <summary>
    /// Data loaded from the database for use in testing ItemLocale event queries
    /// </summary>
    public class DbDataCollectionForItemLocaleEventTesting
    {
        public int DefaultJurisdictionId { get; set; }

        // can be null if in region without alt jurisdiction
        public int? AltJurisdictionId { get; set; }

        public List<Store> ValidStores { get; set; }
        public List<int> ValidStoreNumbers
        {
            get
            {
                return ValidStores?.Select(s => s.Store_No).ToList() ?? new List<int>();
            }
        }
        public Store StoreInDefaultJurisdiction { get; set; }
        public Store StoreInAltJurisdictionIfAny { get; set; }

        public LabelType LabelSmall { get; set; }
        public int LabelSmallTypeId => LabelSmall?.LabelType_ID ?? 0;
        public string LabelSmallTypeDesc => LabelSmall?.LabelTypeDesc;

        public SubTeam SubTeam { get; set; }
        public int SubTeamNo => SubTeam?.SubTeam_No ?? 0;

        public ItemOrigin Origin_USA { get; set; }
        public int Origin_USA_Id => Origin_USA?.Origin_ID ?? 0;
        public string Origin_USA_Name => Origin_USA?.Origin_Name;
        public ItemOrigin Origin_CAN { get; set; }
        public int Origin_CAN_Id => Origin_CAN?.Origin_ID ?? 0;
        public string Origin_CAN_Name => Origin_CAN?.Origin_Name;
        public ItemOrigin Origin_FRA { get; set; }
        public int Origin_FRA_Id => Origin_FRA?.Origin_ID ?? 0;
        public string Origin_FRA_Name => Origin_FRA?.Origin_Name;
        public ItemOrigin Origin_ITA { get; set; }
        public int Origin_ITA_Id => Origin_ITA?.Origin_ID ?? 0;
        public string Origin_ITA_Name => Origin_ITA?.Origin_Name;

        public ItemUnit Unit_Each { get; set; }
        public int Unit_Each_Id => Unit_Each?.Unit_ID ?? 0;
        public string Unit_Each_Name => Unit_Each?.Unit_Name;
        public string Unit_Each_Abbrev => Unit_Each?.Unit_Abbreviation;
        public ItemUnit Unit_Case { get; set; }
        public int Unit_Case_Id => Unit_Case?.Unit_ID ?? 0;
        public string Unit_Case_Name => Unit_Case?.Unit_Name;
        public string Unit_Case_Abbrev => Unit_Case?.Unit_Abbreviation;
        public ItemUnit Unit_Lb { get; set; }
        public int Unit_Lb_Id => Unit_Lb?.Unit_ID ?? 0;
        public string Unit_Lb_Name => Unit_Lb?.Unit_Name;
        public string Unit_Lb_Abbrev => Unit_Lb?.Unit_Abbreviation;
        public ItemUnit Unit_Oz { get; set; }
        public int Unit_Oz_Id => Unit_Oz?.Unit_ID ?? 0;
        public string Unit_Oz_Name => Unit_Oz?.Unit_Name;
        public string Unit_Oz_Abbrev => Unit_Oz?.Unit_Abbreviation;
        public ItemUnit Unit_Kg { get; set; }
        public int Unit_Kg_Id => Unit_Kg?.Unit_ID ?? 0;
        public string Unit_Kg_Name => Unit_Kg?.Unit_Name;
        public string Unit_Kg_Abbrev => Unit_Kg?.Unit_Abbreviation;

        public Vendor Vendor { get; set; }
        public int VendorId => Vendor?.Vendor_ID ?? 0;
        public string VendorItemId => Vendor?.Vendor_ID.ToString();
        public string VendorKey => Vendor?.Vendor_Key;
        public string VendorCompanyName => Vendor?.CompanyName;

        public int LinkedItemKey { get; set; }
        public string LinkedItemIdentifier { get; set; }
    }
}
