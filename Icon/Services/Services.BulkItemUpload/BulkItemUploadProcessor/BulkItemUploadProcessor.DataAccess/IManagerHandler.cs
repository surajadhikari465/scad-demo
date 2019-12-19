namespace BulkItemUploadProcessor.DataAccess
{
    public interface IManagerHandler<TManager>
    {
        void Execute(TManager data);
    }
}