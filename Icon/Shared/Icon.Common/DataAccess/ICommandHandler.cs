
namespace Icon.Common.DataAccess
{
    public interface ICommandHandler<TData>
    {
        void Execute(TData data);
    }
}
