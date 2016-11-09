namespace Vim.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Vim.Service.Models;

    public interface IVimBrandClient
    {
        /// <summary>
        /// A client that the VIM Service will use to send Brands to VIM.
        /// </summary>
        /// <param name="brands">The Brand Information from ICON</param>
        /// <returns>The Brands were successfully updated within VIM.</returns>
        Task<VimBrandResponse> SendBrandsToVimAsync(IEnumerable<VimBrand> brands);
    }
}