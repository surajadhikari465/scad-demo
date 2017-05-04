using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irma.Framework;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using Icon.Logging;
using GlobalEventController.DataAccess.Queries;
using System.Data.Entity;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddOrUpdateNationalHierarchyCommandHandler : ICommandHandler<AddOrUpdateNationalHierarchyCommand>
    {
        private readonly IrmaContext irmacontext;
        private ILogger<AddOrUpdateNationalHierarchyCommandHandler> logger;
        private Boolean isAdding = false;

        public AddOrUpdateNationalHierarchyCommandHandler(IrmaContext irmacontext,
                                                          ILogger<AddOrUpdateNationalHierarchyCommandHandler> logger
                                                         )
        {
            this.irmacontext = irmacontext;
            this.logger = logger;
        }

        private ValidatedNationalClass getValidatedNationalClassByHierarchyClassId(int iconId)
        {
            return irmacontext.ValidatedNationalClass.Where(hc => hc.IconId == iconId).FirstOrDefault();
        }
        public void Handle(AddOrUpdateNationalHierarchyCommand command)
        {
            HierarchyClass hierarchyClass = command.hierarchyClass;
            ValidatedNationalClass validatedNationalClass = getValidatedNationalClassByHierarchyClassId(command.IconId);
            command.level = hierarchyClass.hierarchyLevel;
            command.ParentId = hierarchyClass.hierarchyParentClassID;

            if (validatedNationalClass != null)
            {
                command.IrmaId = validatedNationalClass.IrmaId;
            }

            if (hierarchyClass.hierarchyLevel == HierarchyLevels.NationalFamily || hierarchyClass.hierarchyLevel == HierarchyLevels.NationalCategory)
            {
                AddOrUpdateNatItemfamily(command);
            }
            else if (hierarchyClass.hierarchyLevel == HierarchyLevels.NationalSubCategory)
            {
                AddOrUpdateNatItemCat(command);
            }
            else
            {
                AddOrUpdateNatItemClass(command);
            }
        }
        private void AddOrUpdateNatItemfamily(AddOrUpdateNationalHierarchyCommand command)
        {
            NatItemFamily natItemFamily;
            using (DbContextTransaction transaction = irmacontext.Database.BeginTransaction())
            {
                try
                {
                    if (command.IrmaId == null)
                    {
                        natItemFamily = new NatItemFamily
                        {
                            NatFamilyName = command.Name,
                            LastUpdateTimestamp = DateTime.Now
                        };
                        irmacontext.NatItemFamily.Add(natItemFamily);
                        isAdding = true;
                    }
                    else
                    {
                        natItemFamily = irmacontext.NatItemFamily.SingleOrDefault(nic => nic.NatFamilyID == command.IrmaId);
                        natItemFamily.NatFamilyName = getUpdatedNetFamilyName(natItemFamily.NatFamilyName, command.Name, (int)command.level);
                        natItemFamily.LastUpdateTimestamp = DateTime.Now;
                    }

                    irmacontext.SaveChanges();
                    command.IrmaId = natItemFamily.NatFamilyID;

                    if (isAdding)
                    {
                        AddValidatedNationalClass(command);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        private string getUpdatedNetFamilyName(string natFamilyNameFromDb, string newNatFamilyName, int level)
        {
            string updatedNatFamilyName = string.Empty;
            string[] natFamilyNameArray = natFamilyNameFromDb.Split(new char[] { '-' }, 2);

            if (level == HierarchyLevels.NationalFamily)
            {
                if (natFamilyNameArray.Count() > 1)
                    updatedNatFamilyName = newNatFamilyName.TrimEnd() + " - " + natFamilyNameArray[1].TrimStart();
                else
                    updatedNatFamilyName = newNatFamilyName.TrimEnd();
            }
            else if (level == HierarchyLevels.NationalCategory)
            {
                updatedNatFamilyName = natFamilyNameArray[0].TrimEnd() + " - " + newNatFamilyName.TrimStart();
            }
            return updatedNatFamilyName;
        }
        private void AddOrUpdateNatItemCat(AddOrUpdateNationalHierarchyCommand command)
        {
            NatItemCat natItemCat;
            ValidatedNationalClass validatedNationalClass = getValidatedNationalClassByHierarchyClassId((int)command.ParentId);
            int parentId;

            string errorMessage = String.Format("The parent for National Hierarchy not found in  table {0}:  HierarchyClassId = {1}", "Validated National Class", command.ParentId);

            if (validatedNationalClass == null || validatedNationalClass.IrmaId == null)
            {
                throw new Exception(errorMessage);
            }
            else
            {
                parentId = (int)validatedNationalClass.IrmaId;
            }
            using (DbContextTransaction transaction = irmacontext.Database.BeginTransaction())
            {
                try
                {
                    if (command.IrmaId == null)
                    {
                        natItemCat = new NatItemCat
                        {
                            NatCatName = command.Name,
                            NatFamilyID = parentId,
                            LastUpdateTimestamp = DateTime.Now
                        };
                        irmacontext.NatItemCat.Add(natItemCat);
                        isAdding = true;
                    }
                    else
                    {
                        natItemCat = irmacontext.NatItemCat.SingleOrDefault(nic => nic.NatCatID == command.IrmaId);
                        natItemCat.NatCatName = command.Name;
                        natItemCat.LastUpdateTimestamp = DateTime.Now;
                    }

                    irmacontext.SaveChanges();
                    command.IrmaId = natItemCat.NatCatID;

                    if (isAdding)
                    {
                        AddValidatedNationalClass(command);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        private void AddOrUpdateNatItemClass(AddOrUpdateNationalHierarchyCommand command)
        {
            NatItemClass natItemClass;
            int parentId;

            ValidatedNationalClass validatedNationalClass = getValidatedNationalClassByHierarchyClassId((int)command.ParentId);
            string errorMessage = String.Format("The parent for National Hierarchy not found in  table {0}:  HierarchyClassId = {1}", "Validated National Class", command.ParentId);

            if (validatedNationalClass == null || validatedNationalClass.IrmaId == null)
            {
                throw new Exception(errorMessage);
            }
            else
            {
                parentId = (int)validatedNationalClass.IrmaId;
            }
            using (DbContextTransaction transaction = irmacontext.Database.BeginTransaction())
            {
                try
                {
                    int classId;
                    if (command.IrmaId == null)
                    {
                        if (command.hierarchyClass.HierarchyClassTrait != null && command.hierarchyClass.HierarchyClassTrait.Count >0)
                        {
                            classId = Convert.ToInt32(command.hierarchyClass.HierarchyClassTrait.ToList().Single(hct => hct.traitID == Traits.NationalClassCode).traitValue);
                        }
                        else
                        {
                            throw new Exception(String.Format("Unable to retrieve Class Id from Infor for HierarchyClass = {1}", command.Name));
                        }

                        natItemClass = new NatItemClass
                        {
                            ClassID = classId,
                            ClassName = command.Name,
                            NatCatID = (int)parentId,
                            LastUpdateTimestamp = DateTime.Now
                        };
                        irmacontext.NatItemClass.Add(natItemClass);
                        isAdding = true;
                    }
                    else
                    {
                        natItemClass = irmacontext.NatItemClass.SingleOrDefault(nic => nic.ClassID == command.IrmaId);
                        natItemClass.ClassName = command.Name;
                        natItemClass.LastUpdateTimestamp = DateTime.Now;
                    }

                    irmacontext.SaveChanges();
                    command.IrmaId = natItemClass.ClassID;

                    if (isAdding)
                    {
                        AddValidatedNationalClass(command);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        private void AddValidatedNationalClass(AddOrUpdateNationalHierarchyCommand command)
        {
            ValidatedNationalClass validatedNationalClass = new ValidatedNationalClass()
            {
                IrmaId = command.IrmaId,
                IconId = command.IconId,
                InsertDate = System.DateTime.Now
            };

            irmacontext.ValidatedNationalClass.Add(validatedNationalClass);
            irmacontext.SaveChanges();
        }
    }
}
