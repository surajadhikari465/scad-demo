using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.Common;
using Icon.Logging;
using System.Configuration;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Decorators
{
    public class DeleteHierarchyClassIconEventDecorator : ICommandHandler<DeleteHierarchyClassCommand>
    {
        private readonly ICommandHandler<DeleteHierarchyClassCommand> commandHandler;
        private IconContext context;
        private ILogger logger;

        public DeleteHierarchyClassIconEventDecorator(
            ICommandHandler<DeleteHierarchyClassCommand> commandHandler,
            IconContext context,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.logger = logger;
        }

        public void Execute(DeleteHierarchyClassCommand command)
        {
            // log that an icon hierarchy class is being deleted
            var deletedClass = command.DeletedHierarchyClass;
            logger.Info($"Deleting iCON Hierarchy Class. ID = {deletedClass.hierarchyClassID}, Name = '{deletedClass.hierarchyClassName}'");
        }
    }
}
