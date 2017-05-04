using FluentValidation;
using Icon.Infor.Listeners.Price.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.Validators
{
    public class PriceModelValidator : AbstractValidator<PriceModel>, ICollectionValidator<PriceModel>
    {
        public void ValidateCollection(IEnumerable<PriceModel> collection)
        {
            throw new NotImplementedException();
        }
    }
}
