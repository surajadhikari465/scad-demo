using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model.Repository;

namespace OOS.Model
{
    public class Region
    {
        public int Id { get; private set; }
        public string Abbrev { get; set; }


        private List<Store> stores = new List<Store>();

        public Region(int id)
        {
            Id = id;
        }

        public List<Store> GetStores()
        {
            return stores.ToList();
        }

        public void AddStore(Store store)
        {
            stores.Add(store);
        }

        public List<Store> OpenStores()
        {
            return stores.Where(p => !p.Status.Equals(Store.Closed, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Store> ClosedStores()
        {
            return stores.Where(p => p.Status.Equals(Store.Closed, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
