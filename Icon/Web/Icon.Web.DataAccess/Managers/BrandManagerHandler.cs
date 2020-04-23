using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;
using Icon.Common;

namespace Icon.Web.DataAccess.Managers
{
    public class BrandManagerHandler : IManagerHandler<BrandManager>
    {
        private IconContext context;
        private ICommandHandler<BrandCommand> updateBrandCommandHandler;
        private ICommandHandler<UpdateBrandHierarchyClassTraitsCommand> updateHierarchyClassTraitsCommandHandler;
        private ICommandHandler<AddBrandMessageCommand> addBrandMessageCommandHandler;
        private IMapper mapper;

        public BrandManagerHandler(
            IconContext context,
            ICommandHandler<BrandCommand> updateBrandCommandHandler,
            ICommandHandler<UpdateBrandHierarchyClassTraitsCommand> updateHierarchyClassTraitsCommandHandler,
            ICommandHandler<AddBrandMessageCommand> addBrandMessageCommandHandler,
            IMapper mapper)
        {
            this.context = context;
            this.updateBrandCommandHandler = updateBrandCommandHandler;
            this.updateHierarchyClassTraitsCommandHandler = updateHierarchyClassTraitsCommandHandler;
            this.addBrandMessageCommandHandler = addBrandMessageCommandHandler;
            this.mapper = mapper;
        }

        public void Execute(BrandManager data)
        {
            BrandCommand command = mapper.Map<BrandCommand>(data);
            UpdateBrandHierarchyClassTraitsCommand updateHierarchyClassTraitCommand = mapper.Map<UpdateBrandHierarchyClassTraitsCommand>(data);

            try
            {
                Validate(data);
                updateBrandCommandHandler.Execute(command);
                addBrandMessageCommandHandler.Execute(new AddBrandMessageCommand()
                    {
                        Brand = data.Brand,
                        BrandAbbreviation = data.BrandAbbreviation,
                        Designation = data.Designation,
                        ParentCompany = data.ParentCompany,
                        ZipCode = data.ZipCode,
                        Locality = data.Locality,
                        Action = MessageActionTypes.AddOrUpdate

                    });
                updateHierarchyClassTraitsCommandHandler.Execute(updateHierarchyClassTraitCommand);
            }
            catch (DuplicateValueException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException($"An error occurred when processing Brand {data.Brand.hierarchyClassName} (ID: {data.Brand.hierarchyClassID.ToString()}).", ex);
            }
        }

        void Validate(BrandManager data)
        {
            using (var context = new IconContext())
            {
                data.Brand.hierarchyClassName = String.IsNullOrWhiteSpace(data.Brand.hierarchyClassName) ? null : data.Brand.hierarchyClassName.Trim();
                data.BrandAbbreviation = string.IsNullOrWhiteSpace(data.BrandAbbreviation) ? null : data.BrandAbbreviation.Trim();

                if (data.Brand.hierarchyClassName == null)
                {
                    throw new Exception("The brand name is missing.");
                }

                bool isTrimmed = (data.Brand.hierarchyClassName.Length >= Constants.IrmaBrandNameMaxLength);

                if (context.HierarchyClass.Any(x => x.hierarchyID == Hierarchies.Brands && x.hierarchyClassID != data.Brand.hierarchyClassID && String.Compare(x.hierarchyClassName, data.Brand.hierarchyClassName, true) == 0))
                {
                    throw new DuplicateValueException($"The brand {data.Brand.hierarchyClassName} already exists.");
                }

                if (isTrimmed)
                {
                    var irmaBrandName = data.Brand.hierarchyClassName.Substring(0, Constants.IrmaBrandNameMaxLength);

                    if (context.HierarchyClass.Any(x => x.Hierarchy.hierarchyName == HierarchyNames.Brands && x.hierarchyClassName.StartsWith(irmaBrandName) && x.hierarchyClassID != data.Brand.hierarchyClassID))
                    {
                        throw new DuplicateValueException($"Brand name trimmed to {Constants.IrmaBrandNameMaxLength.ToString()} characters {irmaBrandName} already exists. Change the brand name so that the first {Constants.IrmaBrandNameMaxLength.ToString()} characters are unique.");
                    }
                }

                if (data.BrandAbbreviation != null)
                {
                    if (context.HierarchyClassTrait.Any(x => x.hierarchyClassID != data.Brand.hierarchyClassID
                         && x.Trait.traitCode == TraitCodes.BrandAbbreviation
                         && String.Compare(x.traitValue, data.BrandAbbreviation, true) == 0))
                    {
                        throw new DuplicateValueException(String.Format("The brand abbreviation {0} already exists.", data.BrandAbbreviation));
                    }
                }
            }
        }
    }
}