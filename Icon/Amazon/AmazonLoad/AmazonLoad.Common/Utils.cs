using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.Common
{
    public static class Utils
    {
        public static int CalcBatchSize(int defaultBatchSize, int maxNumberOfRows, int numberOfRecordsSent)
        {
            int batchSize = defaultBatchSize;
            if (maxNumberOfRows != 0)
            {
                if (numberOfRecordsSent >= maxNumberOfRows)
                {
                    return -1;
                }
                if (maxNumberOfRows > 0)
                {
                    if (maxNumberOfRows - numberOfRecordsSent < batchSize)
                    {
                        batchSize = maxNumberOfRows - numberOfRecordsSent;
                    }
                    else if (maxNumberOfRows < batchSize)
                    {
                        batchSize = maxNumberOfRows;
                    }
                }
            }
            return batchSize;
        }
    }
}
