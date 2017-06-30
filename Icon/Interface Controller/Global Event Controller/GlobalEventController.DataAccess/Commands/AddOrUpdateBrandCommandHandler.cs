using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Icon.Logging;
using Irma.Framework;
using System;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddOrUpdateBrandCommandHandler : ICommandHandler<AddOrUpdateBrandCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;
        private ILogger<AddOrUpdateBrandCommandHandler> logger;

        public AddOrUpdateBrandCommandHandler(IDbContextFactory<IrmaContext> contextFactory, ILogger<AddOrUpdateBrandCommandHandler> logger)
        {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        public void Handle(AddOrUpdateBrandCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                var validatedBrand = context.ValidatedBrand.SingleOrDefault(vb => vb.IconBrandId == command.IconBrandId);

                if (validatedBrand == null)
                {
                    var irmaBrand = context.ItemBrand.SingleOrDefault(ib => ib.Brand_Name == command.BrandName);
                    if (irmaBrand == null)
                    {
                        validatedBrand = AddValidatedBrandAndIrmaBrand(context, command);
                    }
                    else
                    {
                        validatedBrand = ValidateExistingIrmaBrand(context, command, irmaBrand);
                    }
                }
                else
                {
                    validatedBrand.ItemBrand.Brand_Name = command.BrandName;
                }

                validatedBrand.ItemBrand.LastUpdateTimestamp = DateTime.Now;

                context.SaveChanges();

                command.BrandId = validatedBrand.ItemBrand.Brand_ID;
            }
        }

        private ValidatedBrand AddValidatedBrandAndIrmaBrand(IrmaContext context, AddOrUpdateBrandCommand command)
        {
            ValidatedBrand validatedBrand = new ValidatedBrand
            {
                IconBrandId = command.IconBrandId.Value,
                ItemBrand = new ItemBrand
                {
                    Brand_Name = command.BrandName
                }
            };
            context.ValidatedBrand.Add(validatedBrand);
            return validatedBrand;
        }

        private ValidatedBrand ValidateExistingIrmaBrand(IrmaContext context, AddOrUpdateBrandCommand command, ItemBrand irmaBrand)
        {
            ValidatedBrand validatedBrand;
            var currentValidatedBrands = irmaBrand.ValidatedBrand.ToList();
            foreach (var currentValidatedBrand in currentValidatedBrands)
            {
                context.ValidatedBrand.Remove(currentValidatedBrand);
                context.SaveChanges();
            }
            validatedBrand = new ValidatedBrand { IconBrandId = command.IconBrandId.Value, ItemBrand = irmaBrand };
            context.ValidatedBrand.Add(validatedBrand);
            return validatedBrand;
        }
    }
}
