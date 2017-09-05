using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSupport.DataAccess.TransferObjects;

namespace WebSupport.ViewModels
{
    public class StoreViewModel
    {
        public StoreViewModel()
        {

        }

        public StoreViewModel(StoreTransferObject storeTransferObject) : this()
        {
            BusinessUnit = storeTransferObject.BusinessUnit;
            Abbreviation = storeTransferObject.Abbreviation;
            Name = storeTransferObject.Name;
        }

        public string Name { get; set; }
        public string BusinessUnit { get; set; }
        public string Abbreviation { get; set; }
    }
}