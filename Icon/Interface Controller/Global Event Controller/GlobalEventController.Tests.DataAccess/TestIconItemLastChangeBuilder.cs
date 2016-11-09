using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Tests.DataAccess
{
    public class TestIconItemLastChangeBuilder
    {
        private int iconItemLastChangeId;
        private string identifier;
        private int? subTeamNo;
        private int? brandId;
        private string itemDescription;
        private string posDescription;
        private decimal? packageDesc1;
        private bool? foodStamps;
        private decimal? scaleTare;
        private int? taxClassId;
        private DateTime insertDate;

        public TestIconItemLastChangeBuilder()
        {
            this.iconItemLastChangeId = 0;
            this.identifier = "555444333221";
            this.subTeamNo = null;
            this.brandId = null;
            this.itemDescription = "Test Last Change Builder Row";
            this.posDescription = "Test LC Builder";
            this.packageDesc1 = 1;
            this.foodStamps = true;
            this.scaleTare = 0;
            this.taxClassId = null;
            this.insertDate = DateTime.Now;
        }

        public TestIconItemLastChangeBuilder WithIconItemLastChangeId(int lastChangeId)
        {
            this.iconItemLastChangeId = lastChangeId;
            return this;
        }

        public TestIconItemLastChangeBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }

        public TestIconItemLastChangeBuilder WithSubTeamNo(int? subTeamNo)
        {
            this.subTeamNo = subTeamNo;
            return this;
        }

        public TestIconItemLastChangeBuilder WithBrandId(int? brandId)
        {
            this.brandId = brandId;
            return this;
        }

        public TestIconItemLastChangeBuilder WithItemDescription(string itemDescription)
        {
            this.itemDescription = itemDescription;
            return this;
        }

        public TestIconItemLastChangeBuilder WithPosDescription(string posDescription)
        {

            this.posDescription = posDescription;
            return this;
        }

        public TestIconItemLastChangeBuilder WithPackageUnit(decimal? packageDesc1)
        {
            this.packageDesc1 = packageDesc1;
            return this;
        }

        public TestIconItemLastChangeBuilder WithFoodStamps(bool? foodStamps)
        {
            this.foodStamps = foodStamps;
            return this;
        }

        public TestIconItemLastChangeBuilder WithScaleTare(decimal? tare)
        {
            this.scaleTare = tare;
            return this;
        }

        public TestIconItemLastChangeBuilder WithTaxClassId(int? tax)
        {
            this.taxClassId = tax;
            return this;
        }

        public TestIconItemLastChangeBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public IconItemLastChange Build()
        {
            IconItemLastChange lastChange = new IconItemLastChange();
            lastChange.IconItemLastChangeId = this.iconItemLastChangeId;
            lastChange.Identifier = this.identifier;
            lastChange.Subteam_No = this.subTeamNo;
            lastChange.Brand_ID = this.brandId;
            lastChange.Item_Description = this.itemDescription;
            lastChange.POS_Description = this.posDescription;
            lastChange.Package_Desc1 = this.packageDesc1;
            lastChange.ScaleTare = this.scaleTare;
            lastChange.Food_Stamps = this.foodStamps;
            lastChange.TaxClassID = this.taxClassId;
            lastChange.InsertDate = this.insertDate;

            return lastChange;
        }
    }
}
