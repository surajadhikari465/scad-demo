using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class BulkItemUploadManagerHandler : IManagerHandler<BulkItemUploadManager>
    {
        private ICommandHandler<BulkItemUploadCommand> bulkUploadCommandHandler;

        public BulkItemUploadManagerHandler(
            ICommandHandler<BulkItemUploadCommand> bulkUploadCommandHandler)

        {
            this.bulkUploadCommandHandler = bulkUploadCommandHandler;
        }

        public void Execute(BulkItemUploadManager data)
        {
            try
            {
                bulkUploadCommandHandler.Execute(new BulkItemUploadCommand()
                { BulkItemUploadModel = data.BulkItemUploadModel });
            }

            catch (Exception ex)
            {
                throw new CommandException($"An error occurred in uploading the excel file.", ex);
            }
        }
    }
}