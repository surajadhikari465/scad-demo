using System;
using OOSCommon;

namespace OOS.Model
{
    public interface IKnownUploadRepository
    {
        void Insert(IKnownUpload upload);
        IKnownUpload For(DateTime date);
        void Modify(IKnownUpload upload);
        void Remove(DateTime date);
        void Reset();
        bool ExistProductStatusProjection(ProductStatus projection);
    }
}
