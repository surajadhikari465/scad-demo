using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Models
{
    public class ExtendedAttributesModel
    {
        public int ItemId { get; set; }
		public Dictionary<string, string> Traits { get; set; }
    }
}