namespace OutOfStock.Classes
{
    public class DepartmentCountData
    {
        public int DepartmentId;
        public string DepartmentName;
        public int ItemCount;

        public DepartmentCountData()
        {
        }

        public DepartmentCountData(int id, string name, int count)
        {
            this.DepartmentId = id;
            this.DepartmentName = name;
            this.ItemCount = count;
        }

    }
}