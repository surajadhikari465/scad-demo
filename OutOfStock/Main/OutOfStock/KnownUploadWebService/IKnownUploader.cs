using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OOSCommon.Import;

namespace OutOfStock.KnownUploadWebService
{
    [ServiceContract(Namespace = "http://schemas.wholefoods.com/knownupload")]
    public interface IKnownUploader
    {
        [OperationContract]
        void Upload(KnownUploadDocument doc);
    }
}
