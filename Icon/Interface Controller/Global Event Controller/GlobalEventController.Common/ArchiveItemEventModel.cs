using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Common
{
    public class ArchiveItemEventModel
    {
        public ArchiveItemEventModel(ValidatedItemModel itemModel, EventQueue eventQueue, NutriFactsModel nutriFactsModel = null)
        {
            ItemModel = itemModel;
            Event = eventQueue;
            NutriFactsModel = nutriFactsModel;
        }

        public EventQueue Event { get; set; }
        public ValidatedItemModel ItemModel { get; set; }
        public NutriFactsModel NutriFactsModel { get; set; }
    }
}
