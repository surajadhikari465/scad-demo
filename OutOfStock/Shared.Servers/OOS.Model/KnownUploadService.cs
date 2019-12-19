using OOSCommon;
using OOSCommon.Import;

namespace OOS.Model
{
    public class KnownUploadService : IKnownUploadService
    {
        private ICreateKnownUploader uploaderFactory;

        public KnownUploadService(ICreateKnownUploader uploaderFactory)
        {
            this.uploaderFactory = uploaderFactory;
        }

        public bool Upload(IKnownUpload uploadDoc)
        {
            Validate(uploadDoc);

            return uploaderFactory.Make().Upload(uploadDoc);
        }

        private void Validate(IKnownUpload doc)
        {
            if (doc.ItemData == null)
                throw new KnownUploadValidationException("Known upload item data cannot be null");

            if (doc.VendorRegionMap == null)
                throw new KnownUploadValidationException("Known upload vendor region map cannot be null");
        }
    }
}
