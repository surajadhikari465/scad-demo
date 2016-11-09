namespace RegionalEventController.DataAccess.Interfaces
{
    public interface ICommandHandler<TCommand>
    {
        void Execute(TCommand command);
    }
}