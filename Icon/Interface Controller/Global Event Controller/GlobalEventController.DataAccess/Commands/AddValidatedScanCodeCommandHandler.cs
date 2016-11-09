using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using System.Data.Entity;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddValidatedScanCodeCommandHandler : ICommandHandler<AddValidatedScanCodeCommand>
    {
        private readonly IrmaContext context;

        public AddValidatedScanCodeCommandHandler(IrmaContext context)
        {
            this.context = context;
        }
        public void Handle(AddValidatedScanCodeCommand command)
        {
            // Get existing scanCodes
            List<string> validatedScanCodes = this.context.ValidatedScanCode.Where(vsc => command.ScanCodes.Contains(vsc.ScanCode)).Select(vsc => vsc.ScanCode).ToList();

            // Add Scan Codes that were not found
            List<ValidatedScanCode> scanCodesToAdd = command.ScanCodes
                .Except(validatedScanCodes)
                .Select(scanCode => new ValidatedScanCode { ScanCode = scanCode, InsertDate = DateTime.Now })
                .ToList();

            this.context.ValidatedScanCode.AddRange(scanCodesToAdd);
        }
    }
}
