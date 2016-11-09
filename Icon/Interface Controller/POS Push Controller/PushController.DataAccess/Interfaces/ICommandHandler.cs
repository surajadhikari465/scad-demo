
namespace PushController.DataAccess.Interfaces
{
    public interface ICommandHandler<TCommand>
    {
        void Execute(TCommand command);
    }
}
