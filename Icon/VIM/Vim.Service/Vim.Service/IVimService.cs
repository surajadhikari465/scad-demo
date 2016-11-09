namespace Vim.Service
{
    using System.Threading.Tasks;
    using Vim.Service.Models;

    public interface IVimService
    {
        /// <summary>
        /// Sending Brand Add or Update information from ICON to VIM.
        /// </summary>
        /// <param name="brandRequest">List Brand Information from ICON</param>
        /// <returns>Response from the client.</returns>
        Task<VimBrandResponse> SendBrandsAsync(VimBrandRequest brandRequest);
    }
}
