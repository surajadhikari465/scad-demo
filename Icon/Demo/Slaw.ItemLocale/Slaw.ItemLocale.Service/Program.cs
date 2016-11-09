using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Slaw.ItemLocale.Service.MessageBuilders;
using Icon.Esb.Producer;
using Icon.Esb;
using Slaw.ItemLocale.Serializers;

namespace Slaw.ItemLocale.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ItemLocaleRepository repository = new ItemLocaleRepository();
            SlawService service = new SlawService(
                repository,
                new ItemLocaleMessageBuilder(new Serializer<Icon.Esb.Schemas.Wfm.Contracts.items>()), 
                new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig()));

            var regions = new List<string>
            {
                "FL",
                "MA",
                "MW",
                "NA",
                "RM",
                "SO",
                "NC",
                "NE",
                "PN",
                "SP",
                "SW",
                "UK"
            };

            foreach (var region in regions)
            {
                var itemLocaleModels = repository.GetItemLocaleData(region);

                while (itemLocaleModels.Any())
                {
                    service.AddOrUpdateItemLocales(itemLocaleModels);
                    repository.FinalizeItemLocaleRecords(region, itemLocaleModels);

                    itemLocaleModels = repository.GetItemLocaleData(region);
                }
            }
        }
    }
}
