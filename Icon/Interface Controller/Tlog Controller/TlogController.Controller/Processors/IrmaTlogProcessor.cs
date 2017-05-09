using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TlogController.Common;
using TlogController.Controller.ProcessorModules;
using TlogController.DataAccess.Models;

namespace TlogController.Controller.Processors
{
    public class IrmaTlogProcessor : IIrmaTlogProcessor
    {
        private IIconTlogProcessorModule iconTlogProcessorModule;
        private IIrmaTlogProcessorModule irmaTlogProcessorModule;
        private IrmaTlog irmaTlog;

        public IrmaTlogProcessor(
            IIconTlogProcessorModule iconTlogProcessorModule,
            IIrmaTlogProcessorModule irmaTlogProcessorModule,
            IrmaTlog irmaTlog)
        {
            this.iconTlogProcessorModule = iconTlogProcessorModule;
            this.irmaTlogProcessorModule = irmaTlogProcessorModule;
            this.irmaTlog = irmaTlog;
        }

        public void UpdateSalesSumByitem()
        {
            try
            {
                irmaTlogProcessorModule.PushSalesSumByitemDataInBulkToIrma(irmaTlog);
            }
            catch (Exception)
            {
                List<IrmaTlog> irmaTlogList = SplitRegionalIrmaTlog(irmaTlog);

                foreach (IrmaTlog irmaTlogSplit in irmaTlogList)
                {
                    try
                    {
                        irmaTlogProcessorModule.PushSalesSumByitemDataInBulkToIrma(irmaTlogSplit);
                    }
                    catch (Exception)
                    {
                        irmaTlogProcessorModule.PushSalesSumByitemDataTransactionByTransactionToIrma(irmaTlogSplit);
                    }
                }
            }
        }

        private List<IrmaTlog> SplitRegionalIrmaTlog(IrmaTlog irmaTlog)
        {
            List<IrmaTlog> irmaTlogList = new List<IrmaTlog>();

            if (irmaTlog.ItemMovementTransactionList != null && irmaTlog.ItemMovementTransactionList.Count > StartupOptions.MaxTlogTransactionsWhenSplit)
            {
                List<List<ItemMovementTransaction>> itemMovementTransactionLists = irmaTlog.ItemMovementTransactionList.Split(StartupOptions.MaxTlogTransactionsWhenSplit);
                foreach (List<ItemMovementTransaction> itemMovementTransactionList in itemMovementTransactionLists)
                {
                    IrmaTlog irmaTlogSplit = new IrmaTlog();
                    irmaTlogSplit.ItemMovementTransactionList = itemMovementTransactionList;

                    int itemMovementToIrmaListStartedIndex = itemMovementTransactionList[0].FirstItemMovementToIrmaIndex;
                    int itemMovementToIrmaListEndedIndex = itemMovementTransactionList[itemMovementTransactionList.Count - 1].LastItemMovementToIrmaIndex;
                    irmaTlogSplit.ItemMovementToIrmaList = irmaTlog.ItemMovementToIrmaList.GetRange(itemMovementToIrmaListStartedIndex, itemMovementToIrmaListEndedIndex - itemMovementToIrmaListStartedIndex + 1);
                    irmaTlogSplit.RegionCode = irmaTlog.RegionCode;

                    int originalFirstItemMovementToIrmaIndex = irmaTlogSplit.ItemMovementTransactionList[0].FirstItemMovementToIrmaIndex;
                    foreach (ItemMovementTransaction itemMovementTransaction in irmaTlogSplit.ItemMovementTransactionList)
                    {
                        itemMovementTransaction.FirstItemMovementToIrmaIndex = itemMovementTransaction.FirstItemMovementToIrmaIndex - originalFirstItemMovementToIrmaIndex;
                        itemMovementTransaction.LastItemMovementToIrmaIndex = itemMovementTransaction.LastItemMovementToIrmaIndex - originalFirstItemMovementToIrmaIndex;
                    }

                    irmaTlogList.Add(irmaTlogSplit);
                }
            }
            else
            {
                irmaTlogList.Add(irmaTlog);
            }
            return irmaTlogList;
        }
    }
}
