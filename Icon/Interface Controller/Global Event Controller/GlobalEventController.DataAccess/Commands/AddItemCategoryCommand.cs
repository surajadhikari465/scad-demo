using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddItemCategoryCommand
    {
        public int SubTeamNo { get; set; }
        public int UserId { get; set; }
        public int ItemCategoryId { get; set; }
    }
}
