using Icon.Common.DataAccess;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitBuilder.ESB.Listeners.Item.Service.Commands
{
    public class ArchiveMessageCommandHandler : ICommandHandler<ArchiveMessageCommand>
    {
        private IRepository<Items> repo;
        public ArchiveMessageCommandHandler(IRepository<Items> itemRepository)
        {
            this.repo = itemRepository;
        }
        public void Execute(ArchiveMessageCommand data)
        {

            var MessageType = new SqlParameter("MessageTypeId", 1); // Item MessageType
            var Message = new SqlParameter("Message", SqlDbType.VarChar);
            Message.Value = data.Message.MessageText;
            var MessageHeader = new SqlParameter("MessageHeader", DBNull.Value);
           

            var parameters = new List<SqlParameter>();
            parameters.Add(MessageType);
            parameters.Add(Message);
            parameters.Add(MessageHeader);

            repo.UnitOfWork.Context.Database.ExecuteSqlCommand("app.MessageHistoryInsert @MessageTypeId, @Message, @MessageHeader", parameters);
          
        }
    }
}
