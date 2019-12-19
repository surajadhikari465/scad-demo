using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOS.Services.DAL;
using OOS.Services.DataModels;

namespace OOS.Services.Tests.Fakes
{
    public class FakeSage : ISageApi
    {

        public List<SageStore> GetModifiedStores()
        {
            return new List<SageStore>
            {
                new SageStore{ tlc = "AAA", region = "AA", status = "open", name = "astronautilus"},
                new SageStore{ tlc = "BBB", region = "BB", status = "open", name = "badAstronaut"},
                new SageStore{ tlc = "CCC", region = "CC", status = "open", name = "cantankerousCoot"},
                new SageStore{ tlc = "DDD", region = "DD", status = "open", name = "doooooooooomed"},
                //
            };
        }
    }
}
