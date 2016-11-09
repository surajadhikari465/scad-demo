using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ItemMovement;
using Icon.Esb.ItemMovementListener.Commands;
using Icon.Esb.ItemMovementListener.Models;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using System;
using System.Collections.Generic;
using TIBCO.EMS;

namespace Icon.Esb.ItemMovementListener
{
    public class ItemMovementListener : ListenerApplication<ItemMovementListener, ListenerApplicationSettings>
    {
        private ICommandHandler<SaveItemMovementTransactionCommand> itemMovementTransactionCommandHandler;
        private ItemMovementMessageParser messageParser;
        private List<IRMATransactionModel> transactions = new List<IRMATransactionModel>();
        private ItemMovementListenerSettings listenerSettings;
        
        public ItemMovementListener(
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ItemMovementListener> logger,
            ICommandHandler<SaveItemMovementTransactionCommand> ItemMovementTransactionCommandHandler,
            ItemMovementListenerSettings listenerSettings)
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.itemMovementTransactionCommandHandler = ItemMovementTransactionCommandHandler;
            this.listenerSettings = listenerSettings;
            
            messageParser = new ItemMovementMessageParser();
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            try
            {
                TransactionModel transaction = messageParser.ParseMessage(args.Message);

                foreach (LineItemModel lineItem in transaction.RetailTransaction.lineItems)
                {
                    IRMATransactionModel trans = new IRMATransactionModel();

                    trans.BusinessUnitId = Convert.ToInt32(transaction.StoreNumber);
                    trans.RegisterNumber = Convert.ToInt32(transaction.RegisterNumber);

                    trans.TransDate = Convert.ToDateTime(lineItem.EndDateTime);

                    trans.ESBMessageID = transaction.ESBMessageID;

                    trans.TransactionSequenceNumber = Convert.ToInt32(transaction.TransactionSequenceNumber);
                    trans.LineItemNumber = Convert.ToInt32(lineItem.SequenceNumber);

                    trans.ItemVoid = lineItem.VoidFlag ? true : false;

                    if (lineItem.Sale_LineItem != null)
                    {
                        trans.Identifier = lineItem.Sale_LineItem.ItemId;

                        if (lineItem.Sale_LineItem.Units == "EA")
                        {

                            trans.Weight = Convert.ToDecimal(0.0);
                            trans.Quantity = Convert.ToInt32(lineItem.Sale_LineItem.Quantity);
                        }
                        else
                        {
                            trans.Weight = Convert.ToDecimal(lineItem.Sale_LineItem.Quantity);
                            trans.Quantity = 0;
                        }

                        trans.ItemType = 0;

                        trans.BasePrice = Convert.ToDecimal(lineItem.Sale_LineItem.RegularSalesUnitPrice);
                        trans.MarkDownAmount = Convert.ToDecimal(lineItem.Sale_LineItem.ExtendedDiscountAmount);
                    }
                    else
                    {
                        trans.Identifier = lineItem.Return_LineItem.ItemId;

                        if (lineItem.Return_LineItem.Units == "EA")
                        {
                            trans.Weight = Convert.ToDecimal(0.0);
                            trans.Quantity = Convert.ToInt32(lineItem.Return_LineItem.Quantity);
                        }
                        else
                        {
                            trans.Weight = Convert.ToDecimal(lineItem.Return_LineItem.Quantity);
                            trans.Quantity = 0;
                        }

                        trans.ItemType = 2;

                        trans.BasePrice = Convert.ToDecimal(lineItem.Return_LineItem.RegularSalesUnitPrice);
                        trans.MarkDownAmount = Convert.ToDecimal(lineItem.Return_LineItem.ExtendedDiscountAmount);
                    }

                    transactions.Add(trans);
                }

                if (transactions.Count >= listenerSettings.MessageQueueSize)
                {
                    var parameters = new SaveItemMovementTransactionCommand
                    {
                        ItemMovementTransactions = transactions
                    };

                    itemMovementTransactionCommandHandler.Execute(parameters);

                    transactions.Clear();
                }
            }
            catch (Exception ex)
            {
                LogAndNotifyErrorWithMessage(ex, args);
                transactions.Clear();
            }

            if ((esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge))
            {
                args.Message.Acknowledge();
            }

            if (listenerSettings.PerformanceLoggingEnabled)
            {
                logger.Info("Finished processing Item Movement message.");
            }
        }
    }
}

