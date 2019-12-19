using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon.Import;

namespace OutOfStock.KnownUploadWebService
{
    public interface IKnownUploadMapper
    {
        KnownUpload MapKnownUpload(KnownUploadDocument doc);
    }
}
