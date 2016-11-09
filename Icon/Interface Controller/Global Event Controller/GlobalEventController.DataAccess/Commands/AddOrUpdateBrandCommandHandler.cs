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
                validatedBrand = new ValidatedBrand
                {
                    IconBrandId = command.IconBrandId.Value,
                    ItemBrand = new ItemBrand
                    {
                        Brand_Name = command.BrandName
                    }
                };

                context.ValidatedBrand.Add(validatedBrand);
            }
            else
            {
                validatedBrand.ItemBrand.Brand_Name = command.BrandName;
            }

            context.SaveChanges();

            command.BrandId = validatedBrand.ItemBrand.Brand_ID;
        }
    }
}
