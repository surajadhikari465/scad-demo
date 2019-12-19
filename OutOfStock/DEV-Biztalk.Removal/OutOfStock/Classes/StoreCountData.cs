using System.Collections.Generic;

namespace OutOfStock.Classes
{
    public class StoreCountData
    {
        public int StoreId;
        public string StoreName;
        public List<DepartmentCountData> DepartmentCountInfo;

        public StoreCountData()
        {
        }

        public StoreCountData(int id, string name, List<DepartmentCountData> countinfo)
        {
            this.StoreId = id;
            this.StoreName = name;
            this.DepartmentCountInfo = countinfo;
        }
    }
}