using System;
using System.Collections.Generic;
using System.Text;

namespace KitBuilder.DataAccess.Enums
{
    public enum Status
    {
        Disabled = 1,
        Building = 2,
        PublishQueued = 3,
        Published = 4,
        Modifying = 5,
        PublishFailed = 6,
        PublishReQueued = 7,
        PartiallyPublished = 8
   }
}
