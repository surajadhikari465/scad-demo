namespace KitBuilderWebApi.QueryParameters
{
    public abstract class BaseParameters
    {
        const int maxPageSize = 1000;
        public int PageNumber { get; set; } = 1;

        private int pageSize = 30;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    
        public string OrderBy { get; set; }
        public string Fields { get; set; }
    }
}