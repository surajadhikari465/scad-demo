using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using System.Collections.Generic;

namespace Mammoth.PrimeAffinity.Library.Processors
{
    public class PrimeAffinityPsgProcessorParameters
    {
        public string Region { get; set; }
        public IEnumerable<PrimeAffinityMessageModel> PrimeAffinityMessageModels { get; set; }
        public ActionEnum MessageAction { get; set; }
    }
}
