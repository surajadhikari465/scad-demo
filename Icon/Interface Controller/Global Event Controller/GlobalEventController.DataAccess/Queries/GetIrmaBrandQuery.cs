using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetIrmaBrandQuery : IQuery<ItemBrand>
    {
        public int IconBrandId { get; set; }

        public int ResultItemCount { get; set; }
    }
}
