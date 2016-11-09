using System;
using Irma.Framework;
using Mammoth.Price.Controller.DataAccess.Tests;

namespace Irma.Testing.Builders
{
    public class TestStoreJurisdictionBuilder
    {
        private int storeJurisdictionID;
        private string storeJurisdictionDesc;
        private System.Nullable<int> currencyID;

        public TestStoreJurisdictionBuilder()
        {
            this.storeJurisdictionID = 0;
            this.storeJurisdictionDesc = null;
            this.currencyID = null;
        }

        public TestStoreJurisdictionBuilder WithStoreJurisdictionID(int storeJurisdictionID)
        {
            this.storeJurisdictionID = storeJurisdictionID;
            return this;
        }

        public TestStoreJurisdictionBuilder WithStoreJurisdictionDesc(string storeJurisdictionDesc)
        {
            this.storeJurisdictionDesc = storeJurisdictionDesc;
            return this;
        }

        public TestStoreJurisdictionBuilder WithCurrencyID(System.Nullable<int> currencyID)
        {
            this.currencyID = currencyID;
            return this;
        }

        public StoreJurisdiction Build()
        {
            StoreJurisdiction storeJurisdiction = new StoreJurisdiction();

            storeJurisdiction.StoreJurisdictionID = this.storeJurisdictionID;
            storeJurisdiction.StoreJurisdictionDesc = this.storeJurisdictionDesc;
            storeJurisdiction.CurrencyID = this.currencyID;

            return storeJurisdiction;
        }

        public static implicit operator StoreJurisdiction(TestStoreJurisdictionBuilder builder)
        {
            return builder.Build();
        }
    }
}