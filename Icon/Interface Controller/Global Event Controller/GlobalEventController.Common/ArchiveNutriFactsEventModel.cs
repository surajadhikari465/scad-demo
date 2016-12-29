using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Common
{
    public class ArchiveNutriFactsEventModel
    {
        public ArchiveNutriFactsEventModel(NutriFactsModel nutriFactsModel, EventQueue eventQueue)
        {
            NutriFactsModel = nutriFactsModel;
            Event = eventQueue;
        }

        public EventQueue Event { get; set; }
        public NutriFactsModel NutriFactsModel { get; set; }
    }
}
