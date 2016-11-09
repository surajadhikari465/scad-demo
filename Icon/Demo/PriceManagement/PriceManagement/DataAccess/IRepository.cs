using PriceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceManagement.DataAccess
{
    public interface IRepository
    {
        IQueryable<ItemModel> Items { get; }
        IQueryable<PriceModel> Prices { get; }
        IQueryable<LocaleModel> Locales { get; }

        void AddRange<T>(List<T> entities);
    }
}
