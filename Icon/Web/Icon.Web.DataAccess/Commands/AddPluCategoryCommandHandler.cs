using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Commands
{
    public class AddPluCategoryCommandHandler : ICommandHandler<AddPluCategoryCommand>
    {
        private IconContext context;

        public AddPluCategoryCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddPluCategoryCommand data)
        {
            context.PLUCategory.Add(data.PluCategory);
            context.SaveChanges();
        }
    }
}
