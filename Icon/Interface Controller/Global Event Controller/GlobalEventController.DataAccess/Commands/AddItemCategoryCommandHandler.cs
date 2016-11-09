using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddItemCategoryCommandHandler : ICommandHandler<AddItemCategoryCommand>
    {
        private readonly IrmaContext context;
        private readonly string SubTeamAlignedcategoryName = "SubTeam Aligned";
        public AddItemCategoryCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(AddItemCategoryCommand command)
        {
            ItemCategory itemCategory = this.context.ItemCategory
                .SingleOrDefault(i => i.SubTeam_No == command.SubTeamNo && i.Category_Name.Equals(SubTeamAlignedcategoryName));
            if (itemCategory == null)
            {
                itemCategory = new ItemCategory();
                itemCategory.SubTeam_No = command.SubTeamNo;
                itemCategory.Category_Name = SubTeamAlignedcategoryName;
                itemCategory.User_ID = command.UserId;
                this.context.ItemCategory.Add(itemCategory);
                this.context.SaveChanges();
            }

            command.ItemCategoryId = itemCategory.Category_ID;
        }
    }
}
