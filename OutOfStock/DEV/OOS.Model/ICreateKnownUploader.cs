using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon.Import;

namespace OOS.Model
{
    public interface ICreateKnownUploader
    {
        IOOSUpdateKnown Make();
    }
}
