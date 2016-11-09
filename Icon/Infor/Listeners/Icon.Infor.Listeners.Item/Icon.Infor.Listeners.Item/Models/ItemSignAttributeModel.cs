using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Models
{
    public class ItemSignAttributeModel
    {
        public int ItemId { get; set; }
        public int? AnimalWelfareRatingId { get; set; }
        public bool Biodynamic { get; set; }
        public int? CheeseMilkTypeId { get; set; }
        public bool CheeseRaw { get; set; }
        public int? EcoScaleRatingId { get; set; }
        public string GlutenFreeAgencyName { get; set; }
        public string KosherAgencyName { get; set; }
        public bool Msc { get; set; }
        public string NonGmoAgencyName { get; set; }
        public string OrganicAgencyName { get; set; }
        public bool PremiumBodyCare { get; set; }
        public int? SeafoodFreshOrFrozenId { get; set; }
        public int? SeafoodCatchTypeId { get; set; }
        public string VeganAgencyName { get; set; }
        public bool Vegetarian { get; set; }
        public bool WholeTrade { get; set; }
        public bool GrassFed { get; set; }
        public bool PastureRaised { get; set; }
        public bool FreeRange { get; set; }
        public bool DryAged { get; set; }
        public bool AirChilled { get; set; }
        public bool MadeInHouse { get; set; }

        public ItemSignAttributeModel(
            int itemId,
            int? AnimalWelfareRatingId,
            bool Biodynamic,
            int? CheeseMilkTypeId,
            bool CheeseRaw,
            int? EcoScaleRatingId,
            string GlutenFreeAgency,
            string KosherAgency,
            bool Msc,
            string NonGmoAgency,
            string OrganicAgency,
            bool PremiumBodyCare,
            int? SeafoodFreshOrFrozenTypeId,
            int? SeafoodCatchTypeId,
            string VeganAgency,
            bool Vegetarian,
            bool WholeTrade,
            bool GrassFed,
            bool PastureRaised,
            bool FreeRange,
            bool DryAged,
            bool AirChilled,
            bool MadeInHouse)
        {
            this.ItemId = itemId;
            this.AnimalWelfareRatingId = AnimalWelfareRatingId;
            this.Biodynamic = Biodynamic;
            this.CheeseMilkTypeId = CheeseMilkTypeId;
            this.CheeseRaw = CheeseRaw;
            this.EcoScaleRatingId = EcoScaleRatingId;
            this.GlutenFreeAgencyName = GlutenFreeAgency;
            this.KosherAgencyName = KosherAgency;
            this.Msc = Msc;
            this.NonGmoAgencyName = NonGmoAgency;
            this.OrganicAgencyName = OrganicAgency;
            this.PremiumBodyCare = PremiumBodyCare;
            this.SeafoodFreshOrFrozenId = SeafoodFreshOrFrozenTypeId;
            this.SeafoodCatchTypeId = SeafoodCatchTypeId;
            this.VeganAgencyName = VeganAgency;
            this.Vegetarian = Vegetarian;
            this.WholeTrade = WholeTrade;
            this.GrassFed = GrassFed;
            this.PastureRaised = PastureRaised;
            this.FreeRange = FreeRange;
            this.DryAged = DryAged;
            this.AirChilled = AirChilled;
            this.MadeInHouse = MadeInHouse;
        }
    }
}
