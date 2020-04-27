using System;
using System.Collections.Generic;
using System.Linq;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Interfaces;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.DataAccess.Commands;
using BrandUploadProcessor.Service.Interfaces;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.Service
{
    public class AddUpdateBrandManager : IAddUpdateBrandManager
    {
        private readonly ICommandHandler<UpdateBrandsCommand> updateBrandsCommandHandler;
        private readonly ICommandHandler<AddBrandsCommand> addBrandsCommandHandler;
        private IErrorMessageBuilder errorMessageBuilder;

        public AddUpdateBrandManager(
            ICommandHandler<UpdateBrandsCommand> updateBrandsCommandHandler,
            ICommandHandler<AddBrandsCommand> addBrandsCommandHandler,
            IErrorMessageBuilder errorMessageBuilder
        )
        {
            this.updateBrandsCommandHandler = updateBrandsCommandHandler;
            this.addBrandsCommandHandler = addBrandsCommandHandler;
            this.errorMessageBuilder = errorMessageBuilder;
        }

        public void UpdateBrands(List<UpdateBrandModel> updateBrandModels,
            List<ErrorItem<UpdateBrandModel>> invalidBrands,
            List<int> updatedBrandIds
        )
        {
            var command = new UpdateBrandsCommand {Brands = updateBrandModels};

            try
            {
                updateBrandsCommandHandler.Execute(command);
                if (command.UpdatedBrandIds != null)
                {
                    updatedBrandIds.AddRange(command.UpdatedBrandIds);
                }

                if (command.InvalidBrands != null)
                {
                    invalidBrands.AddRange(command.InvalidBrands);
                }
            }

            catch (Exception ex)
            {
                if (command.Brands.Count > 1)
                {
                    UpdateBrands(command.Brands.Take(updateBrandModels.Count / 2).ToList(), invalidBrands, updatedBrandIds);
                    UpdateBrands(command.Brands.Skip(updateBrandModels.Count / 2).ToList(), invalidBrands, updatedBrandIds);
                }
                else
                {
                    var errorMessage = errorMessageBuilder.BuildErrorMessage(ex); //todo: do we still need this?
                    invalidBrands.Add(new ErrorItem<UpdateBrandModel>(command.Brands[0], errorMessage));
                }
            }
        }

        public void CreateBrands(List<AddBrandModel> addItemModels,
            List<ErrorItem<AddBrandModel>> invalidBrands,
            List<int> addedBrandIds)
        {
            var command = new AddBrandsCommand {Brands = addItemModels, AddedBrandIds = new List<int>(), InvalidBrands = new List<ErrorItem<AddBrandModel>>()};

            try
            {
                addBrandsCommandHandler.Execute(command);
                if (command.AddedBrandIds != null)
                {
                    addedBrandIds.AddRange(command.AddedBrandIds);
                }

                if (command.InvalidBrands != null)
                {
                    invalidBrands.AddRange(command.InvalidBrands);
                }
            }

            catch (Exception ex)
            {
                if (command.Brands.Count > 1)
                {
                    CreateBrands(command.Brands.Take(addItemModels.Count / 2).ToList(), invalidBrands, addedBrandIds);
                    CreateBrands(command.Brands.Skip(addItemModels.Count / 2).ToList(), invalidBrands, addedBrandIds);
                }
                else
                {
                    var errorMessage = errorMessageBuilder.BuildErrorMessage(ex);
                    invalidBrands.Add(new ErrorItem<AddBrandModel>(command.Brands[0], errorMessage));
                }
            }
        }
    }
}