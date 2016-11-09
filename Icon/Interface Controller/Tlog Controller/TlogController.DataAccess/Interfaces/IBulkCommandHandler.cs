namespace TlogController.DataAccess.Interfaces
{
    public interface IBulkCommandHandler<TCommand>
    {
        int Execute(TCommand command);
    }
}
