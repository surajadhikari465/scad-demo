using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceManagement.Models;

namespace PriceManagement.DataAccess
{
    public class Repository : IRepository
    {
        private static List<PriceModel> prices = new List<PriceModel>
        {
            new PriceModel { ItemId = 1, Price = 3.99m, BusinessUnit = 1234, Region = "FL" }
        };
        private static List<ItemModel> items = new List<ItemModel>
        {
            new ItemModel { ItemId = 1, ScanCode = "1234", Prices = prices }
        };
        private static List<LocaleModel> locales = new List<LocaleModel>
        {
            new LocaleModel { LocaleId = 1, BusinessUnitId = 1234, LocaleName = "Bernard", RegionCode = "FL" },
            new LocaleModel { LocaleId = 2, BusinessUnitId = 12345, LocaleName = "Fernard", RegionCode = "FL" },
            new LocaleModel { LocaleId = 3, BusinessUnitId = 123456, LocaleName = "Trolley", RegionCode = "FL" }
        };

        public IQueryable<ItemModel> Items { get { return items.AsQueryable(); } }
        public IQueryable<PriceModel> Prices { get { return prices.AsQueryable(); } }
        public IQueryable<LocaleModel> Locales { get { return locales.AsQueryable(); } }

        public void AddRange<T>(List<T> entities)
        {
            if(typeof(T) == typeof(PriceModel))
            {
                prices.AddRange(entities.Cast<PriceModel>());
            }
            if(typeof(T) == typeof(ItemModel))
            {
                items.AddRange(entities.Cast<ItemModel>());
            }
            if(typeof(T) == typeof(LocaleModel))
            {
                locales.AddRange(entities.Cast<LocaleModel>());
            }
        }
    }
}
