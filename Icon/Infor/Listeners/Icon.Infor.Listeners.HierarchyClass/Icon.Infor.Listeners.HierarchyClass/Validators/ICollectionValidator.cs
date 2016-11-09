﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Validators
{
    public interface ICollectionValidator<T>
    {
        void ValidateCollection(IEnumerable<T> collection);
    }
}
