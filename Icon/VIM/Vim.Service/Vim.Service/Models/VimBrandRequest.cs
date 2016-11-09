namespace Vim.Service.Models
{
    using System.Collections.Generic;

    public class VimBrandRequest
    {
        public IEnumerable<VimBrand> Brands { get; set; }
    }
}
