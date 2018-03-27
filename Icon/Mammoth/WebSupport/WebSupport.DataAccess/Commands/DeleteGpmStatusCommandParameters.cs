using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Commands
{
    public class DeleteGpmStatusCommandParameters
    {
        public DeleteGpmStatusCommandParameters() { }

        public DeleteGpmStatusCommandParameters(string region) : this()
        {
            Region = region;
        }

        public string Region { get; set; }
    }
}
