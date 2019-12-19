using System;

namespace OOSCommon.Import
{
    public interface IOOSUpdateKnown
    {
        bool BeginBatch(DateTime uploadDate, IOOSImportKnown importKnown);
        bool WriteKnownOOS(IOOSImportKnown importKnown);
        bool Upload(IKnownUpload uploadDoc);
    }
}
