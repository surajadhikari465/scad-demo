namespace Testing.Core.Models.Irma
{
    using System;
    using System.Collections.Generic;

    public partial class StoreJurisdiction
    {
        public int StoreJurisdictionID { get; set; }
        public string StoreJurisdictionDesc { get; set; }
        public Nullable<int> CurrencyID { get; set; }
    }
}
