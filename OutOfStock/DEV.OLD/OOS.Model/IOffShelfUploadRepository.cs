using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model.Repository;
using SharedKernel;

namespace OOS.Model
{
    public interface IOffShelfUploadRepository
    {
        void Add(OffShelfUpload newUpload);
        OffShelfUpload Find(string scanDate);
        IEnumerable<OffShelfUpload> FindBetween(string from, string to);
        OffShelfUpload Find(DateTime scanDate);
        IEnumerable<OffShelfUpload> FindAll();
    }
}
