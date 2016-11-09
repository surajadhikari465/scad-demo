using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Validators
{
    public interface ICollectionValidator<T>
    {
        CollectionValidatorResult<T> Validate(IEnumerable<T> collection);
    }
}
