using System.Collections.Generic;
using OOSCommon;

namespace OOS.Model
{
    public interface ITranslateKnownUploadToProductStatusProjections
    {
        IEnumerable<ProductStatus> Translate(IKnownUpload upload);
    }
}
