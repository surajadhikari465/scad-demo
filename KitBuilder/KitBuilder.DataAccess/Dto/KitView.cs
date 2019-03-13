using KitBuilder.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.Dto
{
    public class KitView
    {
		public KitView()
		{
		}
       
        public int KitId { get; set; }
        public string Description { get; set; }
        public int RegionId { get; set; }
        public int MetroId { get; set; }
        public int StoreId { get; set; }
        public decimal KitPrice { get; set; }
        public int MinimumCalories { get; set; }
        public int MaximumCalories { get; set; }
        public int KitLocaleId { get; set; }
        public bool AuthorizedByStore { get; set; }
        public bool Excluded { get; set; }
        public string ErrorMessage { get; set; }
        public ICollection<LinkGroupView> LinkGroups { get; set; }
    }
}
