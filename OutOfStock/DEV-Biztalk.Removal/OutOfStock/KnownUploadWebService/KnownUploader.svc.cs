using System;
using System.ServiceModel;
using OOS.Model;
using OOSCommon;
using OOSCommon.Import;
using StructureMap;

namespace OutOfStock.KnownUploadWebService
{
    [ServiceBehavior(Namespace = "http://schemas.wholefoods.com/knownupload")]
    public class KnownUploader : IKnownUploader
    {
        private KnownUploadService uploadService;
        private IKnownUploadMapper mapper;
        private IOOSLog log;

        public KnownUploader() : this(ObjectFactory.GetInstance<KnownUploadService>(), ObjectFactory.GetInstance<KnownUploadMapper>(), ObjectFactory.GetInstance<ILogService>())
        {
        }

        public KnownUploader(KnownUploadService uploadService, IKnownUploadMapper mapper, ILogService logService)
        {
            this.uploadService = uploadService;
            this.mapper = mapper;
            log = logService.GetLogger();
        }

        public void Upload(KnownUploadDocument doc)
        {
            try
            {
                uploadService.Upload(Map(doc));
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Upload() Error: {0}", ex.Message));
            }
        }

        private KnownUpload Map(KnownUploadDocument doc)
        {
            return mapper.MapKnownUpload(doc);
        }
    }
}
