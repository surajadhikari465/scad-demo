using System;
using OOS.Model;

namespace OutOfStock.ScanProcessor
{
    public class ScanProcessorUserProfile : IUserProfile
    {
        public bool IsRegionBuyer()
        {
            throw new NotImplementedException();
        }

        public string UserStoreAbbreviation()
        {
            throw new NotImplementedException();
        }

        public bool IsCentral()
        {
            throw new NotImplementedException();
        }

        public string UserRegion()
        {
            throw new NotImplementedException();
        }

        public bool IsStoreLevel()
        {
            throw new NotImplementedException();
        }

        public string UserName { get; }
    }
}