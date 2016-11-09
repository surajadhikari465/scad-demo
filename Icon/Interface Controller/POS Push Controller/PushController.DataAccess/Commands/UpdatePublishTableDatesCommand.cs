using Irma.Framework;
using System;
using System.Collections.Generic;

namespace PushController.DataAccess.Commands
{
    public class UpdatePublishTableDatesCommand
    {
        public IrmaContext Context { get; set; }
        public bool ProcessedSuccessfully { get; set; }
        public List<IConPOSPushPublish> PublishedPosData { get; set; }
        public DateTime Date { get; set; }
    }
}
