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
        private readonly IrmaContext irmacontext;
        private const string natItemCatTableName = "NatItemCat";
        private const string natItemClassTableName = "NatItemClass";
        private const string natItemfamilyTableName = "NatItemfamily";
        private ILogger<DeleteNationalHierarchyCommandHandler> logger;
        public DeleteNationalHierarchyCommandHandler(IrmaContext irmacontext, ILogger<DeleteNationalHierarchyCommandHandler> logger)
        {
            this.irmacontext = irmacontext;
            this.logger = logger;
        }
        public void Handle(DeleteNationalHierarchyCommand command)
        {
            ValidatedNationalClass validatedNationalClass = getValidatedNationalClassByHierarchyClassId(command.iconId);

            if (validatedNationalClass == null)
            {
                logger.Error(String.Format("No record found in ValidatedNationalClass table for IconId: ", command.iconId));
            }
            else
            {
                command.level = validatedNationalClass.Level;
                command.irmaId = validatedNationalClass.IrmaId;

                if (command.level == HierarchyLevels.NationalFamily || command.level == HierarchyLevels.NationalCategory)
                {
                    DeleteNatItemFamily(command);
                }
                else if (command.level == HierarchyLevels.NationalSubCategory)
                {
                    DeleteNatItemCat(command);
                }
                else
                {
                    DeleteNatItemClass(command);
                }
            }
        }
        private ValidatedNationalClass getValidatedNationalClassByHierarchyClassId(int iconId)
        {
            return irmacontext.ValidatedNationalClass.Where(hc => hc.IconId == iconId).FirstOrDefault();
        }
        private void DeleteNatItemCat(DeleteNationalHierarchyCommand command)
        {
            var validatedNationalHierarchyToDelete = irmacontext.NatItemCat.Where(nic => nic.NatCatID == command.irmaId).SingleOrDefault();
            bool hasPassedValidation = ValidatedNationalHierarchyToDelete<NatItemCat>(validatedNationalHierarchyToDelete, natItemCatTableName, (int)command.irmaId);

            if (hasPassedValidation)
            {
                var nationalClassToDelete = irmacontext.ValidatedNationalClass.Where(vnc => vnc.IconId == command.iconId).SingleOrDefault();
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
            var validatedNationalHierarchyToDelete = irmacontext.NatItemClass.Where(nic => nic.ClassID == command.irmaId).SingleOrDefault();
            bool hasPassedValidation = ValidatedNationalHierarchyToDelete<NatItemClass>(validatedNationalHierarchyToDelete, natItemClassTableName, (int)command.irmaId);

            if (hasPassedValidation)
            {
                var nationalClassToDelete = irmacontext.ValidatedNationalClass.Where(vnc => vnc.IconId == command.iconId).SingleOrDefault();
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
            var validatedNationalHierarchyToDelete = irmacontext.NatItemFamily.Where(nif => nif.NatFamilyID == command.irmaId).SingleOrDefault();

            bool hasPassedValidation = ValidatedNationalHierarchyToDelete<NatItemFamily>(validatedNationalHierarchyToDelete, natItemfamilyTableName, (int)command.irmaId);

            if (hasPassedValidation)
            {
                var nationalClassToDelete = irmacontext.ValidatedNationalClass.Where(vnc => vnc.IconId == command.iconId).SingleOrDefault();
                using (DbContextTransaction transaction = irmacontext.Database.BeginTransaction())
                {
                    try
                    {
                        irmacontext.NatItemFamily.Remove(validatedNationalHierarchyToDelete);
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
        private Boolean ValidatedNationalHierarchyToDelete<T>(T nationalHierarchyClass, string tableName, int Id)
        {
            bool hasChild = false;
            bool hasAssociatedItem = false;
            string errorMessage = string.Empty;

            if (EqualityComparer<T>.Default.Equals(nationalHierarchyClass, default(T)))
            {
                errorMessage = String.Format("The following National Hierarchy was not found in the IRMA table {0}, so no update will be performed:  HierarchyClassId = {1}", tableName, Id);
                logger.Error(errorMessage);
                // throw exception-- will return to caller and there we are updating event queue table
                throw new Exception(errorMessage);
            }
            // child exist
            if (tableName == natItemfamilyTableName)
            {
                hasChild = irmacontext.NatItemCat.Where(nic => nic != null && nic.NatFamilyID == Id).Any();
            }
            // child exist
            else if (tableName == natItemClassTableName)
            {
                hasChild = irmacontext.NatItemClass.Where(nic => nic != null && nic.NatCatID == Id).Any();
            }
            //item exist
            else if (tableName == natItemClassTableName)
            {
                hasAssociatedItem = irmacontext.Item.Where(it => it != null && it.ClassID == Id).Any();
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