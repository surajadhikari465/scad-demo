namespace Vim.Service.Brand.EsbClient
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Vim.Service;
    using Vim.Service.Models;

    public class EsbBrandClient : IVimBrandClient
    {
        public async Task<VimBrandResponse> SendBrandsToVimAsync(IEnumerable<VimBrand> brands)
        {
            throw new NotImplementedException();
        }
    }
}
