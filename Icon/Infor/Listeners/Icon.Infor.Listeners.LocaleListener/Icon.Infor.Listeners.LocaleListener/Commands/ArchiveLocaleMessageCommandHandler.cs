using FastMember;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Infor.Listeners.LocaleListener.Models;
using Dapper;
using Icon.Framework;

namespace Icon.Infor.Listeners.LocaleListener.Commands
{
    public class ArchiveLocaleMessageCommandHandler : ICommandHandler<ArchiveLocaleMessageCommand>
    {
        private readonly string connectionString;

        public ArchiveLocaleMessageCommandHandler(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Execute(ArchiveLocaleMessageCommand data)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var messageId = Guid.Parse(data.Message.GetProperty("MessageID"));

                using (var sqlBulkCopy = new SqlBulkCopy(connection))
                {
                    using (var reader = ObjectReader.Create(GetArchiveModel(data.Locale, messageId),
                        nameof(MessageArchiveLocaleModel.MessageArchiveId),
                        nameof(MessageArchiveLocaleModel.LocaleId),
                        nameof(MessageArchiveLocaleModel.BusinessUnitId),
                        nameof(MessageArchiveLocaleModel.LocaleName),
                        nameof(MessageArchiveLocaleModel.LocaleTypeCode),
                        nameof(MessageArchiveLocaleModel.InforMessageId),
                        nameof(MessageArchiveLocaleModel.Action),
                        nameof(MessageArchiveLocaleModel.Context),
                        nameof(MessageArchiveLocaleModel.ErrorCode),
                        nameof(MessageArchiveLocaleModel.ErrorDetails)))
                    {
                        sqlBulkCopy.DestinationTableName = "infor.MessageArchiveLocale";
                        sqlBulkCopy.WriteToServer(reader);
                    }
                }

                connection.Execute(
                    @"INSERT INTO infor.MessageHistory
                           (MessageTypeId
                           ,MessageStatusId
                           ,Message
                           ,InforMessageId)
                     VALUES
                           (@MessageTypeId
                           ,@MessageStatusId
                           ,@Message
                           ,@InforMessageId)",
                    new InforMessageHistoryModel
                    {
                        MessageTypeId = MessageTypes.Locale,
                        MessageStatusId = MessageStatusTypes.Consumed,
                        Message = data.Message.MessageText,
                        InforMessageId = messageId
                    });
            }
        }

        private IEnumerable<MessageArchiveLocaleModel> GetArchiveModel(LocaleModel localeModel, Guid messageId)
        {
            var archiveModels = new List<MessageArchiveLocaleModel>();

            archiveModels.Add(new MessageArchiveLocaleModel(localeModel, messageId));
            AddChildren(localeModel, archiveModels, messageId);

            return archiveModels;
        }

        private void AddChildren(LocaleModel localeModel, List<MessageArchiveLocaleModel> archiveModels, Guid messageId)
        {
            if (localeModel.Locales != null && localeModel.Locales.Any())
            {
                foreach (var childLocaleModel in localeModel.Locales)
                {
                    archiveModels.Add(new MessageArchiveLocaleModel(childLocaleModel, messageId));
                    AddChildren(childLocaleModel, archiveModels, messageId);
                }
            }
        }
    }
}
