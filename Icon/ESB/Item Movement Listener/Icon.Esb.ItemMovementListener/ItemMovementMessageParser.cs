using Icon.Esb.ItemMovementListener.Models;
using Icon.Esb.Subscriber;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Esb.ItemMovementListener
{
    public class ItemMovementMessageParser
    {
        private XmlSerializer serializer;
        private TextReader textReader;

        public ItemMovementMessageParser()
        {
            serializer = new XmlSerializer(typeof(Contracts.ItemMovementTransactionType));
        }

        public TransactionModel ParseMessage(IEsbMessage message)
        {
            Contracts.ItemMovementTransactionType itemMovementTransactionType;

            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(message.MessageText)))
            {
                itemMovementTransactionType = serializer.Deserialize(textReader) as Contracts.ItemMovementTransactionType;
            }

            TransactionModel transaction = new TransactionModel()
            {
                ESBMessageID = itemMovementTransactionType.transactionNumber,
                TransactionSequenceNumber = itemMovementTransactionType.sequenceNumber.ToString(),
                StoreNumber = itemMovementTransactionType.locale.id.ToString(),
                CashierID = ((Contracts.IndividualType)(itemMovementTransactionType.party.type.Item)).personas[0].personaIdentifiers[0].value.ToString(),
                RegisterNumber = itemMovementTransactionType.locale.store.pointOfSaleRegisters.types[0].register[0].id.ToString()
            };

            List<LineItemModel> lineItems = new List<LineItemModel>();

            int totalLineItems = itemMovementTransactionType.retailTransaction.lineItems.Count();
            for (int i = 0; i < totalLineItems; i++)
            {
                LineItemModel lineItem = new LineItemModel();

                lineItem.VoidFlag = itemMovementTransactionType.retailTransaction.lineItems[i].isVoided;
                lineItem.SubTeam = itemMovementTransactionType.retailTransaction.lineItems[i].subTeam;
                lineItem.SequenceNumber = itemMovementTransactionType.retailTransaction.lineItems[i].sequenceNumber.ToString();
                lineItem.EndDateTime = itemMovementTransactionType.retailTransaction.lineItems[i].endDateTIme.ToString();

                Contracts.SaleType saleItem = itemMovementTransactionType.retailTransaction.lineItems[i].type.Item as Contracts.SaleType;

                if (saleItem != null)
                {
                    lineItem.Sale_LineItem = new LineItemTypeDataModel();

                    lineItem.Sale_LineItem.ItemId = saleItem.itemId.ToString();
                    lineItem.Sale_LineItem.Quantity = saleItem.quantity.value.ToString();
                    lineItem.Sale_LineItem.Units = saleItem.quantity.units.uom.code.ToString();
                    lineItem.Sale_LineItem.RegularSalesUnitPrice = saleItem.price.priceAmount.amount.ToString();

                    if (saleItem.extendedDiscountAmount != null)
                        lineItem.Sale_LineItem.ExtendedDiscountAmount = saleItem.extendedDiscountAmount.amount.ToString();
                    else
                        lineItem.Sale_LineItem.ExtendedDiscountAmount = lineItem.Sale_LineItem.RegularSalesUnitPrice;
                }

                Contracts.ReturnType returnItem = itemMovementTransactionType.retailTransaction.lineItems[i].type.Item as Contracts.ReturnType;

                if (returnItem != null)
                {
                    lineItem.Return_LineItem = new LineItemTypeDataModel();

                    lineItem.Return_LineItem.ItemId = returnItem.itemId.ToString();
                    lineItem.Return_LineItem.Quantity = returnItem.quantity.value.ToString();
                    lineItem.Return_LineItem.Units = returnItem.quantity.units.uom.code.ToString();
                    lineItem.Return_LineItem.RegularSalesUnitPrice = returnItem.price.priceAmount.amount.ToString();

                    if (returnItem.extendedDiscountAmount != null)
                        lineItem.Return_LineItem.ExtendedDiscountAmount = returnItem.extendedDiscountAmount.amount.ToString();
                    else
                        lineItem.Return_LineItem.ExtendedDiscountAmount = lineItem.Return_LineItem.RegularSalesUnitPrice;
                }

                lineItems.Add(lineItem);
            }

            transaction.RetailTransaction = new RetailTransactionModel();

            transaction.RetailTransaction.lineItems = lineItems;

            return transaction;
        }
    }
}
