using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetUserQuery : IQuery<Users>
    {
        public string UserName { get; set; }
    }
}
