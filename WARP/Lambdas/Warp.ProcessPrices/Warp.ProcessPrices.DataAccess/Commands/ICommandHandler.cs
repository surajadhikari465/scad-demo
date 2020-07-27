namespace Warp.ProcessPrices.DataAccess.Commands
{
    public interface ICommandHandler<TData>
    {
        void Execute(TData data);
    }
}