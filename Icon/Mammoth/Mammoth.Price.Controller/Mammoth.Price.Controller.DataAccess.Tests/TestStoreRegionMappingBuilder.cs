using System;
using Irma.Framework;
using Mammoth.Price.Controller.DataAccess.Tests;

namespace Irma.Testing.Builders
{
    public class TestStoreRegionMappingBuilder
    {
        private int store_No;
        private string region_Code;

        public TestStoreRegionMappingBuilder()
        {
            this.store_No = 0;
            this.region_Code = null;
        }

        public TestStoreRegionMappingBuilder WithStore_No(int store_No)
        {
            this.store_No = store_No;
            return this;
        }

        public TestStoreRegionMappingBuilder WithRegion_Code(string region_Code)
        {
            this.region_Code = region_Code;
            return this;
        }

        public StoreRegionMapping Build()
        {
            StoreRegionMapping storeRegionMapping = new StoreRegionMapping();

            storeRegionMapping.Store_No = this.store_No;
            storeRegionMapping.Region_Code = this.region_Code;

            return storeRegionMapping;
        }

        public static implicit operator StoreRegionMapping(TestStoreRegionMappingBuilder builder)
        {
            return builder.Build();
        }
    }
}