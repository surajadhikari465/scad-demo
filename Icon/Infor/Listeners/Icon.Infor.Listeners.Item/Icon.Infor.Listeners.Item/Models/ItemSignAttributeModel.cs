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
		public string AnimalWelfareRating { get; set; }
		public bool Biodynamic { get; set; }
		public string MilkType { get; set; }
		public bool CheeseRaw { get; set; }
		public string EcoScaleRating { get; set; }
		public string GlutenFreeAgencyName { get; set; }
		public string KosherAgencyName { get; set; }
		public bool Msc { get; set; }
		public string NonGmoAgencyName { get; set; }
		public string OrganicAgencyName { get; set; }
		public bool PremiumBodyCare { get; set; }
		public string FreshOrFrozen { get; set; }
		public string SeafoodCatchType { get; set; }
		public string VeganAgencyName { get; set; }
		public bool Vegetarian { get; set; }
		public bool WholeTrade { get; set; }
		public bool GrassFed { get; set; }
		public bool PastureRaised { get; set; }
		public bool FreeRange { get; set; }
		public bool DryAged { get; set; }
		public bool AirChilled { get; set; }
		public bool MadeInHouse { get; set; }
		public string CustomerFriendlyDescription { get; set; }

		public ItemSignAttributeModel(
			int itemId,
			string AnimalWelfareRating,
			bool Biodynamic,
			string MilkType,
			bool CheeseRaw,
			string EcoScaleRating,
			string GlutenFreeAgency,
			string KosherAgency,
			bool Msc,
			string NonGmoAgency,
			string OrganicAgency,
			bool PremiumBodyCare,
			string FreshOrFrozen,
			string SeafoodCatchType,
			string VeganAgency,
			bool Vegetarian,
			bool WholeTrade,
			bool GrassFed,
			bool PastureRaised,
			bool FreeRange,
			bool DryAged,
			bool AirChilled,
			bool MadeInHouse,
			string CustomerFriendlyDescription)
		{
			this.ItemId = itemId;
			this.AnimalWelfareRating = AnimalWelfareRating;
			this.Biodynamic = Biodynamic;
			this.MilkType = MilkType;
			this.CheeseRaw = CheeseRaw;
			this.EcoScaleRating = EcoScaleRating;
			this.GlutenFreeAgencyName = GlutenFreeAgency;
			this.KosherAgencyName = KosherAgency;
			this.Msc = Msc;
			this.NonGmoAgencyName = NonGmoAgency;
			this.OrganicAgencyName = OrganicAgency;
			this.PremiumBodyCare = PremiumBodyCare;
			this.FreshOrFrozen = FreshOrFrozen;
			this.SeafoodCatchType = SeafoodCatchType;
			this.VeganAgencyName = VeganAgency;
			this.Vegetarian = Vegetarian;
			this.WholeTrade = WholeTrade;
			this.GrassFed = GrassFed;
			this.PastureRaised = PastureRaised;
			this.FreeRange = FreeRange;
			this.DryAged = DryAged;
			this.AirChilled = AirChilled;
			this.MadeInHouse = MadeInHouse;
			this.CustomerFriendlyDescription = CustomerFriendlyDescription;
		}
	}
}
