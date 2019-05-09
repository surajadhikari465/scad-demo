using System;
using System.Collections.Generic;
using System.Text;

namespace KitBuilder.DataAccess.Enums
{
    public enum Status
    {
        Disabled = 1,
        Building = 2,
        ReadytoPublish =3,
        PublishQueued = 4,
        Published = 5,
        Modifying = 6,
        PublishFailed = 7,
        PublishReQueued = 8,
        PartiallyPublished = 9

   }
}
