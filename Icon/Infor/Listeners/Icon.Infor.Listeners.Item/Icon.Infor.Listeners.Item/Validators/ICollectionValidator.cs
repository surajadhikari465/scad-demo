﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Validators
{
    public interface ICollectionValidator<T>
    {
        void ValidateCollection(IEnumerable<T> collection);
    }
}
