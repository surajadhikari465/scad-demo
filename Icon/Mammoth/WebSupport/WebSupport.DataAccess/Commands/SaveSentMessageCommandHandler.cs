using Icon.Common.DataAccess;
using Mammoth.Common.DataAccess;
using Mammoth.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebSupport.DataAccess.Commands
{
    public class SaveSentMessageCommandHandler : ICommandHandler<SaveSentMessageCommand>
    {
        private MammothContext context;

        public SaveSentMessageCommandHandler(MammothContext context)
        {
            this.context = context;
        }

        public void Execute(SaveSentMessageCommand data)
        {
            context.PriceResetMessageHistories.Add(new PriceResetMessageHistory
            {
                Message = XDocument.Parse(data.Message).ToString(),
                MessageId = data.MessageId,
                MessageStatusId = MessageStatusTypes.Sent,
                MessageTypeId = MessageTypes.Price,
                MessageProperties = JsonConvert.SerializeObject(data.MessageProperties)
            });
            context.SaveChanges();
        }
    }
}
