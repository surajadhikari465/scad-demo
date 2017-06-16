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
        private readonly IrmaContext irmaContext;
        private ILogger<AddOrUpdateNationalHierarchyCommandHandler> logger;

        public AddOrUpdateNationalHierarchyCommandHandler(
            IrmaContext irmaContext,
            ILogger<AddOrUpdateNationalHierarchyCommandHandler> logger)
        {
            this.irmaContext = irmaContext;
            this.logger = logger;
        }

        public void Handle(AddOrUpdateNationalHierarchyCommand command)
        {
            var validatedNationalClassExists = irmaContext.ValidatedNationalClass
                .Any(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);

            if (validatedNationalClassExists)
            {
                UpdateNationalClass(command);
            }
            else
            {
                AddNationalClass(command);
            }

            irmaContext.SaveChanges();
        }

        private void AddNationalClass(AddOrUpdateNationalHierarchyCommand command)
        {
            switch (command.HierarchyClass.hierarchyLevel)
            {
                case HierarchyLevels.NationalFamily: 
                    // Don't add new National Family records. These will be added with National Categories
                    // since IRMA combines Family and Category records into a single NatItemFamily record
                    break;
                case HierarchyLevels.NationalCategory:
                    var natItemFamily = irmaContext.NatItemFamily.Add(new NatItemFamily
                        {
                            NatFamilyName = $"{command.ParentHierarchyClass.hierarchyClassName} - {command.HierarchyClass.hierarchyClassName}",
                            NatSubTeam_No = null,
                            SubTeam_No = null,
                            LastUpdateTimestamp = DateTime.Now
                        });
                    irmaContext.SaveChanges();
                    AddValidatedNationalClass(command.ParentHierarchyClass, natItemFamily.NatFamilyID);
                    AddValidatedNationalClass(command.HierarchyClass, natItemFamily.NatFamilyID);
                    break;
                case HierarchyLevels.NationalSubCategory:
                    var natFamilyId = irmaContext.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyParentClassID.Value).IrmaId.Value;
                    var natItemCat = irmaContext.NatItemCat.Add(new NatItemCat
                        {
                            NatCatName = command.HierarchyClass.hierarchyClassName,
                            NatFamilyID = natFamilyId,
                            LastUpdateTimestamp = DateTime.Now
                        });
                    irmaContext.SaveChanges();
                    AddValidatedNationalClass(command.HierarchyClass, natItemCat.NatCatID);
                    break;
                case HierarchyLevels.NationalClass:
                    var natCatId = irmaContext.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyParentClassID.Value).IrmaId.Value;
                    var natItemClass = irmaContext.NatItemClass.Add(new NatItemClass
                        {
                            ClassID = int.Parse(command.HierarchyClass.HierarchyClassTrait.First(hct => hct.traitID == Traits.NationalClassCode).traitValue),
                            ClassName = command.HierarchyClass.hierarchyClassName,
                            NatCatID = natCatId,
                            LastUpdateTimestamp = DateTime.Now
                        });
                    irmaContext.SaveChanges();
                    AddValidatedNationalClass(command.HierarchyClass, natItemClass.ClassID);
                    break;
                default:
                    throw new ArgumentException($"Unable to add National Class. No National Hierarchy Level registered for given level of '{command.HierarchyClass.hierarchyLevel}'.", nameof(command.HierarchyClass.hierarchyLevel));
            }
        }

        private void UpdateNationalClass(AddOrUpdateNationalHierarchyCommand command)
        {
            ValidatedNationalClass validatedNationalClass = null;
            NatItemFamily natItemFamily = null;

            switch (command.HierarchyClass.hierarchyLevel)
            {
                case HierarchyLevels.NationalFamily:
                    var validatedNationalClasses = irmaContext.ValidatedNationalClass
                        .Where(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);
                    foreach (var vnc in validatedNationalClasses)
                    {
                        natItemFamily = irmaContext.NatItemFamily.First(nif => nif.NatFamilyID == vnc.IrmaId);
                        var categoryName = GetCategoryName(natItemFamily);
                        natItemFamily.NatFamilyName = $"{command.HierarchyClass.hierarchyClassName} - {categoryName}";
                        natItemFamily.LastUpdateTimestamp = DateTime.Now;
                    }
                    break;
                case HierarchyLevels.NationalCategory:
                    validatedNationalClass = irmaContext.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);
                    natItemFamily = irmaContext.NatItemFamily.First(nif => nif.NatFamilyID == validatedNationalClass.IrmaId);
                    var familyName = GetFamilyName(natItemFamily);
                    natItemFamily.NatFamilyName = $"{familyName} - {command.HierarchyClass.hierarchyClassName}";
                    natItemFamily.LastUpdateTimestamp = DateTime.Now;
                    break;
                case HierarchyLevels.NationalSubCategory:
                    validatedNationalClass = irmaContext.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);
                    var natItemCat = irmaContext.NatItemCat.First(nic => nic.NatCatID == validatedNationalClass.IrmaId);
                    natItemCat.NatCatName = command.HierarchyClass.hierarchyClassName;
                    natItemCat.LastUpdateTimestamp = DateTime.Now;
                    break;
                case HierarchyLevels.NationalClass:
                    validatedNationalClass = irmaContext.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);
                    var natItemClass = irmaContext.NatItemClass.First(nicl => nicl.ClassID == validatedNationalClass.IrmaId);
                    natItemClass.ClassName = command.HierarchyClass.hierarchyClassName;
                    natItemClass.LastUpdateTimestamp = DateTime.Now;
                    break;
                default:
                    throw new ArgumentException($"Unable to update National Class. No National Hierarchy Level registered for given level of '{command.HierarchyClass.hierarchyLevel}'.", nameof(command.HierarchyClass.hierarchyLevel));
            }
        }

        private static string GetFamilyName(NatItemFamily natItemFamily)
        {
            return natItemFamily.NatFamilyName.Split(new[] { '-' }, 2)[0].Trim();
        }

        private static string GetCategoryName(NatItemFamily natItemFamily)
        {
            return natItemFamily.NatFamilyName.Split(new[] { '-' }, 2)[1].Trim();
        }

        private void AddValidatedNationalClass(HierarchyClass hierarchyClass, int irmaId)
        {
            irmaContext.ValidatedNationalClass.Add(new ValidatedNationalClass()
                {
                    IrmaId = irmaId,
                    IconId = hierarchyClass.hierarchyClassID,
                    Level = hierarchyClass.hierarchyLevel,
                    InsertDate = DateTime.Now
                });
        }
    }
}
