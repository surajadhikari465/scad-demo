using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Mappers
{
    public class RowObjectToBrandMapperResponse<T>
    {
        public List<T> Brands { get; set; }
        public Dictionary<T, RowObject> BrandToRowDictionary { get; set; }

        public RowObject GetItemsRowObject(T item)
        {
            if (BrandToRowDictionary.ContainsKey(item))
            {
                return BrandToRowDictionary[item];
            }
            else
            {
                return null;
            }
        }
    }
}
