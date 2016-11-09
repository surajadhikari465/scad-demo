using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class ProductSelectionGroupTypeViewModel
    {
        public int ProductSelectionGroupTypeId { get; set; }
        public string ProductSelectionGroupTypeName { get; set; }

        public ProductSelectionGroupTypeViewModel(){ }

        public ProductSelectionGroupTypeViewModel(ProductSelectionGroupType psgType)
        {
            ProductSelectionGroupTypeId = psgType.ProductSelectionGroupTypeId;
            ProductSelectionGroupTypeName = psgType.ProductSelectionGroupTypeName;
        }
    }
}