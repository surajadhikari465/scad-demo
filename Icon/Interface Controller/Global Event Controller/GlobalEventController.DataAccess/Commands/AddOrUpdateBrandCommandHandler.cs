using GlobalEventController.DataAccess.Infrastructure;
using InterfaceController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irma.Framework;
using Icon.Logging;
using GlobalEventController.Common;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddOrUpdateBrandCommandHandler : ICommandHandler<AddOrUpdateBrandCommand>
    {
        private readonly IrmaContext context;
        private ILogger<AddOrUpdateBrandCommandHandler> logger;

        public AddOrUpdateBrandCommandHandler(IrmaContext context, ILogger<AddOrUpdateBrandCommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Handle(AddOrUpdateBrandCommand command)
        {
            var validatedBrand = context.ValidatedBrand.SingleOrDefault(vb => vb.IconBrandId == command.IconBrandId);

            if (validatedBrand == null)
            {
                var irmaBrand = context.ItemBrand.SingleOrDefault(ib => ib.Brand_Name == command.BrandName);
                if (irmaBrand == null)
                {
                    validatedBrand = AddValidatedBrandAndIrmaBrand(command);
                }
                else
                {
                    validatedBrand = ValidateExistingIrmaBrand(command, irmaBrand);
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

        private ValidatedBrand AddValidatedBrandAndIrmaBrand(AddOrUpdateBrandCommand command)
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

        private ValidatedBrand ValidateExistingIrmaBrand(AddOrUpdateBrandCommand command, ItemBrand irmaBrand)
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
