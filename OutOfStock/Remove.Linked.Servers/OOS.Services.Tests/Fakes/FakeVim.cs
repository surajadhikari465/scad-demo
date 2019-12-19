using System.Collections.Generic;
using OOS.Services.DataModels;
using OOS.Services.DAL;

namespace OOS.Services.Tests.Fakes
{
    public class FakeVim  : IVimRepo
    {
        public List<VimStore> GetVimStores()
        {
            return new List<VimStore>
            {
                new VimStore{ STORE_ABBR = "AAA", REGION = "AA", STATUS = "open", STORE_NAME = "astronautilus"},
                new VimStore{ STORE_ABBR = "BBB", REGION = "BB", STATUS = "open", STORE_NAME = "badAstronaut"},
                new VimStore{ STORE_ABBR = "CCC", REGION = "CC", STATUS = "open", STORE_NAME = "cantankerousCoot"},
                new VimStore{ STORE_ABBR = "DDD", REGION = "DD", STATUS = "open", STORE_NAME = "doooooooooomed"},
                //
            };
        }
    }
}