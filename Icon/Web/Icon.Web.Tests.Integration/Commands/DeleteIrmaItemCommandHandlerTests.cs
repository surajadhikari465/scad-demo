using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class DeleteIrmaItemCommandHandlerTests
    {
        private IconContext context;
        private DeleteIrmaItemCommand deleteIrmaItemCommand;
        private DeleteIrmaItemCommandHandler deleteIrmaItemCommandHandler;
        private string irmaItemIdentifier;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            deleteIrmaItemCommandHandler = new DeleteIrmaItemCommandHandler(this.context);
            irmaItemIdentifier = "999999";

            IRMAItem irmaItem  = new TestIrmaItemBuilder().WithIdentifier(irmaItemIdentifier);
           

            // Remove any existing instances of the IRMAItem
            context.IRMAItem.RemoveRange(context.IRMAItem
                .Where(hct => hct.identifier == irmaItemIdentifier));
            context.SaveChanges();


            // Add the IRMAItem so that it will be available for edit.
            context.IRMAItem.Add(irmaItem);
            context.SaveChanges();

            // Capture the just-added IRMAItem ID.
           
            deleteIrmaItemCommand = new DeleteIrmaItemCommand()
            {
                IrmaItemId = irmaItem.irmaItemID
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            context = new IconContext();
            context.IRMAItem.RemoveRange(context.IRMAItem.Where(hct => hct.identifier == irmaItemIdentifier));
            context.SaveChanges();            
            context.Dispose();
            context = null;
        }

        [TestMethod]
        public void DeleteIrmaItemCommandHandler_SuccessfulExecution_IrmaItemShouldBeDeleted()
        {
            // When.
           
            deleteIrmaItemCommandHandler.Execute(deleteIrmaItemCommand);

            // Then.
            bool irmaItemExists = context.IRMAItem.Any(hc => hc.identifier == irmaItemIdentifier);
            Assert.IsFalse(irmaItemExists);
        }

        

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void DeleteIrmaItemCommandHandler_FailedExecution_ShouldThrowCommandException()
        {
            // When.
            deleteIrmaItemCommand.IrmaItemId = 0;
            deleteIrmaItemCommandHandler.Execute(deleteIrmaItemCommand);
            
            //Then
            //Should throw exception.
        }
    }
}

