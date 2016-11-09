namespace Vim.Common.DataAccess
{
    public interface ICommandHandler<TData>
    {
        void Execute(TData data);
    }
}
