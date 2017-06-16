using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class DeleteNationalHierarchyCommandHandler : ICommandHandler<DeleteNationalHierarchyCommand>
    {
        private const string natItemCatTableName = "NatItemCat";
        private const string natItemClassTableName = "NatItemClass";
        private const string natItemfamilyTableName = "NatItemFamily";

        private IrmaContext irmacontext;
        private ILogger<DeleteNationalHierarchyCommandHandler> logger;

        public DeleteNationalHierarchyCommandHandler(IrmaContext irmacontext, ILogger<DeleteNationalHierarchyCommandHandler> logger)
        {
            this.irmacontext = irmacontext;
            this.logger = logger;
        }

        public void Handle(DeleteNationalHierarchyCommand command)
        {
            List<ValidatedNationalClass> validatedNationalClasses = getValidatedNationalClassByHierarchyClassId(command.IconId);

            if (!validatedNationalClasses.Any())
            {
                logger.Error(String.Format("No record found in ValidatedNationalClass table for IconId: ", command.IconId));
            }
            else
            {
                foreach (var validatedNationalClass in validatedNationalClasses)
                {
                    command.Level = validatedNationalClass.Level;
                    command.IrmaId = validatedNationalClass.IrmaId;

                    if (command.Level == HierarchyLevels.NationalFamily || command.Level == HierarchyLevels.NationalCategory)
                    {
                        DeleteNatItemFamily(command);
                    }
                    else if (command.Level == HierarchyLevels.NationalSubCategory)
                    {
                        DeleteNatItemCat(command);
                    }
                    else
                    {
                        DeleteNatItemClass(command);
                    }
                }
            }
        }

        private List<ValidatedNationalClass> getValidatedNationalClassByHierarchyClassId(int iconId)
        {
            return irmacontext.ValidatedNationalClass.Where(hc => hc.IconId == iconId).ToList();
        }

        private void DeleteNatItemCat(DeleteNationalHierarchyCommand command)
        {
            var validatedNationalHierarchyToDelete = irmacontext.NatItemCat.Where(nic => nic.NatCatID == command.IrmaId).SingleOrDefault();
            bool hasPassedValidation = ValidatedNationalHierarchyToDelete(validatedNationalHierarchyToDelete, natItemCatTableName, (int)command.IrmaId);

            if (hasPassedValidation)
            {
                var nationalClassToDelete = irmacontext.ValidatedNationalClass.Where(vnc => vnc.IconId == command.IconId).SingleOrDefault();
                using (DbContextTransaction transaction = irmacontext.Database.BeginTransaction())
                {
                    try
                    {
                        irmacontext.NatItemCat.Remove(validatedNationalHierarchyToDelete);
                        irmacontext.SaveChanges();
                        irmacontext.ValidatedNationalClass.Remove(nationalClassToDelete);
                        irmacontext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private void DeleteNatItemClass(DeleteNationalHierarchyCommand command)
        {
            var validatedNationalHierarchyToDelete = irmacontext.NatItemClass.Where(nic => nic.ClassID == command.IrmaId).SingleOrDefault();
            bool hasPassedValidation = ValidatedNationalHierarchyToDelete(validatedNationalHierarchyToDelete, natItemClassTableName, (int)command.IrmaId);

            if (hasPassedValidation)
            {
                var nationalClassToDelete = irmacontext.ValidatedNationalClass.Where(vnc => vnc.IconId == command.IconId).SingleOrDefault();
                using (DbContextTransaction transaction = irmacontext.Database.BeginTransaction())
                {
                    try
                    {
                        irmacontext.NatItemClass.Remove(validatedNationalHierarchyToDelete);
                        irmacontext.SaveChanges();
                        irmacontext.ValidatedNationalClass.Remove(nationalClassToDelete);
                        irmacontext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private void DeleteNatItemFamily(DeleteNationalHierarchyCommand command)
        {
            var validatedNationalHierarchyToDelete = irmacontext.NatItemFamily.SingleOrDefault(nif => nif.NatFamilyID == command.IrmaId);

            bool hasPassedValidation = ValidatedNationalHierarchyToDelete(validatedNationalHierarchyToDelete, natItemfamilyTableName, (int)command.IrmaId);

            if (hasPassedValidation)
            {
                var nationalClassToDelete = irmacontext.ValidatedNationalClass.SingleOrDefault(vnc => vnc.IconId == command.IconId);
                using (DbContextTransaction transaction = irmacontext.Database.BeginTransaction())
                {
                    try
                    {
                        if (validatedNationalHierarchyToDelete != null)
                        {
                            irmacontext.NatItemFamily.Remove(validatedNationalHierarchyToDelete);
                            irmacontext.SaveChanges();
                        }
                        irmacontext.ValidatedNationalClass.Remove(nationalClassToDelete);
                        irmacontext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private Boolean ValidatedNationalHierarchyToDelete<T>(T nationalHierarchyClass, string tableName, int Id)
        {
            bool hasChild = false;
            bool hasAssociatedItem = false;
            string errorMessage = string.Empty;

            if (tableName != natItemfamilyTableName && EqualityComparer<T>.Default.Equals(nationalHierarchyClass, default(T)))
            {
                errorMessage = String.Format("The following National Hierarchy was not found in the IRMA table {0}, so no update will be performed:  HierarchyClassId = {1}", tableName, Id);
                logger.Error(errorMessage);
                // throw exception-- will return to caller and there we are updating event queue table
                throw new Exception(errorMessage);
            }
            // child exist
            if (tableName == natItemfamilyTableName)
            {
                hasChild = irmacontext.NatItemCat.Any(nic => nic.NatFamilyID == Id);
            }
            // child exist
            else if (tableName == natItemClassTableName)
            {
                hasChild = irmacontext.NatItemClass.Any(nic => nic.NatCatID == Id);
            }
            //item exist
            else if (tableName == natItemClassTableName)
            {
                hasAssociatedItem = irmacontext.Item.Any(it => it.ClassID == Id);
            }

            if (hasChild)
            {
                errorMessage = String.Format("The following National Hierarchy has child items and cannot be deleted from table {0}:  HierarchyClassId = {1}", tableName, Id);
                logger.Error(errorMessage);
                // throw exception-- will return to caller and there we are updating event queue table
                throw new Exception(errorMessage);
            }

            if (hasAssociatedItem)
            {
                errorMessage = String.Format("The following National Hierarchy has items associated to it and cannot be deleted from table {0}:  HierarchyClassId = {1}", tableName, Id);
                logger.Error(errorMessage);
                // throw exception-- will return to caller and there we are updating event queue table
                throw new Exception(errorMessage);
            }
            return true;
        }
    }
}