using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class DeleteNationalHierarchyCommandHandler : ICommandHandler<DeleteNationalHierarchyCommand>
    {
        private const string natItemCatTableName = "NatItemCat";
        private const string natItemClassTableName = "NatItemClass";
        private const string natItemfamilyTableName = "NatItemFamily";

        private IDbContextFactory<IrmaContext> contextFactory;
        private ILogger<DeleteNationalHierarchyCommandHandler> logger;

        public DeleteNationalHierarchyCommandHandler(IDbContextFactory<IrmaContext> contextFactory, ILogger<DeleteNationalHierarchyCommandHandler> logger)
        {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        public void Handle(DeleteNationalHierarchyCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                List<ValidatedNationalClass> validatedNationalClasses = getValidatedNationalClassByHierarchyClassId(context, command.IconId);

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
                            DeleteNatItemFamily(context, command);
                        }
                        else if (command.Level == HierarchyLevels.NationalSubCategory)
                        {
                            DeleteNatItemCat(context, command);
                        }
                        else
                        {
                            DeleteNatItemClass(context, command);
                        }
                    }
                }
            }
        }

        private List<ValidatedNationalClass> getValidatedNationalClassByHierarchyClassId(IrmaContext context, int iconId)
        {
            return context.ValidatedNationalClass.Where(hc => hc.IconId == iconId).ToList();
        }

        private void DeleteNatItemCat(IrmaContext context, DeleteNationalHierarchyCommand command)
        {
            var validatedNationalHierarchyToDelete = context.NatItemCat.Where(nic => nic.NatCatID == command.IrmaId).SingleOrDefault();
            bool hasPassedValidation = ValidatedNationalHierarchyToDelete(context, validatedNationalHierarchyToDelete, natItemCatTableName, (int)command.IrmaId);

            if (hasPassedValidation)
            {
                var nationalClassToDelete = context.ValidatedNationalClass.Where(vnc => vnc.IconId == command.IconId).SingleOrDefault();
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.NatItemCat.Remove(validatedNationalHierarchyToDelete);
                        context.SaveChanges();
                        context.ValidatedNationalClass.Remove(nationalClassToDelete);
                        context.SaveChanges();
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

        private void DeleteNatItemClass(IrmaContext context, DeleteNationalHierarchyCommand command)
        {
            var validatedNationalHierarchyToDelete = context.NatItemClass.Where(nic => nic.ClassID == command.IrmaId).SingleOrDefault();
            bool hasPassedValidation = ValidatedNationalHierarchyToDelete(context, validatedNationalHierarchyToDelete, natItemClassTableName, (int)command.IrmaId);

            if (hasPassedValidation)
            {
                var nationalClassToDelete = context.ValidatedNationalClass.Where(vnc => vnc.IconId == command.IconId).SingleOrDefault();
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.NatItemClass.Remove(validatedNationalHierarchyToDelete);
                        context.SaveChanges();
                        context.ValidatedNationalClass.Remove(nationalClassToDelete);
                        context.SaveChanges();
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

        private void DeleteNatItemFamily(IrmaContext context, DeleteNationalHierarchyCommand command)
        {
            var validatedNationalHierarchyToDelete = context.NatItemFamily.SingleOrDefault(nif => nif.NatFamilyID == command.IrmaId);

            bool hasPassedValidation = ValidatedNationalHierarchyToDelete(context, validatedNationalHierarchyToDelete, natItemfamilyTableName, (int)command.IrmaId);

            if (hasPassedValidation)
            {
                var nationalClassToDelete = context.ValidatedNationalClass.SingleOrDefault(vnc => vnc.IconId == command.IconId);
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (validatedNationalHierarchyToDelete != null)
                        {
                            context.NatItemFamily.Remove(validatedNationalHierarchyToDelete);
                            context.SaveChanges();
                        }
                        context.ValidatedNationalClass.Remove(nationalClassToDelete);
                        context.SaveChanges();
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

        private Boolean ValidatedNationalHierarchyToDelete<T>(IrmaContext context, T nationalHierarchyClass, string tableName, int Id)
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
                hasChild = context.NatItemCat.Any(nic => nic.NatFamilyID == Id);
            }
            // child exist
            else if (tableName == natItemClassTableName)
            {
                hasChild = context.NatItemClass.Any(nic => nic.NatCatID == Id);
            }
            //item exist
            else if (tableName == natItemClassTableName)
            {
                hasAssociatedItem = context.Item.Any(it => it.ClassID == Id);
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