﻿using Icon.Common.Xml;
using Icon.DbContextFactory;
using Icon.Esb.Schemas.Mammoth;
using IrmaPriceListenerService.Serializer;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace IrmaPriceListenerService.Archive
{
    public class ErrorEventPublisher : IErrorEventPublisher
    {
        private IDbContextFactory<MammothContext> mammothContextFactory;
        private IrmaPriceListenerServiceSettings settings;
        private ISerializer<ErrorMessage> serializer;
        private static readonly IList<string> fatalErrors = new List<string>()
        {
            "Connection is closed",
            "com.microsoft.sqlserver.jdbc.SQLServerException"
        };

        public ErrorEventPublisher(
            IDbContextFactory<MammothContext> mammothContextFactory,
            IrmaPriceListenerServiceSettings settings,
            ISerializer<ErrorMessage> serializer
        )
        {
            this.mammothContextFactory = mammothContextFactory;
            this.settings = settings;
            this.serializer = serializer;
        }

        public void PublishErrorMessage(SQSExtendedClientReceiveModel message, Exception ex)
        {
            string errorSeverity = Constants.ErrorSeverity.Error;
            if (fatalErrors.Any((fatalError) => ex.Message.Contains(fatalError) || ex.Message.Contains(fatalError)))
            {
                errorSeverity = Constants.ErrorSeverity.Fatal;
            }

            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = 10;
                string storeErrorSqlStatement = @"INSERT INTO gpm.ErrorMessages (Application, MessageID, MessageProperties, Message, ErrorCode, ErrorDetails, ErrorSeverity) 
                    VALUES (@Application, @MessageID, @MessageProperties, @Message, @ErrorCode, @ErrorDetails, @ErrorSeverity)";

                mammothContext.Database.ExecuteSqlCommand(
                    storeErrorSqlStatement,
                    new SqlParameter("@Application", settings.ApplicationName),
                    new SqlParameter("@MessageID", message.MessageAttributes[Constants.MessageAttribute.TransactionId]),
                    new SqlParameter("@MessageProperties", ConvertPropertiesToXml(message.MessageAttributes)),
                    new SqlParameter("@Message", message.S3Details[0].Data),
                    new SqlParameter("@ErrorCode", ex.GetType()),
                    new SqlParameter("@ErrorDetails", ex.Message),
                    new SqlParameter("@ErrorSeverity", errorSeverity)
                );
            }
        }

        private string ConvertPropertiesToXml(IDictionary<string, string> messageAttributes)
        {
            IList<NameValuePair> nameValuePairs = new List<NameValuePair>();
            foreach (var keyValue in messageAttributes)
            {
                nameValuePairs.Add(new NameValuePair()
                {
                    name = keyValue.Key,
                    value = keyValue.Value
                });
            }

            var errorMessage = new ErrorMessage()
            {
                MessageProperties = nameValuePairs.ToArray()
            };

            return serializer.Serialize(errorMessage, new Utf8StringWriter());
        }
    }
}
