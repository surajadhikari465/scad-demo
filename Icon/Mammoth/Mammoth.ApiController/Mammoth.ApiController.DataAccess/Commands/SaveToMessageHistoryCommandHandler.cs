using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Mammoth.Framework;

namespace Mammoth.ApiController.DataAccess.Commands
{
    public class SaveToMessageHistoryCommandHandler : ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>
    {
        private IDbContextFactory<MammothContext> mammothContextFactory;

        public SaveToMessageHistoryCommandHandler(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothContextFactory = mammothContextFactory;
        }

        public void Execute(SaveToMessageHistoryCommand<MessageHistory> data)
        {
            using (var context = mammothContextFactory.CreateContext())
            {
                context.MessageHistories.Add(data.Message);
                context.SaveChanges();
            }
        }
    }
}
