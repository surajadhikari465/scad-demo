
namespace Mammoth.Common.DataAccess.CommandQuery
{
    public interface ICommandHandler<TData>
    {
        void Execute(TData data);
    }
}
