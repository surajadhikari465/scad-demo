using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using System;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddOrUpdateNationalHierarchyCommandHandler : ICommandHandler<AddOrUpdateNationalHierarchyCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;
        private ILogger<AddOrUpdateNationalHierarchyCommandHandler> logger;

        public AddOrUpdateNationalHierarchyCommandHandler(
            IDbContextFactory<IrmaContext> contextFactory,
            ILogger<AddOrUpdateNationalHierarchyCommandHandler> logger)
        {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        public void Handle(AddOrUpdateNationalHierarchyCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                var validatedNationalClassExists = context.ValidatedNationalClass
                    .Any(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);

                if (validatedNationalClassExists)
                {
                    UpdateNationalClass(context, command);
                }
                else
                {
                    AddNationalClass(context, command);
                }

                context.SaveChanges();
            }
        }

        private void AddNationalClass(IrmaContext context, AddOrUpdateNationalHierarchyCommand command)
        {
            switch (command.HierarchyClass.hierarchyLevel)
            {
                case HierarchyLevels.NationalFamily: 
                    // Don't add new National Family records. These will be added with National Categories
                    // since IRMA combines Family and Category records into a single NatItemFamily record
                    break;
                case HierarchyLevels.NationalCategory:
                    var natItemFamily = context.NatItemFamily.Add(new NatItemFamily
                        {
                            NatFamilyName = $"{command.ParentHierarchyClass.hierarchyClassName} - {command.HierarchyClass.hierarchyClassName}",
                            NatSubTeam_No = null,
                            SubTeam_No = null,
                            LastUpdateTimestamp = DateTime.Now
                        });
                    context.SaveChanges();
                    AddValidatedNationalClass(context, command.ParentHierarchyClass, natItemFamily.NatFamilyID);
                    AddValidatedNationalClass(context, command.HierarchyClass, natItemFamily.NatFamilyID);
                    break;
                case HierarchyLevels.NationalSubCategory:
                    var natFamilyId = context.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyParentClassID.Value).IrmaId.Value;
                    var natItemCat = context.NatItemCat.Add(new NatItemCat
                        {
                            NatCatName = command.HierarchyClass.hierarchyClassName,
                            NatFamilyID = natFamilyId,
                            LastUpdateTimestamp = DateTime.Now
                        });
                    context.SaveChanges();
                    AddValidatedNationalClass(context, command.HierarchyClass, natItemCat.NatCatID);
                    break;
                case HierarchyLevels.NationalClass:
                    var natCatId = context.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyParentClassID.Value).IrmaId.Value;
                    var natItemClass = context.NatItemClass.Add(new NatItemClass
                        {
                            ClassID = int.Parse(command.HierarchyClass.HierarchyClassTrait.First(hct => hct.traitID == Traits.NationalClassCode).traitValue),
                            ClassName = command.HierarchyClass.hierarchyClassName,
                            NatCatID = natCatId,
                            LastUpdateTimestamp = DateTime.Now
                        });
                    context.SaveChanges();
                    AddValidatedNationalClass(context, command.HierarchyClass, natItemClass.ClassID);
                    break;
                default:
                    throw new ArgumentException($"Unable to add National Class. No National Hierarchy Level registered for given level of '{command.HierarchyClass.hierarchyLevel}'.", nameof(command.HierarchyClass.hierarchyLevel));
            }
        }

        private void UpdateNationalClass(IrmaContext context, AddOrUpdateNationalHierarchyCommand command)
        {
            ValidatedNationalClass validatedNationalClass = null;
            NatItemFamily natItemFamily = null;

            switch (command.HierarchyClass.hierarchyLevel)
            {
                case HierarchyLevels.NationalFamily:
                    var validatedNationalClasses = context.ValidatedNationalClass
                        .Where(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);
                    foreach (var vnc in validatedNationalClasses)
                    {
                        natItemFamily = context.NatItemFamily.First(nif => nif.NatFamilyID == vnc.IrmaId);
                        var categoryName = GetCategoryName(natItemFamily);
                        natItemFamily.NatFamilyName = $"{command.HierarchyClass.hierarchyClassName} - {categoryName}";
                        natItemFamily.LastUpdateTimestamp = DateTime.Now;
                    }
                    break;
                case HierarchyLevels.NationalCategory:
                    validatedNationalClass = context.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);
                    natItemFamily = context.NatItemFamily.First(nif => nif.NatFamilyID == validatedNationalClass.IrmaId);
                    var familyName = GetFamilyName(natItemFamily);
                    natItemFamily.NatFamilyName = $"{familyName} - {command.HierarchyClass.hierarchyClassName}";
                    natItemFamily.LastUpdateTimestamp = DateTime.Now;
                    break;
                case HierarchyLevels.NationalSubCategory:
                    validatedNationalClass = context.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);
                    var natItemCat = context.NatItemCat.First(nic => nic.NatCatID == validatedNationalClass.IrmaId);
                    natItemCat.NatCatName = command.HierarchyClass.hierarchyClassName;
                    natItemCat.LastUpdateTimestamp = DateTime.Now;
                    break;
                case HierarchyLevels.NationalClass:
                    validatedNationalClass = context.ValidatedNationalClass
                        .First(vnc => vnc.IconId == command.HierarchyClass.hierarchyClassID);
                    var natItemClass = context.NatItemClass.First(nicl => nicl.ClassID == validatedNationalClass.IrmaId);
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

        private void AddValidatedNationalClass(IrmaContext context, HierarchyClass hierarchyClass, int irmaId)
        {
            context.ValidatedNationalClass.Add(new ValidatedNationalClass()
                {
                    IrmaId = irmaId,
                    IconId = hierarchyClass.hierarchyClassID,
                    Level = hierarchyClass.hierarchyLevel,
                    InsertDate = DateTime.Now
                });
        }
    }
}
