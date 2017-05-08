
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Irma.Framework
{

using System;
    using System.Collections.Generic;
    
public partial class Store
{

    public Store()
    {

        this.IConPOSPushPublish = new HashSet<IConPOSPushPublish>();

        this.Price = new HashSet<Price>();

        this.Sales_SumByItem = new HashSet<Sales_SumByItem>();

        this.ItemLocaleChangeQueue = new HashSet<MammothItemLocaleChangeQueue>();

        this.StoreSubTeam = new HashSet<StoreSubTeam>();

        this.Users1 = new HashSet<Users>();

        this.Users2 = new HashSet<Users>();

        this.PriceChangeQueue = new HashSet<MammothPriceChangeQueue>();

    }


    public int Store_No { get; set; }

    public string Store_Name { get; set; }

    public string Phone_Number { get; set; }

    public bool Mega_Store { get; set; }

    public bool Distribution_Center { get; set; }

    public bool Manufacturer { get; set; }

    public bool WFM_Store { get; set; }

    public bool Internal { get; set; }

    public string TelnetUser { get; set; }

    public string TelnetPassword { get; set; }

    public int BatchID { get; set; }

    public int BatchRecords { get; set; }

    public Nullable<int> BusinessUnit_ID { get; set; }

    public Nullable<int> Zone_ID { get; set; }

    public string UNFI_Store { get; set; }

    public Nullable<System.DateTime> LastRecvLogDate { get; set; }

    public Nullable<int> LastRecvLog_No { get; set; }

    public Nullable<int> RecvLogUser_ID { get; set; }

    public Nullable<short> EXEWarehouse { get; set; }

    public bool Regional { get; set; }

    public Nullable<System.DateTime> LastSalesUpdateDate { get; set; }

    public string StoreAbbr { get; set; }

    public Nullable<int> PLUMStoreNo { get; set; }

    public Nullable<int> TaxJurisdictionID { get; set; }

    public Nullable<int> POSSystemId { get; set; }

    public Nullable<int> PSI_Store_No { get; set; }

    public Nullable<int> StoreJurisdictionID { get; set; }

    public Nullable<bool> UseAvgCostHistory { get; set; }

    public string GeoCode { get; set; }



    public virtual ICollection<IConPOSPushPublish> IConPOSPushPublish { get; set; }

    public virtual ICollection<Price> Price { get; set; }

    public virtual ICollection<Sales_SumByItem> Sales_SumByItem { get; set; }

    public virtual Users Users { get; set; }

    public virtual ICollection<MammothItemLocaleChangeQueue> ItemLocaleChangeQueue { get; set; }

    public virtual ICollection<StoreSubTeam> StoreSubTeam { get; set; }

    public virtual ICollection<Users> Users1 { get; set; }

    public virtual ICollection<Users> Users2 { get; set; }

    public virtual ICollection<MammothPriceChangeQueue> PriceChangeQueue { get; set; }

}

}
