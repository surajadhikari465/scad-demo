using System.Collections.Generic;

namespace Vim.Common.ControllerApplication
{
    public interface IRegionalControllerApplicationSettings
    {
        List<string> Regions { get; set; }
        string CurrentRegion { get; set; }
    }
}