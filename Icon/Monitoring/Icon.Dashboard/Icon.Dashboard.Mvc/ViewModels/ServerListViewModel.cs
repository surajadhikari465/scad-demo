using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class ServerListViewModel
    {
        public ServerListViewModel()
        {
            Servers = new List<string>();
        }

        public List<string> Servers { get; set; }
    }

    public class ServerViewModel
    {
        public ServerViewModel() { }

        public string ServerName { get; set; }
    }
}