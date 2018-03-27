using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Commands
{
    public class UpsertGpmStatusCommandParameters
    {
        public UpsertGpmStatusCommandParameters() { }

        public UpsertGpmStatusCommandParameters(string region, bool isGpmEnabled) : this()
        {
            Region = region;
            IsGpmEnabled = isGpmEnabled;
        }

        public string Region { get; set; }
        public bool IsGpmEnabled { get; set; }
    }
}
