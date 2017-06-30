using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    /// <summary>
    /// Adds or Updates rows in IconItemLastChange table.  If a row exists, it only updates the InsertDate.
    /// For now this is used for the Brand Update event.
    /// </summary>
    public class AddUpdateLastChangeByIdentifiersCommandHandler : ICommandHandler<AddUpdateLastChangeByIdentifiersCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public AddUpdateLastChangeByIdentifiersCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(AddUpdateLastChangeByIdentifiersCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                List<IconItemLastChange> addLastChanges = new List<IconItemLastChange>();
                foreach (ItemIdentifier identifier in command.Identifiers)
                {
                    IconItemLastChange lastChange = context.IconItemLastChange
                        .SingleOrDefault(c => c.Identifier == identifier.Identifier);

                    // If the Last Change record doesn't exist, add it.  If it does exits, just update the InsertDate.
                    if (lastChange == null)
                    {
                        lastChange = new IconItemLastChange();
                        lastChange.Identifier = identifier.Identifier;
                        lastChange.Item_Description = identifier.Item.Item_Description;
                        lastChange.POS_Description = identifier.Item.POS_Description.ToUpper();
                        lastChange.Package_Desc1 = identifier.Item.Package_Desc1;
                        lastChange.Food_Stamps = identifier.Item.Food_Stamps;
                        lastChange.ScaleTare = identifier.Item.ScaleTare;
                        lastChange.Subteam_No = null;
                        lastChange.TaxClassID = identifier.Item.TaxClassID;
                        lastChange.Brand_ID = identifier.Item.Brand_ID;
                        lastChange.Subteam_No = identifier.Item.SubTeam_No;
                        lastChange.InsertDate = DateTime.Now;

                        addLastChanges.Add(lastChange);
                    }
                    else
                    {
                        lastChange.InsertDate = DateTime.Now;
                    }
                }

                context.IconItemLastChange.AddRange(addLastChanges);
                context.SaveChanges();
            }
        }
    }
}
