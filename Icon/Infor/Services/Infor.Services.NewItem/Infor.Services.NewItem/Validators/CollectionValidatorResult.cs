using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Validators
{
    public class CollectionValidatorResult<T>
    {
        public IEnumerable<T> ValidEntities { get; set; }
        public IEnumerable<T> InvalidEntities { get; set; }

        public CollectionValidatorResult()
        {
            ValidEntities = new List<T>();
            InvalidEntities = new List<T>();
        }
    }
}
