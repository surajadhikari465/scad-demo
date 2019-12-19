using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    public class MockLogService
    {
        public static ILogService New()
        {
            var logger = MockRepository.GenerateStub<IOOSLog>();
            var logService = MockRepository.GenerateStub<ILogService>();
            logService.Stub(p => p.GetLogger()).Return(logger);
            return logService;
        }
    }
}
