namespace BulkItemUploadProcessor.Common
{
    public class ErrorItem<T>
    {
        public T Item { get; set; }
        public string Error { get; set; }

        public ErrorItem(T item, string error)
        {
            this.Item = item;
            this.Error = error;
        }
    }
}
